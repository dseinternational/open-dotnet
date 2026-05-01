// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// A <see cref="UniquelyIdentifiedEntity{TId}"/> that is also externally addressable
/// by a <see cref="Tag"/>.
/// </summary>
/// <typeparam name="TId">The identifier value type.</typeparam>
public abstract class TaggedUniquelyIdentifiedEntity<TId> : UniquelyIdentifiedEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private Tag _tag;

    /// <summary>
    /// Initializes a new instance with a freshly generated <see cref="UniquelyIdentifiedEntity{TId}.UniqueId"/>
    /// and the supplied <paramref name="tag"/>.
    /// </summary>
    protected TaggedUniquelyIdentifiedEntity(Tag tag)
    {
        EnsureTagNotEmpty(tag);
        _tag = tag;
    }

    /// <summary>
    /// Initializes a new instance with the supplied unique id and tag.
    /// </summary>
    protected TaggedUniquelyIdentifiedEntity(Guid uniqueId, Tag tag)
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
    /// When <typeparamref name="TId"/> is <see cref="Guid"/>, callers writing
    /// <c>new T(g, tag)</c> bind to <see cref="TaggedUniquelyIdentifiedEntity{TId}(Guid, Tag)"/>
    /// (the unique-id overload) rather than this overload. Use the explicit
    /// <see cref="TaggedUniquelyIdentifiedEntity{TId}(TId, Guid, Tag)"/> overload to
    /// disambiguate when <typeparamref name="TId"/> is <see cref="Guid"/>.
    /// </remarks>
    protected TaggedUniquelyIdentifiedEntity(TId id, Tag tag)
        : this(id, Guid.NewGuid(), tag)
    {
    }

    /// <summary>
    /// Initializes a new instance with the supplied id, unique id and tag.
    /// </summary>
    protected TaggedUniquelyIdentifiedEntity(TId id, Guid uniqueId, Tag tag)
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
    protected TaggedUniquelyIdentifiedEntity(
        TId id,
        Guid uniqueId,
        Tag tag,
        DateTimeOffset? created,
        DateTimeOffset? updated,
        Timestamp? timestamp)
        : base(id, uniqueId, created, updated, timestamp)
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
