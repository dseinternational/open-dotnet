// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class EntityTests
{
    [Fact]
    public void Parameterless_Ctor_DefaultsToCreatedState()
    {
        var entity = new GuidEntityFake();

        Assert.Equal(StoredObjectInitialization.Created, entity.Initialization);
        Assert.Equal(default, entity.Id);
    }

    [Fact]
    public void Id_Ctor_DefaultsToCreatedState()
    {
        var id = Guid.NewGuid();
        var entity = new GuidEntityFake(id);

        Assert.Equal(StoredObjectInitialization.Created, entity.Initialization);
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void Materialized_Ctor_StoresIdAndReportsMaterialized()
    {
        var id = Guid.NewGuid();
        var entity = new GuidEntityFake(id, StoredObjectInitialization.Materialized);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void Materialized_Ctor_DefaultId_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new GuidEntityFake(default, StoredObjectInitialization.Materialized));
    }

    [Fact]
    public void ToString_IncludesTypeNameAndId()
    {
        var id = Guid.NewGuid();
        var entity = new GuidEntityFake(id);

        var text = entity.ToString();

        Assert.Contains(nameof(GuidEntityFake), text, StringComparison.Ordinal);
        Assert.Contains(id.ToString(), text, StringComparison.Ordinal);
    }

    [Fact]
    public void Id_ObjectAccessor_ReturnsTypedId()
    {
        var id = Guid.NewGuid();
        IIdentified entity = new GuidEntityFake(id);

        Assert.Equal(id, entity.Id);
    }

    private sealed class GuidEntityFake : Entity<Guid>
    {
        public GuidEntityFake()
        {
        }

        public GuidEntityFake(Guid id, StoredObjectInitialization initialization = StoredObjectInitialization.Created)
            : base(id, initialization)
        {
        }
    }
}
