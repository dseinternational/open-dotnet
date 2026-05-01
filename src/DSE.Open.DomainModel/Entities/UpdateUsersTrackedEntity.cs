// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Entity that tracks <see cref="Created"/> / <see cref="Updated"/> timestamps and the
/// <see cref="CreatedUser"/> / <see cref="UpdatedUser"/> identifiers responsible, plus a
/// concurrency <see cref="Timestamp"/>.
/// </summary>
/// <remarks>
/// See <see cref="UpdatesTrackedEntity{TId}"/> for the parallel non-user-tracked base.
/// Concrete derived types must declare a
/// <see cref="MaterializationConstructorAttribute"/>-marked constructor that chains to
/// <see cref="UpdateUsersTrackedEntity{TId}(TId, DateTimeOffset?, string?, DateTimeOffset?, string?, Timestamp?)"/>.
/// The parameterless and <c>(TId)</c> constructors are the domain-facing 'new entity'
/// paths.
/// </remarks>
public abstract class UpdateUsersTrackedEntity<TId> : Entity<TId>, IUpdateUsersTracked
    where TId : struct, IEquatable<TId>
{
    private DateTimeOffset? _created;
    private DateTimeOffset? _updated;
    private string? _createdUser;
    private string? _updatedUser;
    private Timestamp? _timestamp;

    /// <summary>
    /// Initializes a newly created entity with an unset <see cref="Entity{TId}.Id"/>,
    /// unset timestamps and users, and no concurrency <see cref="Timestamp"/>. Intended
    /// for use by derived classes' domain-facing 'new entity' constructors.
    /// </summary>
    protected UpdateUsersTrackedEntity()
    {
    }

    /// <summary>
    /// Initializes a newly created entity with a known <paramref name="id"/>, unset
    /// timestamps and users, and no concurrency <see cref="Timestamp"/>.
    /// </summary>
    protected UpdateUsersTrackedEntity(TId id)
        : base(id, StoredObjectInitialization.Created)
    {
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage. All values must be present and
    /// non-default.
    /// </summary>
    protected UpdateUsersTrackedEntity(
        TId id,
        DateTimeOffset? created,
        string? createdUser,
        DateTimeOffset? updated,
        string? updatedUser,
        Timestamp? timestamp)
        : base(id, StoredObjectInitialization.Materialized)
    {
        EntityDataInitializationException.ThrowIf(created is null || created.Value == default);
        EntityDataInitializationException.ThrowIf(string.IsNullOrWhiteSpace(createdUser));
        EntityDataInitializationException.ThrowIf(updated is null || updated.Value == default);
        EntityDataInitializationException.ThrowIf(string.IsNullOrWhiteSpace(updatedUser));
        EntityDataInitializationException.ThrowIf(timestamp is null || timestamp.Value == default);

        _created = created;
        _createdUser = createdUser;
        _updated = updated;
        _updatedUser = updatedUser;
        _timestamp = timestamp;
    }

    /// <inheritdoc />
    public DateTimeOffset? Created => _created;

    /// <inheritdoc />
    public DateTimeOffset? Updated => _updated;

    /// <inheritdoc />
    public Timestamp? Timestamp => _timestamp;

    /// <inheritdoc />
    public string? CreatedUser => _createdUser;

    /// <inheritdoc />
    public string? UpdatedUser => _updatedUser;

#pragma warning disable CA1033 // Interface methods should be callable by child types

    void IUpdateTimesTracked.SetCreated(TimeProvider? timeProvider)
    {
        if (_created is not null)
        {
            ThrowHelper.ThrowInvalidOperationException("Cannot set created time as it has already been set.");
            return;
        }

        _created = timeProvider?.GetLocalNow() ?? DateTimeOffset.Now;
        _updated = _created;
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

    void IUpdateUsersTracked.SetCreatedUser(string user) => SetCreatedUser(user);

    void IUpdateUsersTracked.SetUpdatedUser(string user) => SetUpdatedUser(user);

#pragma warning restore CA1033 // Interface methods should be callable by child types

    /// <summary>
    /// Sets the <see cref="CreatedUser"/> and (initially) <see cref="UpdatedUser"/>
    /// identifiers when the entity is being created. Throws
    /// <see cref="InvalidOperationException"/> if <see cref="CreatedUser"/> has
    /// already been set.
    /// </summary>
    protected void SetCreatedUser(string user)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(user);

        if (_createdUser is not null)
        {
            ThrowHelper.ThrowInvalidOperationException("Cannot set created user as it has already been set.");
            return;
        }

        _createdUser = user;
        _updatedUser = user;
    }

    /// <summary>
    /// Sets the <see cref="UpdatedUser"/> identifier when the entity is being updated.
    /// Throws <see cref="InvalidOperationException"/> if <see cref="CreatedUser"/> has
    /// not yet been set.
    /// </summary>
    protected void SetUpdatedUser(string user)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(user);

        if (_createdUser is null)
        {
            ThrowHelper.ThrowInvalidOperationException("Cannot set updated user as created user has not been set.");
            return;
        }

        _updatedUser = user;
    }

    /// <summary>
    /// Sets the concurrency <see cref="Timestamp"/>. Intended for the persistence layer
    /// to call after a successful insert or update on data stores that do not auto-populate
    /// a row-version column (for example SQLite without rowversion support).
    /// </summary>
    /// <param name="value">The timestamp to assign. Must not be the default
    /// <see cref="Timestamp"/>.</param>
    protected void SetTimestamp(Timestamp value)
    {
        EntityDataInitializationException.ThrowIf(value == default);
        _timestamp = value;
    }
}
