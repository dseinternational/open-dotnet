// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DSE.Open.EntityFrameworkCore.HealthChecks;

public abstract class DbContextHealthCheck<TContext> : LoggingHealthCheck
    where TContext : DbContext
{
    protected DbContextHealthCheck(TContext dbContext, ILogger logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        DbContext = dbContext;
    }

    protected TContext DbContext { get; }
}
