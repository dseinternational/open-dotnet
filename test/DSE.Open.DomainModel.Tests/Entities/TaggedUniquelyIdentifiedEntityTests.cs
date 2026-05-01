// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.Values;

namespace DSE.Open.DomainModel.Tests.Entities;

public class TaggedUniquelyIdentifiedEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);
    private static readonly Tag s_tag = new("widget");

    [Fact]
    public void Ctor_WithTag_GeneratesUniqueId()
    {
        var entity = new GuidKeyFake(s_tag);
        Assert.Equal(s_tag, entity.Tag);
        Assert.NotEqual(Guid.Empty, entity.UniqueId);
    }

    [Fact]
    public void Ctor_WithUniqueIdAndTag_PopulatesBoth()
    {
        var uniqueId = Guid.NewGuid();
        var entity = new GuidKeyFake(uniqueId, s_tag);
        Assert.Equal(uniqueId, entity.UniqueId);
        Assert.Equal(s_tag, entity.Tag);
    }

    [Fact]
    public void Ctor_WithIdAndTag_GeneratesUniqueId()
    {
        const int id = 42;
        var entity = new IntKeyFake(id, s_tag);
        Assert.Equal(id, entity.Id);
        Assert.NotEqual(Guid.Empty, entity.UniqueId);
        Assert.Equal(s_tag, entity.Tag);
    }

    [Fact]
    public void Ctor_WithIdUniqueIdAndTag_PopulatesAll()
    {
        const int id = 42;
        var uniqueId = Guid.NewGuid();
        var entity = new IntKeyFake(id, uniqueId, s_tag);
        Assert.Equal(id, entity.Id);
        Assert.Equal(uniqueId, entity.UniqueId);
        Assert.Equal(s_tag, entity.Tag);
    }

    [Fact]
    public void Ctor_DefaultTag_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new GuidKeyFake(default(Tag)));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        const int id = 42;
        var uniqueId = Guid.NewGuid();

        var entity = new IntKeyFake(
            id,
            uniqueId,
            s_tag,
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow,
            s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal(id, entity.Id);
        Assert.Equal(uniqueId, entity.UniqueId);
        Assert.Equal(s_tag, entity.Tag);
    }

    [Fact]
    public void Materialization_Ctor_DefaultTag_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new IntKeyFake(42, Guid.NewGuid(), default(Tag), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, s_sampleTimestamp));
    }

    [Fact]
    public void Tag_Setter_RejectsDefault()
    {
        var entity = new GuidKeyFake(s_tag);
        _ = Assert.Throws<ArgumentException>(() => entity.Tag = default);
    }

    [Fact]
    public void Implements_IUniquelyIdentifiedEntity()
    {
        Assert.IsAssignableFrom<IUniquelyIdentifiedEntity<Guid>>(new GuidKeyFake(s_tag));
    }

    private sealed class GuidKeyFake : TaggedUniquelyIdentifiedEntity<Guid>
    {
        public GuidKeyFake(Tag tag) : base(tag)
        {
        }

        public GuidKeyFake(Guid uniqueId, Tag tag) : base(uniqueId, tag)
        {
        }
    }

    private sealed class IntKeyFake : TaggedUniquelyIdentifiedEntity<int>
    {
        public IntKeyFake(int id, Tag tag) : base(id, tag)
        {
        }

        public IntKeyFake(int id, Guid uniqueId, Tag tag) : base(id, uniqueId, tag)
        {
        }

        public IntKeyFake(
            int id,
            Guid uniqueId,
            Tag tag,
            DateTimeOffset? created,
            DateTimeOffset? updated,
            Timestamp? timestamp)
            : base(id, uniqueId, tag, created, updated, timestamp)
        {
        }
    }
}
