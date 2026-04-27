// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class TitleDescriptionEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Ctor_PopulatesTitleAndDescription()
    {
        var entity = new TitleDescriptionFake("Hello", "World");
        Assert.Equal("Hello", entity.Title);
        Assert.Equal("World", entity.Description);
    }

    [Fact]
    public void Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new TitleDescriptionFake("t", null!));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var entity = new TitleDescriptionFake(
            id, "Hello", "World", DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow, s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal("World", entity.Description);
    }

    [Fact]
    public void Materialization_Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new TitleDescriptionFake(Guid.NewGuid(), "t", null!, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, s_sampleTimestamp));
    }

    [Fact]
    public void Implements_ITitled_And_IDescribed()
    {
        var entity = new TitleDescriptionFake("t", "d");
        Assert.IsAssignableFrom<ITitled>(entity);
        Assert.IsAssignableFrom<IDescribed>(entity);
    }

    private sealed class TitleDescriptionFake : TitleDescriptionEntity<Guid>
    {
        public TitleDescriptionFake(string title, string description) : base(title, description)
        {
        }

        public TitleDescriptionFake(
            Guid id,
            string title,
            string description,
            DateTimeOffset? created,
            DateTimeOffset? updated,
            Timestamp? timestamp)
            : base(id, title, description, created, updated, timestamp)
        {
        }
    }
}
