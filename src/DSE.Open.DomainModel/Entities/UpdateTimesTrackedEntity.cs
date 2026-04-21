// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Entity that tracks <see cref="Created"/> / <see cref="Updated"/> timestamps.
/// </summary>
/// <remarks>
/// See <see cref="Entity{TId}"/> / <see cref="StoredObject"/> for the constructor
/// contract. Concrete derived types must declare a
/// <see cref="MaterializationConstructorAttribute"/>-marked constructor that chains
/// to <see cref="UpdateTimesTrackedEntity{TId}(TId, DateTimeOffset?, DateTimeOffset?)"/>;
/// the parameterless and <c>(TId)</c> constructors are the domain-facing 'new entity'
/// paths.
/// </remarks>
public abstract class UpdateTimesTrackedEntity<TId> : Entity<TId>, IUpdateTimesTracked
    where TId : struct, IEquatable<TId>
{
    private DateTimeOffset? _created;
    private DateTimeOffset? _updated;

    /// <summary>
    /// Initializes a newly created entity with an unset <see cref="Entity{TId}.Id"/>
    /// and unset timestamps. Intended for use by derived classes' domain-facing
    /// 'new entity' constructors.
    /// </summary>
    protected UpdateTimesTrackedEntity()
    {
    }

    /// <summary>
    /// Initializes a newly created entity with a known <paramref name="id"/> and
    /// unset timestamps.
    /// </summary>
    /// <remarks>
    /// Timestamps are expected to be populated via
    /// <see cref="IUpdateTimesTracked.SetCreated(TimeProvider?)"/> before the entity
    /// is persisted.
    /// </remarks>
    protected UpdateTimesTrackedEntity(TId id)
        : base(id, StoredObjectInitialization.Created)
    {
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage. All values must be present and
    /// non-default.
    /// </summary>
    protected UpdateTimesTrackedEntity(TId id, DateTimeOffset? created, DateTimeOffset? updated)
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
