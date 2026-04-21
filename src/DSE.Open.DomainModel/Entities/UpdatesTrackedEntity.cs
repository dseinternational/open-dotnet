// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Entity that tracks <see cref="Created"/> / <see cref="Updated"/> timestamps and
/// carries a concurrency <see cref="Timestamp"/>.
/// </summary>
/// <remarks>
/// See <see cref="Entity{TId}"/> / <see cref="StoredObject"/> for the constructor
/// contract. Concrete derived types must declare a
/// <see cref="MaterializationConstructorAttribute"/>-marked constructor that chains
/// to <see cref="UpdatesTrackedEntity{TId}(TId, DateTimeOffset?, DateTimeOffset?, Timestamp?)"/>;
/// the parameterless and <c>(TId)</c> constructors are the domain-facing 'new entity'
/// paths.
/// </remarks>
public abstract class UpdatesTrackedEntity<TId> : Entity<TId>, IUpdatesTracked
    where TId : struct, IEquatable<TId>
{
    private DateTimeOffset? _created;
    private DateTimeOffset? _updated;
    private readonly Timestamp? _timestamp;

    /// <summary>
    /// Initializes a newly created entity with an unset <see cref="Entity{TId}.Id"/>,
    /// unset timestamps and no concurrency <see cref="Timestamp"/>. Intended for use
    /// by derived classes' domain-facing 'new entity' constructors.
    /// </summary>
    protected UpdatesTrackedEntity()
    {
    }

    /// <summary>
    /// Initializes a newly created entity with a known <paramref name="id"/>, unset
    /// timestamps and no concurrency <see cref="Timestamp"/> — timestamps are
    /// expected to be populated via
    /// <see cref="IUpdateTimesTracked.SetCreated(TimeProvider?)"/> before the entity
    /// is persisted.
    /// </summary>
    protected UpdatesTrackedEntity(TId id)
        : base(id, StoredObjectInitialization.Created)
    {
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage. All values must be present and
    /// non-default.
    /// </summary>
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
