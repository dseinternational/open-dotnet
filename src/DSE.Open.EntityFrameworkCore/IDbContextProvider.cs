// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;

namespace DSE.Open.EntityFrameworkCore;

/// <summary>
/// Provides a <see cref="DbContext"/> with a named configuration.
/// </summary>
public interface IDbContextProvider
{
    /// <summary>
    /// Gets an instance of <typeparamref name="TDbContext"/> configured with the options
    /// identified by <paramref name="name"/>. If <paramref name="name"/> is <see langword="null"/>,
    /// then a default configuration is applied.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the <see cref="DbContext"/> to return.</typeparam>
    /// <param name="name">THe name of the configuration to apply.</param>
    /// <returns>An instance of <typeparamref name="TDbContext"/> configured with the
    /// specified options.</returns>
    /// <exception cref="InvalidOperationException">The requested configuration name is
    /// invalid or the requested <see cref="DbContext"/> type is not registered with
    /// the <see cref="IServiceProvider"/> in the current context.</exception>
    TDbContext GetDbContext<TDbContext>(string? name = null) where TDbContext : DbContext;
}
