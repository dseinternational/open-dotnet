// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// A <see cref="TitledEntity{TId}"/> that is also externally addressable by a <see cref="Tag"/>.
/// </summary>
public abstract class TaggedTitledEntity<TId> : TitledEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private Tag _tag;

    /// <summary>
    /// Initializes a newly created entity with the supplied tag and title.
    /// </summary>
    protected TaggedTitledEntity(Tag tag, string title)
        : base(title)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Initializes a newly created entity with the supplied id, tag and title.
    /// </summary>
    protected TaggedTitledEntity(TId id, Tag tag, string title)
        : base(id, title)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from a
    /// <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected TaggedTitledEntity(
        TId id,
        Tag tag,
        string title,
        DateTimeOffset? created,
        DateTimeOffset? updated,
        Timestamp? timestamp)
        : base(id, title, created, updated, timestamp)
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
