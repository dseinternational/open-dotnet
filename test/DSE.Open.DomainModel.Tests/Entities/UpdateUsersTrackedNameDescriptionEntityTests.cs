// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedNameDescriptionEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Ctor_PopulatesNameAndDescription()
    {
        var entity = new Fake("widget", "a small widget");

        Assert.Equal("widget", entity.Name);
        Assert.Equal("a small widget", entity.Description);
    }

    [Fact]
    public void Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new Fake("n", null!));
    }

    [Fact]
    public void Ctor_WhitespaceDescription_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake("n", " "));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var created = DateTimeOffset.UtcNow.AddDays(-1);
        var updated = DateTimeOffset.UtcNow;

        var entity = new Fake(id, "widget", "desc", created, "alice", updated, "bob", s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal("desc", entity.Description);
        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal("bob", entity.UpdatedUser);
    }

    [Fact]
    public void Materialization_Ctor_NullDescription_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(Guid.NewGuid(), "n", null!, DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, "bob", s_sampleTimestamp));
    }

    [Fact]
    public void Description_Setter_RejectsWhitespace()
    {
        var entity = new Fake("n", "d");
        _ = Assert.Throws<ArgumentException>(() => entity.Description = "  ");
    }

    [Fact]
    public void Implements_INamed_IDescribed_IUpdateUsersTracked()
    {
        var entity = new Fake("n", "d");
        Assert.IsAssignableFrom<INamed>(entity);
        Assert.IsAssignableFrom<IDescribed>(entity);
        Assert.IsAssignableFrom<IUpdateUsersTracked>(entity);
    }

    private sealed class Fake : UpdateUsersTrackedNameDescriptionEntity<Guid>
    {
        public Fake(string name, string description) : base(name, description)
        {
        }

        public Fake(
            Guid id,
            string name,
            string description,
            DateTimeOffset? created,
            string? createdUser,
            DateTimeOffset? updated,
            string? updatedUser,
            Timestamp? timestamp)
            : base(id, name, description, created, createdUser, updated, updatedUser, timestamp)
        {
        }
    }
}
