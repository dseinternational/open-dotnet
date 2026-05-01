// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An <see cref="UpdatesTrackedEntity{TId}"/> that also carries a stable, globally
/// unique <see cref="UniqueId"/> alongside its primary key.
/// </summary>
/// <typeparam name="TId">The identifier value type.</typeparam>
public abstract class UniquelyIdentifiedEntity<TId> : UpdatesTrackedEntity<TId>, IUniquelyIdentifiedEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private readonly Guid _uniqueId;

    /// <summary>
    /// Initializes a new instance with a freshly generated <see cref="UniqueId"/> and
    /// an unset <see cref="Entity{TId}.Id"/>.
    /// </summary>
    protected UniquelyIdentifiedEntity()
        : this(Guid.NewGuid())
    {
    }

    /// <summary>
    /// Initializes a new instance with the supplied <paramref name="uniqueId"/> and
    /// an unset <see cref="Entity{TId}.Id"/>.
    /// </summary>
    protected UniquelyIdentifiedEntity(Guid uniqueId)
    {
        EnsureNonEmpty(uniqueId);
        _uniqueId = uniqueId;
    }

    /// <summary>
    /// Initializes a new instance with the supplied <paramref name="id"/> and
    /// <paramref name="uniqueId"/>.
    /// </summary>
    /// <remarks>
    /// There is intentionally no single-argument <c>(TId id)</c> overload: when
    /// <typeparamref name="TId"/> is <see cref="Guid"/> it would be indistinguishable
    /// from the <see cref="UniquelyIdentifiedEntity{TId}(Guid)"/> overload that
    /// supplies a known unique id, and overload resolution would silently bind to
    /// the wrong constructor. Always supply the <see cref="UniqueId"/> explicitly
    /// alongside the <paramref name="id"/>.
    /// </remarks>
    protected UniquelyIdentifiedEntity(TId id, Guid uniqueId)
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
    protected UniquelyIdentifiedEntity(
        TId id,
        Guid uniqueId,
        DateTimeOffset? created,
        DateTimeOffset? updated,
        Timestamp? timestamp)
        : base(id, created, updated, timestamp)
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
