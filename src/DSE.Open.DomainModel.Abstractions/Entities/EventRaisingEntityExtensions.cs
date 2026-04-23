// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Extension methods on <see cref="IEventRaisingEntity"/> for filtering
/// its pending events by kind.
/// </summary>
public static class EventRaisingEntityExtensions
{
    /// <summary>
    /// Returns the pending events on <paramref name="entity"/> that implement
    /// <see cref="IBeforeSaveChangesDomainEvent"/>. An empty sequence is
    /// returned if the entity has no pending events.
    /// </summary>
    /// <param name="entity">The entity to inspect.</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null"/>.</exception>
    public static IEnumerable<IBeforeSaveChangesDomainEvent> GetBeforeSaveChangesEvents(this IEventRaisingEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!entity.HasEvents)
        {
            return [];
        }

        return entity.Events.OfType<IBeforeSaveChangesDomainEvent>();
    }

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="entity"/> has at
    /// least one pending event that implements
    /// <see cref="IBeforeSaveChangesDomainEvent"/>.
    /// </summary>
    /// <param name="entity">The entity to inspect.</param>
    /// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null"/>.</exception>
    public static bool HasBeforeSaveChangesEvents(this IEventRaisingEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.HasEvents && entity.Events.OfType<IBeforeSaveChangesDomainEvent>().Any();
    }
}
