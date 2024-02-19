// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DSE.Open.EntityFrameworkCore.Diagnostics;

public sealed partial class UpdateTimesTrackedSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<UpdateTimesTrackedSaveChangesInterceptor> _logger;

    public UpdateTimesTrackedSaveChangesInterceptor(
        TimeProvider timeProvider,
        ILogger<UpdateTimesTrackedSaveChangesInterceptor> logger)
    {
        Guard.IsNotNull(timeProvider);
        Guard.IsNotNull(logger);

        _timeProvider = timeProvider;
        _logger = logger;
    }

    public bool SetCreatedTimestamp { get; set; }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        throw new NotImplementedException("Only async database operations are supported.");
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(eventData);

        if (eventData.Context is null)
        {
            return ValueTask.FromResult(result);
        }

        Log.InterceptedSaveChanges(_logger, eventData.Context.GetType().Name);

        var persistedEntityEntries = eventData.Context.ChangeTracker.Entries<IUpdateTimesTracked>().ToArray();

        var addedEntities = persistedEntityEntries
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .ToArray();

        Log.SettingCreatedTime(_logger, addedEntities.Length);

        foreach (var added in addedEntities)
        {
            try
            {
                added.SetCreated(_timeProvider);
            }
            catch (InvalidOperationException ex)
            {
                ThrowHelper.ThrowInvalidOperationException(
                    $"Error setting created time on entity of type {added.GetType().Name} with created time '{added.Created}' " +
                    $"and id '{(added as IIdentified)?.Id}'", ex);
            }

            Log.SetCreatedTime(_logger, added.GetType().Name, (added as IIdentified)?.Id);
        }

        var updatedEntities = persistedEntityEntries
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity)
            .ToArray();

        Log.SettingUpdatedTime(_logger, updatedEntities.Length);

        foreach (var updated in updatedEntities)
        {
            try
            {
                updated.SetUpdated(_timeProvider);
            }
            catch (InvalidOperationException ex)
            {
                ThrowHelper.ThrowInvalidOperationException(
                    $"Error setting updated time on entity of type {updated.GetType().Name} with updated time '{updated.Updated}' " +
                    $"and id '{(updated as IIdentified)?.Id}'", ex);
            }

            Log.SetUpdatedTime(_logger, updated.GetType().Name, (updated as IIdentified)?.Id);
        }

        return ValueTask.FromResult(result);
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 8315001,
            Level = LogLevel.Debug,
            Message = "Intercepted SaveChangesAsync on {type}")]
        public static partial void InterceptedSaveChanges(ILogger logger, string type);

        [LoggerMessage(
            EventId = 8315002,
            Level = LogLevel.Debug,
            Message = "Setting created time for {count} added entities")]
        public static partial void SettingCreatedTime(ILogger logger, int count);

        [LoggerMessage(
            EventId = 8315003,
            Level = LogLevel.Debug,
            Message = "Setting updated time for {count} modified entities")]
        public static partial void SettingUpdatedTime(ILogger logger, int count);

        [LoggerMessage(
            EventId = 8315004,
            Level = LogLevel.Debug,
            Message = "Set created time for '{typeName}' entity '{id}'")]
        public static partial void SetCreatedTime(ILogger logger, string typeName, object? id);

        [LoggerMessage(
            EventId = 8315005,
            Level = LogLevel.Debug,
            Message = "Set updated time for '{typeName}' entity '{id}'")]
        public static partial void SetUpdatedTime(ILogger logger, string typeName, object? id);
    }
}
