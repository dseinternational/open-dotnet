// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An <see cref="UpdateUsersTrackedUniquelyIdentifiedEntity{TId}"/> that is also
/// externally addressable by a <see cref="Tag"/>.
/// </summary>
/// <remarks>
/// User-tracked counterpart to <see cref="TaggedUniquelyIdentifiedEntity{TId}"/>.
/// </remarks>
/// <typeparam name="TId">The identifier value type.</typeparam>
public abstract class UpdateUsersTrackedTaggedUniquelyIdentifiedEntity<TId> : UpdateUsersTrackedUniquelyIdentifiedEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private Tag _tag;

    /// <summary>
    /// Initializes a new instance with a freshly generated <see cref="UpdateUsersTrackedUniquelyIdentifiedEntity{TId}.UniqueId"/>
    /// and the supplied <paramref name="tag"/>.
    /// </summary>
    protected UpdateUsersTrackedTaggedUniquelyIdentifiedEntity(Tag tag)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Initializes a new instance with the supplied unique id and tag.
    /// </summary>
    protected UpdateUsersTrackedTaggedUniquelyIdentifiedEntity(Guid uniqueId, Tag tag)
        : base(uniqueId)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Initializes a new instance with the supplied <paramref name="id"/>, a freshly
    /// generated unique id, and the supplied <paramref name="tag"/>.
    /// </summary>
    /// <remarks>
    /// See <see cref="TaggedUniquelyIdentifiedEntity{TId}(TId, Tag)"/> for the
    /// <typeparamref name="TId"/> = <see cref="Guid"/> overload-resolution caveat.
    /// </remarks>
    protected UpdateUsersTrackedTaggedUniquelyIdentifiedEntity(TId id, Tag tag)
        : this(id, Guid.NewGuid(), tag)
    {
    }

    /// <summary>
    /// Initializes a new instance with the supplied id, unique id and tag.
    /// </summary>
    protected UpdateUsersTrackedTaggedUniquelyIdentifiedEntity(TId id, Guid uniqueId, Tag tag)
        : base(id, uniqueId)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected UpdateUsersTrackedTaggedUniquelyIdentifiedEntity(
        TId id,
        Guid uniqueId,
        Tag tag,
        DateTimeOffset? created,
        string? createdUser,
        DateTimeOffset? updated,
        string? updatedUser,
        Timestamp? timestamp)
        : base(id, uniqueId, created, createdUser, updated, updatedUser, timestamp)
    {
        EntityDataInitializationException.ThrowIf(tag == default);
        _tag = tag;
    }

    /// <summary>
    /// Gets or sets the <see cref="Tag"/> that identifies this entity in URIs and
    /// integration scenarios.
    /// </summary>
    public virtual Tag Tag
    {
        get => _tag;
        set
        {
            EnsureTagNotEmpty(value);
            _tag = value;
        }
    }

    private static void EnsureTagNotEmpty(Tag tag)
    {
        if (tag == default)
        {
            ThrowHelper.ThrowArgumentException(nameof(tag), "Tag must not be empty.");
        }
    }
}
