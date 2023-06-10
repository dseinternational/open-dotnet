// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DSE.Open.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContextProvider<TContext>(
        this IServiceCollection services,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
        where TContext : DbContext
    {
        Guard.IsNotNull(services);

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
