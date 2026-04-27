// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class NamedOwnedEntityTests
{
    [Fact]
    public void Ctor_PopulatesName()
    {
        var entity = new Fake("widget");

        Assert.Equal("widget", entity.Name);
        Assert.Equal(StoredObjectInitialization.Created, entity.Initialization);
    }

    [Fact]
    public void Ctor_NullName_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new Fake(null!));
    }

    [Fact]
    public void Ctor_WhitespaceName_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake(" "));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesName()
    {
        var entity = new Fake("widget", StoredObjectInitialization.Materialized);

        Assert.Equal("widget", entity.Name);
        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
    }

    [Fact]
    public void Materialization_Ctor_NullName_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(null!, StoredObjectInitialization.Materialized));
    }

    [Fact]
    public void Name_Setter_RejectsWhitespace()
    {
        var entity = new Fake("widget");
        _ = Assert.Throws<ArgumentException>(() => entity.Name = " ");
    }

    [Fact]
    public void Implements_INamed()
    {
        Assert.IsAssignableFrom<INamed>(new Fake("n"));
    }

    private sealed class Fake : NamedOwnedEntity
    {
        public Fake(string name) : base(name)
        {
        }

        public Fake(string name, StoredObjectInitialization initialization) : base(name, initialization)
        {
        }
    }
}
