// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;
using DSE.Open.Mediators;
using Microsoft.Extensions.DependencyInjection;

namespace DSE.Open.DomainModel.Tests.Events;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDomainEventDispatcher_RegistersDispatcherAsScoped()
    {
        var services = new ServiceCollection();

        _ = services.AddDomainEventDispatcher();

        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IDomainEventDispatcher));
        Assert.NotNull(descriptor);
        Assert.Equal(typeof(DomainEventDispatcher), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
    }

    [Fact]
    public void AddDomainEventDispatcher_UsesSuppliedLifetime()
    {
        var services = new ServiceCollection();

        _ = services.AddDomainEventDispatcher(ServiceLifetime.Singleton);

        var descriptor = services.First(d => d.ServiceType == typeof(IDomainEventDispatcher));
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }

    [Fact]
    public void AddDomainEventDispatcher_IsIdempotent()
    {
        var services = new ServiceCollection();

        _ = services.AddDomainEventDispatcher();
        _ = services.AddDomainEventDispatcher();

        var count = services.Count(d => d.ServiceType == typeof(IDomainEventDispatcher));
        Assert.Equal(1, count);
    }

    [Fact]
    public void AddDomainEventDispatcher_NullServices_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            DSE.Open.DomainModel.Events.ServiceCollectionExtensions.AddDomainEventDispatcher(null!));
    }

    [Fact]
    public void AddDomainEventHandler_RegistersHandlerForMessageHandlerInterface()
    {
        var services = new ServiceCollection();

        _ = services.AddDomainEventHandler<DomainEventHandlerFake, DomainEventFake>();

        var descriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IMessageHandler<DomainEventFake>));

        Assert.NotNull(descriptor);
        Assert.Equal(typeof(DomainEventHandlerFake), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
    }

    [Fact]
    public void AddDomainEventHandler_UsesSuppliedLifetime()
    {
        var services = new ServiceCollection();

        _ = services.AddDomainEventHandler<DomainEventHandlerFake, DomainEventFake>(ServiceLifetime.Transient);

        var descriptor = services.First(d =>
            d.ServiceType == typeof(IMessageHandler<DomainEventFake>));

        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }

    [Fact]
    public void AddDomainEventHandler_NullServices_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            DSE.Open.DomainModel.Events.ServiceCollectionExtensions.AddDomainEventHandler<DomainEventHandlerFake, DomainEventFake>(null!));
    }
}
