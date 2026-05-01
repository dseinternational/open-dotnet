// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.Values;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedTaggedEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);
    private static readonly Tag s_sampleTag = new("widget");

    [Fact]
    public void Ctor_WithTag_PopulatesTag()
    {
        var entity = new Fake(s_sampleTag);
        Assert.Equal(s_sampleTag, entity.Tag);
    }

    [Fact]
    public void Ctor_WithIdAndTag_PopulatesBoth()
    {
        var id = Guid.NewGuid();
        var entity = new Fake(id, s_sampleTag);

        Assert.Equal(id, entity.Id);
        Assert.Equal(s_sampleTag, entity.Tag);
    }

    [Fact]
    public void Ctor_DefaultTag_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake(default(Tag)));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var entity = new Fake(
            id,
            s_sampleTag,
            DateTimeOffset.UtcNow.AddDays(-1),
            "alice",
            DateTimeOffset.UtcNow,
            "bob",
            s_sampleTimestamp);

        Assert.Equal(s_sampleTag, entity.Tag);
        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal("bob", entity.UpdatedUser);
        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
    }

    [Fact]
    public void Materialization_Ctor_DefaultTag_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(Guid.NewGuid(), default(Tag), DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, "bob", s_sampleTimestamp));
    }

    [Fact]
    public void Tag_Setter_RejectsDefault()
    {
        var entity = new Fake(s_sampleTag);
        _ = Assert.Throws<ArgumentException>(() => entity.Tag = default);
    }

    [Fact]
    public void Tag_Setter_UpdatesValue()
    {
        var entity = new Fake(s_sampleTag);
        var newTag = new Tag("other");
        entity.Tag = newTag;
        Assert.Equal(newTag, entity.Tag);
    }

    [Fact]
    public void Implements_IUpdateUsersTracked()
    {
        Assert.IsAssignableFrom<IUpdateUsersTracked>(new Fake(s_sampleTag));
    }

    private sealed class Fake : UpdateUsersTrackedTaggedEntity<Guid>
    {
        public Fake(Tag tag) : base(tag)
        {
        }

        public Fake(Guid id, Tag tag) : base(id, tag)
        {
        }

        public Fake(
            Guid id,
            Tag tag,
            DateTimeOffset? created,
            string? createdUser,
            DateTimeOffset? updated,
            string? updatedUser,
            Timestamp? timestamp)
            : base(id, tag, created, createdUser, updated, updatedUser, timestamp)
        {
        }
    }
}
