// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;

namespace DSE.Open.EntityFrameworkCore;

/// <summary>
/// Indicates which configuration to use when configuring <typeparamref name="TContext"/>
/// instances in the current context.
/// </summary>
/// <remarks>An instance of a configuration selector should be configured in the same
/// scope as the DbContext, together with a <see cref="IDbContextProvider"/> in the
/// same scope.</remarks>
public interface IDbContextConfiguration<TContext>
    where TContext : DbContext
{
    /// <summary>
    /// Identifies the configuration to use when configuring <typeparamref name="TContext"/>
    /// instances in the current context. If <see langword="null"/>, then the default
    /// configuration is used.
    /// </summary>
    string? Name { get; set; }
}
