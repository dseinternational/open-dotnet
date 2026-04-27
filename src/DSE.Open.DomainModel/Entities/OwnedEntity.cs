// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

/// <summary>
/// Base class for an owned entity in a one-to-one relationship that does not have
/// its own primary key — the foreign key to the owning entity is sufficient. See
/// <see href="https://learn.microsoft.com/ef/core/modeling/owned-entities#implicit-keys"/>.
/// </summary>
public abstract class OwnedEntity : StoredObject
{
    /// <summary>
    /// Initializes a newly created owned entity (<see cref="StoredObject.Initialization"/>
    /// will be <see cref="StoredObjectInitialization.Created"/>).
    /// </summary>
    protected OwnedEntity()
        : base(StoredObjectInitialization.Created)
    {
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    /// <param name="initialization">Initialization state. Pass
    /// <see cref="StoredObjectInitialization.Materialized"/> when reconstituting from
    /// storage.</param>
    protected OwnedEntity(StoredObjectInitialization initialization)
        : base(initialization)
    {
    }
}

/// <summary>
/// An owned entity whose id is the same value as its parent's id (one-to-one
/// ownership where the owned entity's primary key is the foreign key).
/// </summary>
/// <typeparam name="TParentId">The parent's identifier type — also this entity's id.</typeparam>
public abstract class OwnedEntity<TParentId> : Entity<TParentId>, IOwnedEntity<TParentId>
    where TParentId : struct, IEquatable<TParentId>
{
    /// <summary>
    /// Initializes a newly created owned entity. The id is supplied by the parent or
    /// assigned later by storage.
    /// </summary>
    protected OwnedEntity()
    {
    }

    /// <summary>
    /// Initializes a newly created owned entity with a known id.
    /// </summary>
    protected OwnedEntity(TParentId id)
        : base(id)
    {
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage.
    /// </summary>
    protected OwnedEntity(TParentId id, StoredObjectInitialization initialization)
        : base(id, initialization)
    {
    }

    /// <inheritdoc />
    public TParentId ParentId => Id;
}

/// <summary>
/// An owned entity in a one-to-many relationship — has its own primary key as well
/// as a separate <see cref="ParentId"/> identifying its owning parent. See
/// <see href="https://learn.microsoft.com/ef/core/modeling/owned-entities"/>.
/// </summary>
/// <typeparam name="TId">This entity's identifier type.</typeparam>
/// <typeparam name="TParentId">The parent's identifier type.</typeparam>
public abstract class OwnedEntity<TId, TParentId> : Entity<TId>, IOwnedEntity<TParentId>
    where TId : struct, IEquatable<TId>
    where TParentId : struct, IEquatable<TParentId>
{
    private readonly TParentId _parentId;

    /// <summary>
    /// Initializes a newly created owned entity. <see cref="ParentId"/> is left at
    /// <c>default(TParentId)</c> and is expected to be assigned by the ORM when the
    /// entity is attached to its parent.
    /// </summary>
    protected OwnedEntity()
    {
    }

    /// <summary>
    /// Initializes a newly created owned entity with a known <paramref name="id"/>
    /// and <paramref name="parentId"/>.
    /// </summary>
    protected OwnedEntity(TId id, TParentId parentId)
        : base(id)
    {
        _parentId = parentId;
    }

    /// <summary>
    /// Materialization constructor — derived concrete types should chain to this from
    /// a <see cref="MaterializationConstructorAttribute"/>-marked constructor when
    /// reconstituting the entity from storage. Throws
    /// <see cref="EntityDataInitializationException"/> if <paramref name="parentId"/>
    /// is <c>default(TParentId)</c>.
    /// </summary>
    protected OwnedEntity(TId id, TParentId parentId, StoredObjectInitialization initialization)
        : base(id, initialization)
    {
        if (initialization == StoredObjectInitialization.Materialized)
        {
            EntityDataInitializationException.ThrowIf(parentId.Equals(default));
        }

        _parentId = parentId;
    }

    /// <inheritdoc />
    public TParentId ParentId => _parentId;
}
