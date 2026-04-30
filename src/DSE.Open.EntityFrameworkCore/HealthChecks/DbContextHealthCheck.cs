// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DSE.Open.EntityFrameworkCore.HealthChecks;

/// <summary>
/// Base class for a health check that exercises a <see cref="DbContext"/> of type <typeparamref name="TContext"/>.
/// </summary>
/// <typeparam name="TContext">The <see cref="DbContext"/> type.</typeparam>
public abstract class DbContextHealthCheck<TContext> : LoggingHealthCheck
    where TContext : DbContext
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="dbContext">The <see cref="DbContext"/> to check.</param>
    /// <param name="logger">The logger.</param>
    protected DbContextHealthCheck(TContext dbContext, ILogger logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        DbContext = dbContext;
    }

    /// <summary>Gets the <see cref="DbContext"/> being checked.</summary>
    protected TContext DbContext { get; }
}
