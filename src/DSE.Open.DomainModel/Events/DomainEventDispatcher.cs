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

        var consecutiveEvents = new List<IDomainEvent>();
        var backgroundEvents = new List<IBackgroundDomainEvent>();

        foreach (var entity in entities)
        {
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

        // wait for background events to all be dispatched successfully...

        await Task.WhenAll(backgroundTasks).ConfigureAwait(false);
    }
}
