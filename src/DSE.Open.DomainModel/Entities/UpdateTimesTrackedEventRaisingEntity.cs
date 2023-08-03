// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

public abstract class UpdateTimesTrackedEventRaisingEntity<TId> : EventRaisingEntity<TId>, IUpdateTimesTracked
    where TId : struct, IEquatable<TId>
{
    private DateTimeOffset? _created;
    private DateTimeOffset? _updated;

    protected UpdateTimesTrackedEventRaisingEntity()
    {
    }

    protected UpdateTimesTrackedEventRaisingEntity(TId id)
        : base(id, StoredObjectInitialization.Created)
    {
    }

    protected UpdateTimesTrackedEventRaisingEntity(TId id, DateTimeOffset? created, DateTimeOffset? updated)
        : base(id, StoredObjectInitialization.Materialized)
    {
        EntityDataInitializationException.ThrowIf(created is null || created.Value == default);
        EntityDataInitializationException.ThrowIf(updated is null || updated.Value == default);

        _created = created;
        _updated = updated;
    }

    public DateTimeOffset? Created => _created;

    public DateTimeOffset? Updated => _updated;

#pragma warning disable CA1033 // Interface methods should be callable by child types

    void IUpdateTimesTracked.SetCreated(TimeProvider? timeProvider)
    {
        if (_created is not null)
        {
            ThrowHelper.ThrowInvalidOperationException("Cannot set created time as it has already been set.");
            return;
        }

        _created = timeProvider?.GetLocalNow() ?? DateTimeOffset.Now;
        _updated = Created;
    }

    void IUpdateTimesTracked.SetUpdated(TimeProvider? timeProvider)
    {
        if (_created is null)
        {
            ThrowHelper.ThrowInvalidOperationException("Cannot set updated time as created time has not been set.");
            return;
        }

        _updated = timeProvider?.GetLocalNow() ?? DateTimeOffset.Now;
    }
#pragma warning restore CA1033 // Interface methods should be callable by child types
}
