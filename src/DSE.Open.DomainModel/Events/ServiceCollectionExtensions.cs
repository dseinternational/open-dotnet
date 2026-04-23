// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Mediators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DSE.Open.DomainModel.Events;

/// <summary>
/// <see cref="IServiceCollection"/> extension methods that register domain
/// event infrastructure (an <see cref="IDomainEventDispatcher"/> and handlers
/// for specific <see cref="IDomainEvent"/> types).
/// </summary>
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
        ArgumentNullException.ThrowIfNull(services);
        _ = services.AddMessageDispatcher(serviceLifetime);
        services.TryAdd(new ServiceDescriptor(typeof(IDomainEventDispatcher), typeof(DomainEventDispatcher), serviceLifetime));
        return services;
    }

    /// <summary>
    /// Adds a <see cref="IDomainEventMessageHandler{TMessage}"/> descriptor to the service collection. By default,
    /// the registered lifetime is <see cref="ServiceLifetime.Scoped"/>.
    /// </summary>
    /// <typeparam name="THandler">The concrete handler type.</typeparam>
    /// <typeparam name="TMessage">The domain event type handled.</typeparam>
    /// <param name="services">The collection of service descriptors.</param>
    /// <param name="serviceLifetime">The lifetime of the handler. Defaults to
    /// <see cref="ServiceLifetime.Scoped"/>.</param>
    /// <returns>The <paramref name="services"/> instance, to support chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/>
    /// is <see langword="null"/>.</exception>
    [RequiresUnreferencedCode("May break functionality when AOT compiling")]
    public static IServiceCollection AddDomainEventHandler<THandler, TMessage>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where THandler : class, IDomainEventMessageHandler<TMessage>
        where TMessage : IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(services);
        services.Add(new(typeof(IMessageHandler<TMessage>), typeof(THandler), serviceLifetime));
        return services;
    }
}
