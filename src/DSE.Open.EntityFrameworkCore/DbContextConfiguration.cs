// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;

namespace DSE.Open.EntityFrameworkCore;

/// <inheritdoc />
public class DbContextConfiguration<TContext> : IDbContextConfiguration<TContext>
    where TContext : DbContext
{
    /// <inheritdoc />
    public string? Name { get; set; }
}
