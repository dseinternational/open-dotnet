// Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
// Information contained herein is PROPRIETARY AND CONFIDENTIAL.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DSE.Open.Mediators;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="IMessageDispatcher"/> descriptor to the service collection. By default,
    /// the registered lifetime is <see cref="ServiceLifetime.Scoped"/>.
    /// </summary>
    /// <param name="services">The collection of service descriptors.</param>
    /// <param name="serviceLifetime">The lifetime of the implementation. Defaults to
    /// <see cref="ServiceLifetime.Scoped"/>.</param>
    /// <returns></returns>
    public static IServiceCollection AddMessageDispatcher(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        Guard.IsNotNull(services);
        services.TryAdd(new ServiceDescriptor(typeof(IMessageDispatcher), typeof(MessageDispatcher), serviceLifetime));
        return services;
    }

    /// <summary>
    /// Adds a <see cref="IMessageHandler{TMessage}"/> descriptor to the service collection. By default,
    /// the registered lifetime is <see cref="ServiceLifetime.Scoped"/>.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    /// <typeparam name="TMessage"></typeparam>
    /// <param name="services"></param>
    /// <param name="serviceLifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddMessageHandler<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler, TMessage>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where THandler : class, IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        Guard.IsNotNull(services);
        services.Add(new ServiceDescriptor(typeof(IMessageHandler<TMessage>), typeof(THandler), serviceLifetime));
        return services;
    }
}
