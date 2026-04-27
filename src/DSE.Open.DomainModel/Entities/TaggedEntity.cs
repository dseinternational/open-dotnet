// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// An <see cref="UpdatesTrackedEntity{TId}"/> that is also externally addressable by a
/// <see cref="Tag"/> in addition to its primary key.
/// </summary>
public abstract class TaggedEntity<TId> : UpdatesTrackedEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private Tag _tag;

    /// <summary>
    /// Initializes a newly created entity with the supplied <paramref name="tag"/> and
    /// an unset <see cref="Entity{TId}.Id"/>.
    /// </summary>
    protected TaggedEntity(Tag tag)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Initializes a newly created entity with the supplied <paramref name="id"/> and
    /// <paramref name="tag"/>.
    /// </summary>
    protected TaggedEntity(TId id, Tag tag)
        : base(id)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from a
    /// <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected TaggedEntity(
        TId id,
        Tag tag,
        DateTimeOffset? created,
        DateTimeOffset? updated,
        Timestamp? timestamp)
        : base(id, created, updated, timestamp)
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
