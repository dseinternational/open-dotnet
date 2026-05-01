// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// A user-tracked titled entity that also exposes a non-empty <see cref="Description"/>.
/// </summary>
/// <remarks>
/// User-tracked counterpart to <see cref="TitleDescriptionEntity{TId}"/>.
/// </remarks>
/// <typeparam name="TId">The identifier value type.</typeparam>
public abstract class UpdateUsersTrackedTitleDescriptionEntity<TId> : UpdateUsersTrackedTitledEntity<TId>, IDescribed
    where TId : struct, IEquatable<TId>
{
    private string _description;

    /// <summary>
    /// Initializes a newly created entity with the supplied <paramref name="title"/>
    /// and <paramref name="description"/> and an unset <see cref="Entity{TId}.Id"/>.
    /// </summary>
    protected UpdateUsersTrackedTitleDescriptionEntity(string title, string description)
        : base(title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        _description = description;
    }

    /// <summary>
    /// Initializes a newly created entity with the supplied identifier, title and
    /// description.
    /// </summary>
    protected UpdateUsersTrackedTitleDescriptionEntity(TId id, string title, string description)
        : base(id, title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        _description = description;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected UpdateUsersTrackedTitleDescriptionEntity(
        TId id,
        string title,
        string description,
        DateTimeOffset? created,
        string? createdUser,
        DateTimeOffset? updated,
        string? updatedUser,
        Timestamp? timestamp)
        : base(id, title, created, createdUser, updated, updatedUser, timestamp)
    {
        EntityDataInitializationException.ThrowIf(string.IsNullOrWhiteSpace(description));
        _description = description;
    }

    /// <inheritdoc cref="IDescribed.Description" />
    public virtual string Description
    {
        get => _description;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _description = value;
        }
    }
}
