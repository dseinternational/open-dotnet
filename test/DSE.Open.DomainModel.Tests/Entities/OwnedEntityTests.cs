// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class OwnedEntityTests
{
    [Fact]
    public void NoKey_Ctor_DefaultIsCreated()
    {
        var entity = new NoKeyFake();
        Assert.Equal(StoredObjectInitialization.Created, entity.Initialization);
    }

    [Fact]
    public void NoKey_Materialization_Ctor_SetsMaterialized()
    {
        var entity = new NoKeyFake(StoredObjectInitialization.Materialized);
        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
    }

    [Fact]
    public void OneToOne_ParentId_EqualsId()
    {
        var id = Guid.NewGuid();
        var entity = new OneToOneFake(id);

        Assert.Equal(id, entity.Id);
        Assert.Equal(id, entity.ParentId);
    }

    [Fact]
    public void OneToOne_Materialization_PopulatesId()
    {
        var id = Guid.NewGuid();
        var entity = new OneToOneFake(id, StoredObjectInitialization.Materialized);

        Assert.Equal(id, entity.ParentId);
        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
    }

    [Fact]
    public void OneToMany_Ctor_PopulatesIdAndParentId()
    {
        var id = Guid.NewGuid();
        var parentId = Guid.NewGuid();
        var entity = new OneToManyFake(id, parentId);

        Assert.Equal(id, entity.Id);
        Assert.Equal(parentId, entity.ParentId);
    }

    [Fact]
    public void OneToMany_Materialization_DefaultParentId_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new OneToManyFake(Guid.NewGuid(), default, StoredObjectInitialization.Materialized));
    }

    [Fact]
    public void OneToMany_Materialization_Populates()
    {
        var id = Guid.NewGuid();
        var parentId = Guid.NewGuid();
        var entity = new OneToManyFake(id, parentId, StoredObjectInitialization.Materialized);

        Assert.Equal(id, entity.Id);
        Assert.Equal(parentId, entity.ParentId);
        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
    }

    [Fact]
    public void OneToMany_Implements_IOwnedEntity()
    {
        Assert.IsAssignableFrom<IOwnedEntity<Guid>>(new OneToManyFake(Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public void OneToOne_Implements_IOwnedEntity()
    {
        Assert.IsAssignableFrom<IOwnedEntity<Guid>>(new OneToOneFake(Guid.NewGuid()));
    }

    private sealed class NoKeyFake : OwnedEntity
    {
        public NoKeyFake()
        {
        }

        public NoKeyFake(StoredObjectInitialization initialization) : base(initialization)
        {
        }
    }

    private sealed class OneToOneFake : OwnedEntity<Guid>
    {
        public OneToOneFake()
        {
        }

        public OneToOneFake(Guid id) : base(id)
        {
        }

        public OneToOneFake(Guid id, StoredObjectInitialization initialization) : base(id, initialization)
        {
        }
    }

    private sealed class OneToManyFake : OwnedEntity<Guid, Guid>
    {
        public OneToManyFake()
        {
        }

        public OneToManyFake(Guid id, Guid parentId) : base(id, parentId)
        {
        }

        public OneToManyFake(Guid id, Guid parentId, StoredObjectInitialization initialization)
            : base(id, parentId, initialization)
        {
        }
    }
}
