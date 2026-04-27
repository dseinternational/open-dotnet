// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Entity that exposes a non-empty <see cref="Name"/> and tracks
/// created/updated timestamps and a concurrency <see cref="Timestamp"/>.
/// </summary>
/// <remarks>
/// See <see cref="UpdatesTrackedEntity{TId}"/> for the constructor contract.
/// Concrete derived types must declare a
/// <see cref="MaterializationConstructorAttribute"/>-marked constructor that chains
/// to <see cref="NamedEntity{TId}(TId, string, DateTimeOffset?, DateTimeOffset?, Timestamp?)"/>;
/// the parameterless and <c>(string)</c>/<c>(TId, string)</c> constructors are the
/// domain-facing 'new entity' paths.
/// </remarks>
/// <typeparam name="TId">The identifier value type.</typeparam>
public abstract class NamedEntity<TId> : UpdatesTrackedEntity<TId>, INamed
    where TId : struct, IEquatable<TId>
{
    private string _name;

    /// <summary>
    /// Initializes a newly created entity with the supplied <paramref name="name"/>
    /// and an unset <see cref="Entity{TId}.Id"/>.
    /// </summary>
    protected NamedEntity(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _name = name;
    }

    /// <summary>
    /// Initializes a newly created entity with the supplied <paramref name="id"/>
    /// and <paramref name="name"/>.
    /// </summary>
    protected NamedEntity(TId id, string name)
        : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _name = name;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected NamedEntity(
        TId id,
        string name,
        DateTimeOffset? created,
        DateTimeOffset? updated,
        Timestamp? timestamp)
        : base(id, created, updated, timestamp)
    {
        EntityDataInitializationException.ThrowIf(string.IsNullOrWhiteSpace(name));
        _name = name;
    }

    /// <inheritdoc cref="INamed.Name" />
    public virtual string Name
    {
        get => _name;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _name = value;
        }
    }
}
