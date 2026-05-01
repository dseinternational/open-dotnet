// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An <see cref="UpdateUsersTrackedTitleDescriptionEntity{TId}"/> that is also
/// externally addressable by a <see cref="Tag"/>.
/// </summary>
/// <remarks>
/// User-tracked counterpart to <see cref="TaggedTitleDescriptionEntity{TId}"/>.
/// </remarks>
public abstract class UpdateUsersTrackedTaggedTitleDescriptionEntity<TId> : UpdateUsersTrackedTitleDescriptionEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private Tag _tag;

    /// <summary>
    /// Initializes a newly created entity with the supplied tag, title and description.
    /// </summary>
    protected UpdateUsersTrackedTaggedTitleDescriptionEntity(Tag tag, string title, string description)
        : base(title, description)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Initializes a newly created entity with the supplied id, tag, title and description.
    /// </summary>
    protected UpdateUsersTrackedTaggedTitleDescriptionEntity(TId id, Tag tag, string title, string description)
        : base(id, title, description)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from a
    /// <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected UpdateUsersTrackedTaggedTitleDescriptionEntity(
        TId id,
        Tag tag,
        string title,
        string description,
        DateTimeOffset? created,
        string? createdUser,
        DateTimeOffset? updated,
        string? updatedUser,
        Timestamp? timestamp)
        : base(id, title, description, created, createdUser, updated, updatedUser, timestamp)
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
