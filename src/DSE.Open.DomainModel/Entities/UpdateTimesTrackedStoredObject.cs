// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Stored object tracking <see cref="Created"/> / <see cref="Updated"/> timestamps.
/// </summary>
/// <remarks>
/// See <see cref="StoredObject"/> for the constructor contract. The parameterless
/// constructor is the 'new object' path and leaves both timestamps unset — the
/// domain is expected to populate them via
/// <see cref="IUpdateTimesTracked.SetCreated(TimeProvider?)"/> before the object is
/// persisted. The <c>(DateTimeOffset?, DateTimeOffset?)</c> constructor is the
/// materialization path and derived concrete types must chain to it from a
/// constructor marked with <see cref="MaterializationConstructorAttribute"/>.
/// </remarks>
public abstract class UpdateTimesTrackedStoredObject : StoredObject, IUpdateTimesTracked
{
    private DateTimeOffset? _created;
    private DateTimeOffset? _updated;

    /// <summary>
    /// Initializes a newly created stored object — timestamps are unset and
    /// <see cref="StoredObject.Initialization"/> is
    /// <see cref="StoredObjectInitialization.Created"/>.
    /// </summary>
    protected UpdateTimesTrackedStoredObject()
        : base(StoredObjectInitialization.Created)
    {
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the object from storage. Both timestamps must be non-null and
    /// non-default.
    /// </summary>
    protected UpdateTimesTrackedStoredObject(DateTimeOffset? created, DateTimeOffset? updated)
        : base(StoredObjectInitialization.Materialized)
    {
        EntityDataInitializationException.ThrowIf(created is null || created.Value == default);
        EntityDataInitializationException.ThrowIf(updated is null || updated.Value == default);

        _created = created;
        _updated = updated;
    }

    /// <inheritdoc />
    public DateTimeOffset? Created => _created;

    /// <inheritdoc />
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

