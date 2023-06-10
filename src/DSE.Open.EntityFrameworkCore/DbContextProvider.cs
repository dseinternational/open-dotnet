// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DSE.Open.EntityFrameworkCore;

/// <inheritdoc />
public sealed partial class DbContextProvider : IDbContextProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public DbContextProvider(IServiceProvider serviceProvider, ILogger<DbContextProvider> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    public TDbContext GetDbContext<TDbContext>(string? name = null)
        where TDbContext : DbContext
    {
        Log.DbContextRequested(_logger, typeof(TDbContext).Name, name);
        var selector = _serviceProvider.GetRequiredService<IDbContextConfiguration<TDbContext>>();
        selector.Name = name;
        return _serviceProvider.GetRequiredService<TDbContext>();
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 20232701,
            Level = LogLevel.Debug,
            Message = "DbContext of type {contextType} requested with configuration {config}")]
        public static partial void DbContextRequested(ILogger logger, string contextType, string? config);
    }
}
