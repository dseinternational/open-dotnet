// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DSE.Open.Mediators;

/// <summary>
/// Registration helpers for <see cref="IMessageDispatcher"/> and its handlers.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="MessageDispatcher"/> as <see cref="IMessageDispatcher"/>
    /// in <paramref name="services"/>. Uses <c>TryAdd</c>, so calling it repeatedly
    /// is a no-op after the first call. By default, the lifetime is
    /// <see cref="ServiceLifetime.Scoped"/>.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <param name="serviceLifetime">The lifetime of the registration.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddMessageDispatcher(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAdd(new ServiceDescriptor(typeof(IMessageDispatcher), typeof(MessageDispatcher), serviceLifetime));
        return services;
    }

    /// <summary>
    /// Registers <typeparamref name="THandler"/> as an <see cref="IMessageHandler{TMessage}"/>.
    /// Unlike <see cref="AddMessageDispatcher"/>, this uses plain <c>Add</c> — calling it twice
    /// with the same types registers the handler twice, causing the handler to run twice when
    /// a matching message is published.
    /// </summary>
    /// <typeparam name="THandler">The concrete handler implementation.</typeparam>
    /// <typeparam name="TMessage">The message type the handler handles.</typeparam>
    /// <param name="services">The service collection to modify.</param>
    /// <param name="serviceLifetime">The lifetime of the registration.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null"/>.</exception>
    public static IServiceCollection AddMessageHandler<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TMessage>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where THandler : class, IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        ArgumentNullException.ThrowIfNull(services);
        services.Add(new(typeof(IMessageHandler<TMessage>), typeof(THandler), serviceLifetime));
        return services;
    }
}
