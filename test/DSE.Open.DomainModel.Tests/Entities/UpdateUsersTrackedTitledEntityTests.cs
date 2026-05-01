// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedTitledEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Ctor_WithTitle_PopulatesTitle()
    {
        var entity = new Fake("Widget");
        Assert.Equal("Widget", entity.Title);
    }

    [Fact]
    public void Ctor_NullTitle_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new Fake(null!));
    }

    [Fact]
    public void Ctor_WhitespaceTitle_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake(" "));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var entity = new Fake(
            id,
            "Widget",
            DateTimeOffset.UtcNow.AddDays(-1),
            "alice",
            DateTimeOffset.UtcNow,
            "bob",
            s_sampleTimestamp);

        Assert.Equal(id, entity.Id);
        Assert.Equal("Widget", entity.Title);
        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal("bob", entity.UpdatedUser);
        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
    }

    [Fact]
    public void Materialization_Ctor_NullTitle_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(Guid.NewGuid(), null!, DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, "bob", s_sampleTimestamp));
    }

    [Fact]
    public void Title_Setter_RejectsWhitespace()
    {
        var entity = new Fake("a");
        _ = Assert.Throws<ArgumentException>(() => entity.Title = " ");
    }

    [Fact]
    public void Implements_ITitled_And_IUpdateUsersTracked()
    {
        var entity = new Fake("x");
        Assert.IsAssignableFrom<ITitled>(entity);
        Assert.IsAssignableFrom<IUpdateUsersTracked>(entity);
    }

    private sealed class Fake : UpdateUsersTrackedTitledEntity<Guid>
    {
        public Fake(string title) : base(title)
        {
        }

        public Fake(
            Guid id,
            string title,
            DateTimeOffset? created,
            string? createdUser,
            DateTimeOffset? updated,
            string? updatedUser,
            Timestamp? timestamp)
            : base(id, title, created, createdUser, updated, updatedUser, timestamp)
        {
        }
    }
}
