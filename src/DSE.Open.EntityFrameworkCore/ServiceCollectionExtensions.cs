// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DSE.Open.EntityFrameworkCore;

/// <summary>
/// Extension methods for registering Entity Framework Core services on an
/// <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IDbContextProvider"/> and <see cref="IDbContextConfiguration{TContext}"/>
    /// implementations for the specified <typeparamref name="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext">The <see cref="DbContext"/> type to provide.</typeparam>
    /// <param name="services">The service collection to add the registrations to.</param>
    /// <param name="contextLifetime">
    /// The <see cref="ServiceLifetime"/> for the registered services. Defaults to
    /// <see cref="ServiceLifetime.Scoped"/>.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddDbContextProvider<TContext>(
        this IServiceCollection services,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAdd(new ServiceDescriptor(
            typeof(IDbContextProvider),
            typeof(DbContextProvider),
            contextLifetime));

        services.TryAdd(new ServiceDescriptor(
            typeof(IDbContextConfiguration<TContext>),
            typeof(DbContextConfiguration<TContext>),
            contextLifetime));

        return services;
    }
}
