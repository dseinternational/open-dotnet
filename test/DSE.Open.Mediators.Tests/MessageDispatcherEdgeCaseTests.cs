// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.Mediators.Tests;

public class MessageDispatcherEdgeCaseTests : LoggedTestsBase
{
    public MessageDispatcherEdgeCaseTests(ITestOutputHelper output) : base(output)
    {
    }

    private ServiceProvider BuildProvider(Action<IServiceCollection>? configure = null)
    {
        var services = new ServiceCollection();
        _ = services.AddLogging(ConfigureLogging);
        _ = services.AddMessageDispatcher();
        configure?.Invoke(services);
        return services.BuildServiceProvider();
    }

    // ---------- Null / missing registration ----------

    [Fact]
    public async Task PublishAsync_NullMessage_Throws()
    {
        using var provider = BuildProvider();
        var dispatcher = provider.GetRequiredService<IMessageDispatcher>();

        _ = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await dispatcher.PublishAsync(null!, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task PublishAsync_NoHandlersRegistered_CompletesSilently()
    {
        // Documented behaviour: no handlers → warning logged, no throw.
        using var provider = BuildProvider();
        var dispatcher = provider.GetRequiredService<IMessageDispatcher>();

        await dispatcher.PublishAsync(new PingMessage(), TestContext.Current.CancellationToken);
    }

    // ---------- Handler failures ----------

    [Fact]
    public async Task PublishAsync_HandlerThrowsSynchronously_WrapsInInvalidOperation()
    {
        using var provider = BuildProvider(s =>
            s.AddScoped<IMessageHandler<PingMessage>, SyncThrowingHandler>());

        var dispatcher = provider.GetRequiredService<IMessageDispatcher>();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await dispatcher.PublishAsync(new PingMessage(), TestContext.Current.CancellationToken));

        Assert.Contains(nameof(SyncThrowingHandler), ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task PublishAsync_HandlerThrowsAsynchronously_PropagatesOriginalException()
    {
        using var provider = BuildProvider(s =>
            s.AddScoped<IMessageHandler<PingMessage>, AsyncThrowingHandler>());

        var dispatcher = provider.GetRequiredService<IMessageDispatcher>();

        _ = await Assert.ThrowsAsync<InvalidDataException>(
            async () => await dispatcher.PublishAsync(new PingMessage(), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task PublishAsync_HandlerThrowsOperationCanceledSynchronously_RethrowsUnwrapped()
    {
        // Regression coverage for the TargetInvocationException unwrapping path added
        // in the recent dispatcher bug-fix (#406).
        using var provider = BuildProvider(s =>
            s.AddScoped<IMessageHandler<PingMessage>, SyncCancellingHandler>());

        var dispatcher = provider.GetRequiredService<IMessageDispatcher>();

        _ = await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await dispatcher.PublishAsync(new PingMessage(), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task PublishAsync_FirstHandlerThrows_SubsequentHandlersDoNotRun()
    {
        // Documents the current fail-fast behaviour: a throw from the first handler
        // propagates without running the rest.
        var ran = new List<string>();

        using var provider = BuildProvider(s =>
        {
            _ = s.AddSingleton<IMessageHandler<PingMessage>>(
                new ThrowingTrackingHandler(ran, "first", throwInHandler: true));
            _ = s.AddSingleton<IMessageHandler<PingMessage>>(
                new ThrowingTrackingHandler(ran, "second", throwInHandler: false));
        });

        var dispatcher = provider.GetRequiredService<IMessageDispatcher>();

        _ = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await dispatcher.PublishAsync(new PingMessage(), TestContext.Current.CancellationToken));

        Assert.Equal(["first"], ran);
    }

    // ---------- Cancellation ----------

    [Fact]
    public async Task PublishAsync_PassesCancellationTokenToHandler()
    {
        var observer = new CancellationObservingHandler();

        using var provider = BuildProvider(s =>
            s.AddSingleton<IMessageHandler<PingMessage>>(observer));

        var dispatcher = provider.GetRequiredService<IMessageDispatcher>();

        using var cts = new CancellationTokenSource();
        await dispatcher.PublishAsync(new PingMessage(), cts.Token);

        Assert.Equal(cts.Token, observer.ReceivedToken);
    }

    // ---------- Constructor argument validation ----------

    [Fact]
    public void Ctor_NullServiceProvider_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => new MessageDispatcher(null!, new NullLogger<MessageDispatcher>()));
    }

    [Fact]
    public void Ctor_NullLogger_Throws()
    {
        var provider = new ServiceCollection().BuildServiceProvider();
        _ = Assert.Throws<ArgumentNullException>(
            () => new MessageDispatcher(provider, null!));
    }

    // ---------- Test fakes ----------

    private sealed class PingMessage : IMessage;

    private sealed class SyncThrowingHandler : IMessageHandler<PingMessage>
    {
        public ValueTask HandleAsync(PingMessage message, CancellationToken cancellationToken = default)
        {
            throw new InvalidDataException("sync failure");
        }
    }

    private sealed class AsyncThrowingHandler : IMessageHandler<PingMessage>
    {
        public async ValueTask HandleAsync(PingMessage message, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            throw new InvalidDataException("async failure");
        }
    }

    private sealed class SyncCancellingHandler : IMessageHandler<PingMessage>
    {
        public ValueTask HandleAsync(PingMessage message, CancellationToken cancellationToken = default)
        {
            throw new OperationCanceledException();
        }
    }

    private sealed class CancellationObservingHandler : IMessageHandler<PingMessage>
    {
        public CancellationToken ReceivedToken { get; private set; }

        public ValueTask HandleAsync(PingMessage message, CancellationToken cancellationToken = default)
        {
            ReceivedToken = cancellationToken;
            return ValueTask.CompletedTask;
        }
    }

    private sealed class ThrowingTrackingHandler : IMessageHandler<PingMessage>
    {
        private readonly List<string> _ran;
        private readonly string _label;
        private readonly bool _throwInHandler;

        public ThrowingTrackingHandler(List<string> ran, string label, bool throwInHandler)
        {
            _ran = ran;
            _label = label;
            _throwInHandler = throwInHandler;
        }

        public ValueTask HandleAsync(PingMessage message, CancellationToken cancellationToken = default)
        {
            lock (_ran)
            {
                _ran.Add(_label);
            }

            if (_throwInHandler)
            {
                throw new InvalidDataException("fail-fast");
            }

            return ValueTask.CompletedTask;
        }
    }

    private sealed class NullLogger<T> : Microsoft.Extensions.Logging.ILogger<T>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => false;
        public void Log<TState>(
            Microsoft.Extensions.Logging.LogLevel logLevel,
            Microsoft.Extensions.Logging.EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) { }
    }
}
