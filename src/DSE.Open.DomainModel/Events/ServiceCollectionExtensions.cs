// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Mediators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DSE.Open.DomainModel.Events;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="IDomainEventDispatcher"/> descriptor to the service collection. By default,
    /// the registered lifetime is <see cref="ServiceLifetime.Scoped"/>.
    /// </summary>
    /// <param name="services">The collection of service descriptors.</param>
    /// <param name="serviceLifetime">The lifetime of the implementation. Defaults to
    /// <see cref="ServiceLifetime.Scoped"/>.</param>
    /// <returns></returns>
    public static IServiceCollection AddDomainEventDispatcher(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        Guard.IsNotNull(services);
        _ = services.AddMessageDispatcher(serviceLifetime);
        services.TryAdd(new ServiceDescriptor(typeof(IDomainEventDispatcher), typeof(DomainEventDispatcher), serviceLifetime));
        return services;
    }

    /// <summary>
    /// Adds a <see cref="IDomainEventMessageHandler{TMessage}"/> descriptor to the service collection. By default,
    /// the registered lifetime is <see cref="ServiceLifetime.Scoped"/>.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="services"></param>
    /// <param name="serviceLifetime"></param>
    /// <returns></returns>
    [RequiresUnreferencedCode("May break functionality when AOT compiling")]
    public static IServiceCollection AddDomainEventHandler<THandler, TMessage>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where THandler : class, IDomainEventMessageHandler<TMessage>
        where TMessage : IDomainEvent
    {
        Guard.IsNotNull(services);
        services.Add(new(typeof(IMessageHandler<TMessage>), typeof(THandler), serviceLifetime));
        return services;
    }
}
