// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class NameDescriptionEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Ctor_PopulatesNameAndDescription()
    {
        var entity = new NameDescriptionFake("widget", "a small widget");

        Assert.Equal("widget", entity.Name);
        Assert.Equal("a small widget", entity.Description);
    }

    [Fact]
    public void Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new NameDescriptionFake("n", null!));
    }

    [Fact]
    public void Ctor_WhitespaceDescription_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new NameDescriptionFake("n", " "));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var created = DateTimeOffset.UtcNow.AddDays(-1);
        var updated = DateTimeOffset.UtcNow;

        var entity = new NameDescriptionFake(id, "widget", "desc", created, updated, s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal("desc", entity.Description);
    }

    [Fact]
    public void Materialization_Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new NameDescriptionFake(Guid.NewGuid(), "n", null!, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, s_sampleTimestamp));
    }

    [Fact]
    public void Description_Setter_RejectsWhitespace()
    {
        var entity = new NameDescriptionFake("n", "d");
        _ = Assert.Throws<ArgumentException>(() => entity.Description = "  ");
    }

    [Fact]
    public void Implements_INamed_And_IDescribed()
    {
        var entity = new NameDescriptionFake("n", "d");
        Assert.IsAssignableFrom<INamed>(entity);
        Assert.IsAssignableFrom<IDescribed>(entity);
    }

    private sealed class NameDescriptionFake : NameDescriptionEntity<Guid>
    {
        public NameDescriptionFake(string name, string description) : base(name, description)
        {
        }

        public NameDescriptionFake(
            Guid id,
            string name,
            string description,
            DateTimeOffset? created,
            DateTimeOffset? updated,
            Timestamp? timestamp)
            : base(id, name, description, created, updated, timestamp)
        {
        }
    }
}
