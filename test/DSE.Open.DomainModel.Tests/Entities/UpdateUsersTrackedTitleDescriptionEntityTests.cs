// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedTitleDescriptionEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Ctor_PopulatesTitleAndDescription()
    {
        var entity = new Fake("Widget", "A small widget");

        Assert.Equal("Widget", entity.Title);
        Assert.Equal("A small widget", entity.Description);
    }

    [Fact]
    public void Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new Fake("t", null!));
    }

    [Fact]
    public void Ctor_WhitespaceDescription_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake("t", " "));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var entity = new Fake(
            id,
            "Widget",
            "desc",
            DateTimeOffset.UtcNow.AddDays(-1),
            "alice",
            DateTimeOffset.UtcNow,
            "bob",
            s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal("Widget", entity.Title);
        Assert.Equal("desc", entity.Description);
        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal("bob", entity.UpdatedUser);
    }

    [Fact]
    public void Materialization_Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(Guid.NewGuid(), "t", null!, DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, "bob", s_sampleTimestamp));
    }

    [Fact]
    public void Description_Setter_RejectsWhitespace()
    {
        var entity = new Fake("t", "d");
        _ = Assert.Throws<ArgumentException>(() => entity.Description = "  ");
    }

    [Fact]
    public void Implements_ITitled_IDescribed_IUpdateUsersTracked()
    {
        var entity = new Fake("t", "d");
        Assert.IsAssignableFrom<ITitled>(entity);
        Assert.IsAssignableFrom<IDescribed>(entity);
        Assert.IsAssignableFrom<IUpdateUsersTracked>(entity);
    }

    private sealed class Fake : UpdateUsersTrackedTitleDescriptionEntity<Guid>
    {
        public Fake(string title, string description) : base(title, description)
        {
        }

        public Fake(
            Guid id,
            string title,
            string description,
            DateTimeOffset? created,
            string? createdUser,
            DateTimeOffset? updated,
            string? updatedUser,
            Timestamp? timestamp)
            : base(id, title, description, created, createdUser, updated, updatedUser, timestamp)
        {
        }
    }
}
