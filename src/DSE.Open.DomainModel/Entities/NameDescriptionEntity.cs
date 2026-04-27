// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// A named entity that also exposes a non-empty <see cref="Description"/>.
/// </summary>
/// <typeparam name="TId">The identifier value type.</typeparam>
public abstract class NameDescriptionEntity<TId> : NamedEntity<TId>, IDescribed
    where TId : struct, IEquatable<TId>
{
    private string _description;

    /// <summary>
    /// Initializes a newly created entity with the supplied <paramref name="name"/>
    /// and <paramref name="description"/> and an unset <see cref="Entity{TId}.Id"/>.
    /// </summary>
    protected NameDescriptionEntity(string name, string description)
        : base(name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        _description = description;
    }

    /// <summary>
    /// Initializes a newly created entity with the supplied identifier, name and
    /// description.
    /// </summary>
    protected NameDescriptionEntity(TId id, string name, string description)
        : base(id, name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        _description = description;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected NameDescriptionEntity(
        TId id,
        string name,
        string description,
        DateTimeOffset? created,
        DateTimeOffset? updated,
        Timestamp? timestamp)
        : base(id, name, created, updated, timestamp)
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
