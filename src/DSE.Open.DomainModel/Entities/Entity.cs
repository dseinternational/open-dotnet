// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Entities;

public abstract class Entity<TId> : StoredObject, IEntity<TId>
    where TId : struct, IEquatable<TId>
{
    private readonly TId _id;

    protected Entity()
        : base(StoredObjectInitialization.Created)
    {
    }

    protected Entity(TId id, StoredObjectInitialization initialization = StoredObjectInitialization.Created)
        : base(initialization)
    {
        if (initialization == StoredObjectInitialization.Materialized)
        {
            EntityDataInitializationException.ThrowIf(id.Equals(default));
        }

        _id = id;
    }

    public TId Id => _id;

    object IIdentified.Id => Id;

    public override string ToString()
    {
        return $"{GetType().Name} [Id: {_id}]";
    }
}
