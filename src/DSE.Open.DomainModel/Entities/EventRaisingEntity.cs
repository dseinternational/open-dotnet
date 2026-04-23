// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Base class for entities that can raise domain events.
/// </summary>
/// <remarks>
/// See <see cref="Entity{TId}"/> / <see cref="StoredObject"/> for the constructor
/// contract. Concrete derived types must declare a
/// <see cref="MaterializationConstructorAttribute"/>-marked constructor that chains
/// to <see cref="EventRaisingEntity{TId}(TId, StoredObjectInitialization)"/> with
/// <see cref="StoredObjectInitialization.Materialized"/>; the parameterless base
/// constructor is reserved for the domain-facing 'new entity' path.
/// </remarks>
public abstract class EventRaisingEntity<TId> : Entity<TId>, IEventRaisingEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private readonly Lazy<List<IDomainEvent>> _events = new(() => new());

    /// <summary>
    /// Initializes a new entity with an unset <see cref="Entity{TId}.Id"/> and
    /// <see cref="StoredObject.Initialization"/> set to
    /// <see cref="StoredObjectInitialization.Created"/>. Intended for use by
    /// derived classes' domain-facing 'new entity' constructors only.
    /// </summary>
    protected EventRaisingEntity()
    {
    }

    /// <summary>
    /// Initializes an entity with a known <paramref name="id"/> — see
    /// <see cref="Entity{TId}(TId, StoredObjectInitialization)"/>.
    /// </summary>
    /// <remarks>
    /// Derived concrete entity types should chain to this constructor from a
    /// constructor marked with <see cref="MaterializationConstructorAttribute"/>
    /// when they need to be reconstituted from storage.
    /// </remarks>
    protected EventRaisingEntity(TId id, StoredObjectInitialization initialization = StoredObjectInitialization.Created)
        : base(id, initialization)
    {
    }

    /// <inheritdoc />
    [NotMapped]
    public IEnumerable<IDomainEvent> Events => _events.Value;

    /// <inheritdoc />
    public bool HasEvents => _events.IsValueCreated && _events.Value.Count != 0;

    /// <summary>
    /// Appends <paramref name="event"/> to the pending events for this entity.
    /// </summary>
    /// <param name="event">The event to append.</param>
    /// <exception cref="ArgumentNullException"><paramref name="event"/> is <see langword="null"/>.</exception>
    protected void AddEvent(IDomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);
        _events.Value.Add(@event);
    }

    /// <summary>
    /// Removes all pending events that implement
    /// <see cref="IBeforeSaveChangesDomainEvent"/>. Derived classes may override
    /// to observe or augment this behaviour.
    /// </summary>
    protected virtual void ClearBeforeSaveChangesEventsCore()
    {
        if (HasEvents)
        {
            _ = _events.Value.RemoveAll(e => e is IBeforeSaveChangesDomainEvent);
        }
    }

    /// <summary>
    /// Removes all pending events. Derived classes may override to observe or
    /// augment this behaviour.
    /// </summary>
    protected virtual void ClearEventsCore()
    {
        if (HasEvents)
        {
            _events.Value.Clear();
        }
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types

    void IEventRaisingEntity.ClearBeforeSaveChangesEvents()
    {
        ClearBeforeSaveChangesEventsCore();
    }

    void IEventRaisingEntity.ClearEvents()
    {
        ClearEventsCore();
    }

#pragma warning restore CA1033 // Interface methods should be callable by child types
}
