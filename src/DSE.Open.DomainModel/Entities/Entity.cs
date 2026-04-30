// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Base class for entities with an identifier of type <typeparamref name="TId"/>.
/// </summary>
/// <remarks>
/// See <see cref="StoredObject"/> for the constructor contract. In short: derived
/// concrete entity types must declare a materialization constructor marked with
/// <see cref="MaterializationConstructorAttribute"/> that chains to
/// <see cref="Entity{TId}(TId, StoredObjectInitialization)"/> with
/// <see cref="StoredObjectInitialization.Materialized"/>; the parameterless
/// base constructor is reserved for the domain-facing 'new entity' path and must
/// not be used by the ORM to materialize instances.
/// </remarks>
public abstract class Entity<TId> : StoredObject, IEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private readonly TId _id;

    /// <summary>
    /// Initializes a new entity with an unset <see cref="Id"/> and
    /// <see cref="StoredObject.Initialization"/> set to
    /// <see cref="StoredObjectInitialization.Created"/>.
    /// </summary>
    /// <remarks>
    /// Intended for use by derived classes' domain-facing 'new entity' constructors.
    /// This constructor must not be chained to from a
    /// <see cref="MaterializationConstructorAttribute"/>-marked constructor: it
    /// leaves <see cref="Id"/> at <c>default(TId)</c> and reports
    /// <see cref="StoredObjectInitialization.Created"/>, which would misclassify a
    /// reconstituted entity as newly created.
    /// </remarks>
    protected Entity()
        : base(StoredObjectInitialization.Created)
    {
    }

    /// <summary>
    /// Initializes an entity with a known <paramref name="id"/>. Use
    /// <see cref="StoredObjectInitialization.Created"/> for a newly created entity
    /// whose id is already known, and <see cref="StoredObjectInitialization.Materialized"/>
    /// when reconstituting an existing entity from the data store.
    /// </summary>
    /// <param name="id">The entity identifier. <c>default(TId)</c> is permitted —
    /// validation of the identifier value is the responsibility of the data layer
    /// (insert/update path) rather than the constructor, because Entity Framework
    /// Core can invoke a materialization constructor with <c>default(TId)</c> for
    /// phantom rows in left-outer-joins where the principal entity is missing or
    /// where a non-nullable foreign key is set to its default value.</param>
    /// <param name="initialization">The initialization state — see
    /// <see cref="StoredObject"/>.</param>
    /// <remarks>
    /// Derived concrete entity types should chain to this constructor from a
    /// constructor marked with <see cref="MaterializationConstructorAttribute"/>
    /// when they need to be reconstituted from storage.
    /// </remarks>
    protected Entity(TId id, StoredObjectInitialization initialization = StoredObjectInitialization.Created)
        : base(initialization)
    {
        _id = id;
    }

    /// <inheritdoc />
    public TId Id => _id;

    object IIdentified.Id => Id;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{GetType().Name} [Id: {_id}]";
    }
}
