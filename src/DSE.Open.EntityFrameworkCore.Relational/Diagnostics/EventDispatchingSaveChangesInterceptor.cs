// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using DSE.Open.DomainModel.Events;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DSE.Open.EntityFrameworkCore.Diagnostics;

/// <summary>
/// A <see cref="SaveChangesInterceptor"/> that dispatches domain events for entities
/// implementing <see cref="IEventRaisingEntity"/>. Before-save events are published
/// prior to persistence, and post-save events are published after changes are saved.
/// </summary>
[RequiresDynamicCode("May break functionality when AOT compiling")]
public sealed partial class EventDispatchingSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventDispatchingSaveChangesInterceptor"/> class.
    /// </summary>
    /// <param name="eventDispatcher">The dispatcher used to publish domain events.</param>
    /// <param name="logger">The logger used to record interceptor activity.</param>
    public EventDispatchingSaveChangesInterceptor(
        IDomainEventDispatcher eventDispatcher,
        ILogger<EventDispatchingSaveChangesInterceptor> logger)
    {
        ArgumentNullException.ThrowIfNull(eventDispatcher);
        ArgumentNullException.ThrowIfNull(logger);

        EventDispatcher = eventDispatcher;
        _logger = logger;
    }

    /// <summary>
    /// Gets the <see cref="IDomainEventDispatcher"/> used to publish domain events.
    /// </summary>
    public IDomainEventDispatcher EventDispatcher { get; }

    /// <inheritdoc/>
    /// <remarks>Synchronous save operations are not supported; throws <see cref="NotImplementedException"/>.</remarks>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        throw new NotImplementedException("Only async database operations are supported.");
    }

    /// <inheritdoc/>
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);

        if (eventData.Context is null)
        {
            return result;
        }

        var entitiesWithBeforeSaveEvents = eventData.Context.ChangeTracker.Entries<IEventRaisingEntity>()
            .Select(e => e.Entity)
            .Where(e => e.HasBeforeSaveChangesEvents())
            .ToArray();

        if (entitiesWithBeforeSaveEvents.Length != 0)
        {
#pragma warning disable CA1873 // Avoid potentially expensive logging
            Log.SavingChanges(_logger, entitiesWithBeforeSaveEvents.Length,
                entitiesWithBeforeSaveEvents.Sum(e => e.Events.Count()));
#pragma warning restore CA1873 // Avoid potentially expensive logging

            await EventDispatcher
                .PublishBeforeSaveChangesEventsAsync(entitiesWithBeforeSaveEvents, cancellationToken)
                .ConfigureAwait(false);
        }

        return result;
    }

    /// <inheritdoc/>
    /// <remarks>Synchronous save operations are not supported; throws <see cref="NotImplementedException"/>.</remarks>
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        throw new NotImplementedException("Only async database operations are supported.");
    }

    /// <inheritdoc/>
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventData);

        if (eventData.Context is null)
        {
            return result;
        }

        var entitiesWithEvents = eventData.Context.ChangeTracker.Entries<IEventRaisingEntity>()
            .Select(e => e.Entity)
            .Where(e => e.HasEvents)
            .ToArray();

        if (entitiesWithEvents.Length == 0)
        {
            return result;
        }

#pragma warning disable CA1873 // Avoid potentially expensive logging
        Log.ChangesSaved(_logger, entitiesWithEvents.Length, entitiesWithEvents.Sum(e => e.Events.Count()));
#pragma warning restore CA1873 // Avoid potentially expensive logging

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
