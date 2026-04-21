// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.DomainModel.Entities;
using DSE.Open.Mediators;

namespace DSE.Open.DomainModel.Events;

/// <summary>
/// Dispatches domain events attached to entities.
/// </summary>
/// <remarks>Event dispatchers should be scoped to the current unit of work.</remarks>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    /// <summary>
    /// Maximum number of dispatch passes before <see cref="PublishEventsAsync"/> gives up.
    /// Each pass collects, clears, and dispatches any events currently attached to the
    /// supplied entities; handlers may in turn raise further events which are picked up
    /// in the next pass. If a handler-driven loop never settles, this cap surfaces a
    /// clear error instead of running forever.
    /// </summary>
    internal const int MaxDispatchIterations = 16;

    private readonly IMessageDispatcher _dispatcher;

    public DomainEventDispatcher(IMessageDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);

        _dispatcher = dispatcher;
    }

    /// <inheritdoc />
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    public async Task PublishBeforeSaveChangesEventsAsync(
        IEnumerable<IEventRaisingEntity> entities,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        foreach (var entity in entities)
        {
            var events = entity.GetBeforeSaveChangesEvents().ToArray();

            entity.ClearBeforeSaveChangesEvents();

            foreach (var domainEvent in events)
            {
                await _dispatcher.PublishAsync(domainEvent, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc />
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    public async Task PublishEventsAsync(
        IEnumerable<IEventRaisingEntity> entities,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        // Materialize so repeated iterations observe the same set of entities even if
        // the caller passed a deferred sequence.
        var materialized = entities as IReadOnlyCollection<IEventRaisingEntity>
            ?? entities.ToArray();

        for (var iteration = 0; iteration < MaxDispatchIterations; iteration++)
        {
            var consecutiveEvents = new List<IDomainEvent>();
            var backgroundEvents = new List<IBackgroundDomainEvent>();

            foreach (var entity in materialized)
            {
                if (!entity.HasEvents)
                {
                    continue;
                }

                var events = entity.Events.ToArray();

                entity.ClearEvents();

                foreach (var domainEvent in events)
                {
                    if (domainEvent is IBackgroundDomainEvent backgroundDomainEvent)
                    {
                        backgroundEvents.Add(backgroundDomainEvent);
                    }
                    else
                    {
                        consecutiveEvents.Add(domainEvent);
                    }
                }
            }

            if (consecutiveEvents.Count == 0 && backgroundEvents.Count == 0)
            {
                return;
            }

            // dispatch in parallel...

            var backgroundTasks = backgroundEvents.Select(e =>
                Task.Run(async () => await _dispatcher.PublishAsync(e).ConfigureAwait(false)))
                .ToArray();

            // dispatch consecutively...

            foreach (var ev in consecutiveEvents)
            {
                await _dispatcher.PublishAsync(ev, cancellationToken)
                    .ConfigureAwait(false);
            }

            // wait for background events to all be dispatched successfully before the
            // next pass so any events they raise are picked up.

            await Task.WhenAll(backgroundTasks).ConfigureAwait(false);
        }

        // If events are still pending after the iteration cap, a handler is raising
        // further events in each pass. Surface this explicitly instead of silently
        // dropping them.
        foreach (var entity in materialized)
        {
            if (entity.HasEvents)
            {
                throw new InvalidOperationException(
                    $"Domain event dispatch did not stabilise after {MaxDispatchIterations} iterations; " +
                    "a handler is likely raising further events on each pass.");
            }
        }
    }
}
