// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using DSE.Open.DomainModel.Events;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DSE.Open.EntityFrameworkCore.Diagnostics;

[RequiresDynamicCode("May break functionality when AOT compiling")]
public sealed partial class EventDispatchingSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ILogger _logger;

    public EventDispatchingSaveChangesInterceptor(
        IDomainEventDispatcher eventDispatcher,
        ILogger<EventDispatchingSaveChangesInterceptor> logger)
    {
        Guard.IsNotNull(eventDispatcher);
        Guard.IsNotNull(logger);

        EventDispatcher = eventDispatcher;
        _logger = logger;
    }

    public IDomainEventDispatcher EventDispatcher { get; }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
        => throw new NotImplementedException("Only async database operations are supported.");

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(eventData);

        if (eventData.Context is null)
        {
            return result;
        }

        var entitiesWithBeforeSaveEvents = eventData.Context.ChangeTracker.Entries<IEventRaisingEntity>()
            .Select(e => e.Entity)
            .Where(e => e.HasBeforeSaveChangesEvents())
            .ToArray();

        if (entitiesWithBeforeSaveEvents.Any())
        {
            Log.SavingChanges(_logger, entitiesWithBeforeSaveEvents.Length,
                entitiesWithBeforeSaveEvents.Sum(e => e.Events.Count()));

            await EventDispatcher
                .PublishBeforeSaveChangesEventsAsync(entitiesWithBeforeSaveEvents, cancellationToken)
                .ConfigureAwait(false);
        }

        return result;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        => throw new NotImplementedException("Only async database operations are supported.");

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(eventData);

        if (eventData.Context is null)
        {
            return result;
        }

        var entitiesWithEvents = eventData.Context.ChangeTracker.Entries<IEventRaisingEntity>()
            .Select(e => e.Entity)
            .Where(e => e.HasEvents)
            .ToArray();

        if (!entitiesWithEvents.Any())
        {
            return result;
        }

        Log.ChangesSaved(_logger, entitiesWithEvents.Length, entitiesWithEvents.Sum(e => e.Events.Count()));

        await EventDispatcher.PublishEventsAsync(entitiesWithEvents, cancellationToken).ConfigureAwait(false);

        return result;
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 8314001,
            Level = LogLevel.Debug,
            Message = "Saving changes: found {count} tracked entities with {eventCount} events")]
        public static partial void SavingChanges(ILogger logger, int count, int eventCount);

        [LoggerMessage(
            EventId = 8314002,
            Level = LogLevel.Debug,
            Message = "Changes saved: found {count} tracked entities with {eventCount} events")]
        public static partial void ChangesSaved(ILogger logger, int count, int eventCount);
    }
}
