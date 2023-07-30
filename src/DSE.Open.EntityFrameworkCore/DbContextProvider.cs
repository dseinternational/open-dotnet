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
public static class DbContextExtensions
{
    public static int SaveChanges<T>(
        this T dataContext,
        Func<T, DbUpdateConcurrencyException, int> concurrencyExceptionHandler)
        where T : DbContext
    {
        Guard.IsNotNull(dataContext);
        Guard.IsNotNull(concurrencyExceptionHandler);

        int result;
        try
        {
            result = dataContext.SaveChanges();
        }
        catch (DbUpdateConcurrencyException concurrencyException)
        {
            result = concurrencyExceptionHandler(dataContext, concurrencyException);
        }
        return result;
    }

    public static async Task<int> SaveChangesAsync<T>(
        this T dataContext,
        Func<T, DbUpdateConcurrencyException, CancellationToken, Task<int>> concurrencyExceptionHandler,
        CancellationToken cancellationToken = default)
        where T : DbContext
    {
        Guard.IsNotNull(dataContext);
        Guard.IsNotNull(concurrencyExceptionHandler);

        int result;
        try
        {
            result = await dataContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException concurrencyException)
        {
            result = await concurrencyExceptionHandler(dataContext, concurrencyException, cancellationToken).ConfigureAwait(false);
        }
        return result;
    }
}
public static partial class QueryableExtensions
{
    public static bool AnyWithId<T, TId>(this IQueryable<T> query, TId id)
        where T : class, IIdentified<TId>
        where TId : struct, IEquatable<TId>
    {
        Guard.IsNotNull(query);
        return query.WhereIdEquals(id).Any();
    }

    public static IQueryable<T> WhereIdEquals<T, TId>(this IQueryable<T> query, TId id)
        where T : class, IIdentified<TId>
        where TId : struct, IEquatable<TId>
    {
        Guard.IsNotNull(query);
        return query.Where(e => e.Id.Equals(id));
    }

    public static T SingleWithId<T, TId>(this IQueryable<T> query, TId id)
        where T : class, IIdentified<TId>
        where TId : struct, IEquatable<TId>
    {
        return query.WhereIdEquals(id).Single();
    }

    public static T? SingleOrDefaultWithId<T, TId>(this IQueryable<T> query, TId id)
        where T : class, IIdentified<TId>
        where TId : struct, IEquatable<TId>
    {
        return query.WhereIdEquals(id).SingleOrDefault();
    }

    private static IQueryable<T> ValidateAndInitQuery<T>(IQueryable<T> query, bool withNoTracking)
        where T : class
    {
        Guard.IsNotNull(query);

        var result = query;

        if (withNoTracking)
        {
            result = query.AsNoTracking();
        }

        return result;
    }

    public static Task<bool> AnyWithIdAsync<T, TId>(
        this IQueryable<T> query,
        TId id,
        CancellationToken cancellationToken = default)
        where T : class, IIdentified<TId>
        where TId : struct, IEquatable<TId>
    {
        Guard.IsNotNull(query);
        return query.WhereIdEquals(id).AnyAsync(cancellationToken);
    }

    public static Task<T> SingleWithIdAsync<T, TId>(
        this IQueryable<T> query,
        TId id,
        CancellationToken cancellationToken = default)
        where T : class, IIdentified<TId>
        where TId : struct, IEquatable<TId>
    {
        return query.SingleWithIdAsync(id, false, cancellationToken);
    }

    public static Task<T> SingleWithIdAsync<T, TId>(
        this IQueryable<T> query,
        TId id,
        bool withNoTracking,
        CancellationToken cancellationToken = default)
        where T : class, IIdentified<TId>
        where TId : struct, IEquatable<TId>
    {
        query = ValidateAndInitQuery(query, withNoTracking);
        return query.WhereIdEquals(id).SingleAsync(cancellationToken);
    }

    public static Task<TEntity?> SingleOrDefaultWithIdAsync<TEntity, TId>(
        this IQueryable<TEntity> query,
        TId id,
        bool withNoTracking = false,
        CancellationToken cancellationToken = default)
        where TEntity : class, IIdentified<TId>
        where TId : struct, IEquatable<TId>
    {
        query = ValidateAndInitQuery(query, withNoTracking);
        return query.WhereIdEquals(id).SingleOrDefaultAsync(cancellationToken);
    }
}
