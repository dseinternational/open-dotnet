// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Entity that exposes a non-empty user-facing <see cref="Title"/> and tracks
/// created/updated timestamps and a concurrency <see cref="Timestamp"/>.
/// </summary>
/// <typeparam name="TId">The identifier value type.</typeparam>
public abstract class TitledEntity<TId> : UpdatesTrackedEntity<TId>, ITitled
    where TId : struct, IEquatable<TId>
{
    private string _title;

    /// <summary>
    /// Initializes a newly created entity with the supplied <paramref name="title"/>
    /// and an unset <see cref="Entity{TId}.Id"/>.
    /// </summary>
    protected TitledEntity(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        _title = title;
    }

    /// <summary>
    /// Initializes a newly created entity with the supplied <paramref name="id"/>
    /// and <paramref name="title"/>.
    /// </summary>
    protected TitledEntity(TId id, string title)
        : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        _title = title;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected TitledEntity(
        TId id,
        string title,
        DateTimeOffset? created,
        DateTimeOffset? updated,
        Timestamp? timestamp)
        : base(id, created, updated, timestamp)
    {
        EntityDataInitializationException.ThrowIf(string.IsNullOrWhiteSpace(title));
        _title = title;
    }

    /// <inheritdoc cref="ITitled.Title" />
    public virtual string Title
    {
        get => _title;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _title = value;
        }
    }
}
