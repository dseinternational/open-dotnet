// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.Mediators.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddMessageDispatcher_RegistersDispatcher()
    {
        var services = new ServiceCollection();
        _ = services.AddLogging();

        _ = services.AddMessageDispatcher();

        using var provider = services.BuildServiceProvider();
        var dispatcher = provider.GetService<IMessageDispatcher>();

        Assert.NotNull(dispatcher);
        _ = Assert.IsType<MessageDispatcher>(dispatcher);
    }

    [Fact]
    public void AddMessageDispatcher_DefaultLifetime_IsScoped()
    {
        var services = new ServiceCollection();
        _ = services.AddMessageDispatcher();

        var descriptor = Assert.Single(services, d => d.ServiceType == typeof(IMessageDispatcher));
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
    }

    [Theory]
    [InlineData(ServiceLifetime.Transient)]
    [InlineData(ServiceLifetime.Singleton)]
    public void AddMessageDispatcher_RespectsLifetime(ServiceLifetime lifetime)
    {
        var services = new ServiceCollection();
        _ = services.AddMessageDispatcher(lifetime);

        var descriptor = Assert.Single(services, d => d.ServiceType == typeof(IMessageDispatcher));
        Assert.Equal(lifetime, descriptor.Lifetime);
    }

    [Fact]
    public void AddMessageDispatcher_CalledTwice_RegistersOnce()
    {
        // TryAdd semantics: the second call is a no-op.
        var services = new ServiceCollection();
        _ = services.AddMessageDispatcher();
        _ = services.AddMessageDispatcher();

        _ = Assert.Single(services, d => d.ServiceType == typeof(IMessageDispatcher));
    }

    [Fact]
    public void AddMessageDispatcher_ReturnsSameCollectionForChaining()
    {
        var services = new ServiceCollection();
        var returned = services.AddMessageDispatcher();
        Assert.Same(services, returned);
    }

    [Fact]
    public void AddMessageDispatcher_NullServices_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => ServiceCollectionExtensions.AddMessageDispatcher(null!));
    }

    [Fact]
    public void AddMessageHandler_RegistersHandler()
    {
        var services = new ServiceCollection();
        _ = services.AddLogging();
        _ = services.AddMessageDispatcher();

        _ = services.AddMessageHandler<DummyHandler, DummyMessage>();

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IMessageHandler<DummyMessage>>().ToArray();

        _ = Assert.Single(handlers);
        _ = Assert.IsType<DummyHandler>(handlers[0]);
    }

    [Fact]
    public void AddMessageHandler_CalledTwice_RegistersTwice()
    {
        // Plain Add semantics (not TryAdd): calling twice registers the handler twice.
        var services = new ServiceCollection();
        _ = services.AddLogging();
        _ = services.AddMessageDispatcher();
        _ = services.AddMessageHandler<DummyHandler, DummyMessage>();
        _ = services.AddMessageHandler<DummyHandler, DummyMessage>();

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IMessageHandler<DummyMessage>>().ToArray();

        Assert.Equal(2, handlers.Length);
    }

    [Fact]
    public void AddMessageHandler_NullServices_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(
            () => ServiceCollectionExtensions.AddMessageHandler<DummyHandler, DummyMessage>(null!));
    }

    private sealed class DummyMessage : IMessage;

    private sealed class DummyHandler : IMessageHandler<DummyMessage>
    {
        public ValueTask HandleAsync(DummyMessage message, CancellationToken cancellationToken = default)
            => ValueTask.CompletedTask;
    }
}
