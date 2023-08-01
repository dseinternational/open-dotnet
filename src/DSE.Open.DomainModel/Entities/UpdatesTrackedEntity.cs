// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Entities;

public abstract class UpdatesTrackedEntity<TId> : Entity<TId>, IUpdatesTracked
    where TId : struct, IEquatable<TId>
{
    private DateTimeOffset? _created;
    private DateTimeOffset? _updated;
    private readonly Timestamp? _timestamp;

    protected UpdatesTrackedEntity()
    {
    }

    protected UpdatesTrackedEntity(TId id)
        : base(id, StoredObjectInitialization.Created)
    {
    }

    protected UpdatesTrackedEntity(TId id, DateTimeOffset? created, DateTimeOffset? updated, Timestamp? timestamp)
        : base(id, StoredObjectInitialization.Materialized)
    {
        EntityDataInitializationException.ThrowIf(created is null || created.Value == default);
        EntityDataInitializationException.ThrowIf(updated is null || updated.Value == default);
        EntityDataInitializationException.ThrowIf(timestamp is null || timestamp.Value == default);

        _created = created;
        _updated = updated;
        _timestamp = timestamp;
    }

    public DateTimeOffset? Created => _created;

    public DateTimeOffset? Updated => _updated;

    public Timestamp? Timestamp => _timestamp;

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
