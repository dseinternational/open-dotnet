// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An <see cref="UpdateUsersTrackedEventRaisingEntity{TId}"/> that also carries a
/// stable, globally unique <see cref="UniqueId"/> alongside its primary key.
/// </summary>
/// <remarks>
/// Event-raising counterpart to
/// <see cref="UpdateUsersTrackedUniquelyIdentifiedEntity{TId}"/>.
/// </remarks>
/// <typeparam name="TId">The identifier value type.</typeparam>
public abstract class UpdateUsersTrackedUniquelyIdentifiedEventRaisingEntity<TId> : UpdateUsersTrackedEventRaisingEntity<TId>, IUniquelyIdentifiedEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private readonly Guid _uniqueId;

    /// <summary>
    /// Initializes a new instance with a freshly generated <see cref="UniqueId"/> and
    /// an unset <see cref="Entity{TId}.Id"/>.
    /// </summary>
    protected UpdateUsersTrackedUniquelyIdentifiedEventRaisingEntity()
        : this(Guid.NewGuid())
    {
    }

    /// <summary>
    /// Initializes a new instance with the supplied <paramref name="uniqueId"/> and
    /// an unset <see cref="Entity{TId}.Id"/>.
    /// </summary>
    protected UpdateUsersTrackedUniquelyIdentifiedEventRaisingEntity(Guid uniqueId)
    {
        EnsureNonEmpty(uniqueId);
        _uniqueId = uniqueId;
    }

    /// <summary>
    /// Initializes a new instance with the supplied <paramref name="id"/> and
    /// <paramref name="uniqueId"/>.
    /// </summary>
    /// <remarks>
    /// There is intentionally no single-argument <c>(TId id)</c> overload — see
    /// <see cref="UniquelyIdentifiedEntity{TId}"/> for the rationale.
    /// </remarks>
    protected UpdateUsersTrackedUniquelyIdentifiedEventRaisingEntity(TId id, Guid uniqueId)
        : base(id)
    {
        EnsureNonEmpty(uniqueId);
        _uniqueId = uniqueId;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected UpdateUsersTrackedUniquelyIdentifiedEventRaisingEntity(
        TId id,
        Guid uniqueId,
        DateTimeOffset? created,
        string? createdUser,
        DateTimeOffset? updated,
        string? updatedUser,
        Timestamp? timestamp)
        : base(id, created, createdUser, updated, updatedUser, timestamp)
    {
        EntityDataInitializationException.ThrowIf(uniqueId == Guid.Empty);
        _uniqueId = uniqueId;
    }

    /// <inheritdoc cref="IUniquelyIdentified.UniqueId" />
    public Guid UniqueId => _uniqueId;

    private static void EnsureNonEmpty(Guid uniqueId)
    {
        if (uniqueId == Guid.Empty)
        {
            ThrowHelper.ThrowArgumentException(nameof(uniqueId), "A non-empty Guid value must be supplied.");
        }
    }
}
