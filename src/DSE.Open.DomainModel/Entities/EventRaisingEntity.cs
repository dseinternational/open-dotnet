// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;
using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Entities;

public abstract class EventRaisingEntity<TId> : Entity<TId>, IEventRaisingEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private readonly Lazy<List<IDomainEvent>> _events = new(() => new());

    protected EventRaisingEntity()
    {
    }

    protected EventRaisingEntity(TId id, StoredObjectInitialization initialization = StoredObjectInitialization.Created)
        : base(id, initialization)
    {
    }

    [NotMapped]
    public IEnumerable<IDomainEvent> Events => _events.Value;

    public bool HasEvents => _events.IsValueCreated && _events.Value.Count != 0;

    protected void AddEvent(IDomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);
        _events.Value.Add(@event);
    }

    protected virtual void ClearBeforeSaveChangesEventsCore()
    {
        if (HasEvents)
        {
            _ = _events.Value.RemoveAll(e => e is IBeforeSaveChangesDomainEvent);
        }
    }

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
