// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.Mediators.Tests;

public class CommandDispatcherTests : LoggedTestsBase
{
    public CommandDispatcherTests(ITestOutputHelper output) : base(output)
    {
    }

    private ServiceProvider BuildProvider(Action<IServiceCollection>? configure = null)
    {
        var services = new ServiceCollection();
        _ = services.AddLogging(ConfigureLogging);
        _ = services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        configure?.Invoke(services);
        return services.BuildServiceProvider();
    }

    // ---------- Happy path ----------

    [Fact]
    public async Task Dispatch_ReturnsHandlerResult()
    {
        using var provider = BuildProvider(s =>
            s.AddScoped<ICommandHandler<CreatePerson, int>, CreatePersonHandler>());

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var result = await dispatcher.Dispatch<CreatePerson, int>(
            new CreatePerson("Alice"), TestContext.Current.CancellationToken);

        Assert.Equal(42, result);
    }

    [Fact]
    public async Task Dispatch_PassesCommandToHandler()
    {
        using var provider = BuildProvider(s =>
        {
            _ = s.AddScoped<CapturingCreatePersonHandler>();
            _ = s.AddScoped<ICommandHandler<CreatePerson, int>>(sp =>
                sp.GetRequiredService<CapturingCreatePersonHandler>());
        });

        using var scope = provider.CreateScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        var captured = scope.ServiceProvider.GetRequiredService<CapturingCreatePersonHandler>();

        _ = await dispatcher.Dispatch<CreatePerson, int>(
            new CreatePerson("Bob"), TestContext.Current.CancellationToken);

        Assert.Equal("Bob", captured.Name);
    }

    // ---------- Argument validation ----------

    [Fact]
    public async Task Dispatch_NullCommand_Throws()
    {
        using var provider = BuildProvider(s =>
            s.AddScoped<ICommandHandler<CreatePerson, int>, CreatePersonHandler>());

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        _ = await Assert.ThrowsAsync<ArgumentNullException>(
            () => dispatcher.Dispatch<CreatePerson, int>(null!, TestContext.Current.CancellationToken));
    }

    // ---------- Registration issues ----------

    [Fact]
    public async Task Dispatch_NoHandlerRegistered_Throws()
    {
        using var provider = BuildProvider();

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => dispatcher.Dispatch<CreatePerson, int>(
                new CreatePerson("Alice"), TestContext.Current.CancellationToken));

        Assert.Contains("No handler", ex.Message, StringComparison.Ordinal);
        Assert.Contains(nameof(CreatePerson), ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Dispatch_MultipleHandlersRegistered_Throws()
    {
        using var provider = BuildProvider(s =>
        {
            _ = s.AddScoped<ICommandHandler<CreatePerson, int>, CreatePersonHandler>();
            _ = s.AddScoped<ICommandHandler<CreatePerson, int>, CreatePersonHandler>();
        });

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => dispatcher.Dispatch<CreatePerson, int>(
                new CreatePerson("Alice"), TestContext.Current.CancellationToken));

        Assert.Contains("More than one", ex.Message, StringComparison.Ordinal);
    }

    // ---------- Handler failures ----------

    [Fact]
    public async Task Dispatch_HandlerThrowsSynchronously_WrapsInInvalidOperation()
    {
        using var provider = BuildProvider(s =>
            s.AddScoped<ICommandHandler<CreatePerson, int>, SyncThrowingHandler>());

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => dispatcher.Dispatch<CreatePerson, int>(
                new CreatePerson("Alice"), TestContext.Current.CancellationToken));

        Assert.Contains(nameof(SyncThrowingHandler), ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Dispatch_HandlerThrowsAsynchronously_PropagatesOriginalException()
    {
        // Exceptions thrown from the async body (after the first await) aren't wrapped
        // in TargetInvocationException and should propagate unwrapped through the await.
        using var provider = BuildProvider(s =>
            s.AddScoped<ICommandHandler<CreatePerson, int>, AsyncThrowingHandler>());

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        _ = await Assert.ThrowsAsync<InvalidDataException>(
            () => dispatcher.Dispatch<CreatePerson, int>(
                new CreatePerson("Alice"), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Dispatch_HandlerThrowsOperationCanceledSynchronously_RethrowsUnwrapped()
    {
        // Regression coverage for the TargetInvocationException unwrapping path added
        // in the recent dispatcher bug-fix (#406).
        using var provider = BuildProvider(s =>
            s.AddScoped<ICommandHandler<CreatePerson, int>, SyncCancellingHandler>());

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        _ = await Assert.ThrowsAsync<OperationCanceledException>(
            () => dispatcher.Dispatch<CreatePerson, int>(
                new CreatePerson("Alice"), TestContext.Current.CancellationToken));
    }

    // ---------- Cancellation ----------

    [Fact]
    public async Task Dispatch_PassesCancellationTokenToHandler()
    {
        using var provider = BuildProvider(s =>
        {
            _ = s.AddScoped<CancellationObservingHandler>();
            _ = s.AddScoped<ICommandHandler<CreatePerson, int>>(sp =>
                sp.GetRequiredService<CancellationObservingHandler>());
        });

        using var scope = provider.CreateScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        var observer = scope.ServiceProvider.GetRequiredService<CancellationObservingHandler>();

        using var cts = new CancellationTokenSource();
        _ = await dispatcher.Dispatch<CreatePerson, int>(new CreatePerson("Alice"), cts.Token);

        Assert.Equal(cts.Token, observer.ReceivedToken);
    }

    // ---------- Constructor argument validation ----------

    [Fact]
    public void Ctor_NullServiceProvider_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => new CommandDispatcher(null!, Microsoft.Extensions.Logging.Abstractions.NullLogger<CommandDispatcher>.Instance));
    }

    [Fact]
    public void Ctor_NullLogger_Throws()
    {
        var provider = new ServiceCollection().BuildServiceProvider();
        _ = Assert.Throws<ArgumentNullException>(
            () => new CommandDispatcher(provider, null!));
    }

    // ---------- Test fakes ----------

    private sealed record CreatePerson(string Name) : ICommand;

    private sealed class CreatePersonHandler : ICommandHandler<CreatePerson, int>
    {
        public Task<int> HandleAsync(CreatePerson command, CancellationToken cancellation = default)
            => Task.FromResult(42);
    }

    private sealed class CapturingCreatePersonHandler : ICommandHandler<CreatePerson, int>
    {
        public string? Name { get; private set; }

        public Task<int> HandleAsync(CreatePerson command, CancellationToken cancellation = default)
        {
            Name = command.Name;
            return Task.FromResult(1);
        }
    }

    private sealed class SyncThrowingHandler : ICommandHandler<CreatePerson, int>
    {
        public Task<int> HandleAsync(CreatePerson command, CancellationToken cancellation = default)
        {
            throw new InvalidDataException("sync failure");
        }
    }

    private sealed class AsyncThrowingHandler : ICommandHandler<CreatePerson, int>
    {
        public async Task<int> HandleAsync(CreatePerson command, CancellationToken cancellation = default)
        {
            await Task.Yield();
            throw new InvalidDataException("async failure");
        }
    }

    private sealed class SyncCancellingHandler : ICommandHandler<CreatePerson, int>
    {
        public Task<int> HandleAsync(CreatePerson command, CancellationToken cancellation = default)
        {
            throw new OperationCanceledException();
        }
    }

    private sealed class CancellationObservingHandler : ICommandHandler<CreatePerson, int>
    {
        public CancellationToken ReceivedToken { get; private set; }

        public Task<int> HandleAsync(CreatePerson command, CancellationToken cancellation = default)
        {
            ReceivedToken = cancellation;
            return Task.FromResult(0);
        }
    }

}
