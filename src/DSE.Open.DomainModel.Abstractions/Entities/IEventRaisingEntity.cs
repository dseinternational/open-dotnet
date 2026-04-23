// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An entity that accumulates <see cref="IDomainEvent"/>s during the current
/// unit of work so they can be dispatched by an
/// <see cref="IDomainEventDispatcher"/>.
/// </summary>
public interface IEventRaisingEntity : IEntity
{
    /// <summary>
    /// A collection of domain events created by the entity within the current unit of work.
    /// </summary>
    IEnumerable<IDomainEvent> Events { get; }

    /// <summary>
    /// Indicates if there are any <see cref="Events"/>.
    /// </summary>
    bool HasEvents { get; }

    /// <summary>
    /// Clears all pending events attached to the entity.
    /// </summary>
    void ClearEvents();

    /// <summary>
    /// Clears only the pending events that implement
    /// <see cref="IBeforeSaveChangesDomainEvent"/>, leaving any other events in place.
    /// </summary>
    void ClearBeforeSaveChangesEvents();
}

/// <summary>
/// An <see cref="IEventRaisingEntity"/> with a strongly-typed identifier.
/// </summary>
/// <typeparam name="TId">The identifier value type.</typeparam>
public interface IEventRaisingEntity<TId> : IEventRaisingEntity, IEntity<TId>
    where TId : struct, IEquatable<TId>;
