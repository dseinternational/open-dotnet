// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class NamedEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Ctor_WithName_LeavesIdAndTimestampsUnset()
    {
        var entity = new NamedFake("widget");

        Assert.Equal("widget", entity.Name);
        Assert.Equal(default, entity.Id);
        Assert.Null(entity.Created);
        Assert.Null(entity.Updated);
        Assert.Null(entity.Timestamp);
        Assert.Equal(StoredObjectInitialization.Created, entity.Initialization);
    }

    [Fact]
    public void Ctor_WithIdAndName_PopulatesId()
    {
        var id = Guid.NewGuid();
        var entity = new NamedFake(id, "widget");

        Assert.Equal(id, entity.Id);
        Assert.Equal("widget", entity.Name);
        Assert.Equal(StoredObjectInitialization.Created, entity.Initialization);
    }

    [Fact]
    public void Ctor_NullName_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new NamedFake(null!));
    }

    [Fact]
    public void Ctor_WhitespaceName_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new NamedFake(" "));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var created = DateTimeOffset.Parse("2024-01-01T00:00:00Z", CultureInfo.InvariantCulture);
        var updated = DateTimeOffset.Parse("2024-06-01T12:00:00Z", CultureInfo.InvariantCulture);

        var entity = new NamedFake(id, "widget", created, updated, s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal("widget", entity.Name);
        Assert.Equal(created, entity.Created);
    }

    [Fact]
    public void Materialization_Ctor_NullName_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new NamedFake(Guid.NewGuid(), null!, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, s_sampleTimestamp));
    }

    [Fact]
    public void Name_Setter_UpdatesValue()
    {
        var entity = new NamedFake("a") { Name = "b" };
        Assert.Equal("b", entity.Name);
    }

    [Fact]
    public void Name_Setter_RejectsWhitespace()
    {
        var entity = new NamedFake("a");
        _ = Assert.Throws<ArgumentException>(() => entity.Name = " ");
    }

    [Fact]
    public void Implements_INamed()
    {
        Assert.IsAssignableFrom<INamed>(new NamedFake("x"));
    }

    private sealed class NamedFake : NamedEntity<Guid>
    {
        public NamedFake(string name) : base(name)
        {
        }

        public NamedFake(Guid id, string name) : base(id, name)
        {
        }

        public NamedFake(Guid id, string name, DateTimeOffset? created, DateTimeOffset? updated, Timestamp? timestamp)
            : base(id, name, created, updated, timestamp)
        {
        }
    }
}
