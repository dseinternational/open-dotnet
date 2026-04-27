// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Parameterless_Ctor_LeavesFieldsUnset()
    {
        var entity = new Fake();

        Assert.Null(entity.Created);
        Assert.Null(entity.Updated);
        Assert.Null(entity.CreatedUser);
        Assert.Null(entity.UpdatedUser);
        Assert.Null(entity.Timestamp);
        Assert.Equal(default, entity.Id);
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var created = DateTimeOffset.Parse("2024-01-01T00:00:00Z", CultureInfo.InvariantCulture);
        var updated = DateTimeOffset.Parse("2024-06-01T12:00:00Z", CultureInfo.InvariantCulture);

        var entity = new Fake(id, created, "alice", updated, "bob", s_sampleTimestamp);

        Assert.Equal(id, entity.Id);
        Assert.Equal(created, entity.Created);
        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal(updated, entity.Updated);
        Assert.Equal("bob", entity.UpdatedUser);
        Assert.Equal(s_sampleTimestamp, entity.Timestamp);
        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
    }

    [Fact]
    public void Materialization_Ctor_NullCreatedUser_Throws()
    {
        var id = Guid.NewGuid();
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(id, DateTimeOffset.UtcNow, null, DateTimeOffset.UtcNow, "bob", s_sampleTimestamp));
    }

    [Fact]
    public void Materialization_Ctor_NullUpdatedUser_Throws()
    {
        var id = Guid.NewGuid();
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(id, DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, null, s_sampleTimestamp));
    }

    [Fact]
    public void Materialization_Ctor_NullTimestamp_Throws()
    {
        var id = Guid.NewGuid();
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(id, DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, "bob", null));
    }

    [Fact]
    public void SetCreatedUser_AssignsCreatedAndUpdatedUser()
    {
        var entity = new Fake();
        entity.SetCreatedUserPublic("alice");

        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal("alice", entity.UpdatedUser);
    }

    [Fact]
    public void SetCreatedUser_WhenAlreadySet_Throws()
    {
        var entity = new Fake();
        entity.SetCreatedUserPublic("alice");

        _ = Assert.Throws<InvalidOperationException>(() => entity.SetCreatedUserPublic("bob"));
    }

    [Fact]
    public void SetUpdatedUser_BeforeCreatedUser_Throws()
    {
        var entity = new Fake();
        _ = Assert.Throws<InvalidOperationException>(() => entity.SetUpdatedUserPublic("bob"));
    }

    [Fact]
    public void SetUpdatedUser_AfterCreated_Updates()
    {
        var entity = new Fake();
        entity.SetCreatedUserPublic("alice");
        entity.SetUpdatedUserPublic("bob");

        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal("bob", entity.UpdatedUser);
    }

    [Fact]
    public void Implements_IUpdateUsersTracked()
    {
        Assert.IsAssignableFrom<IUpdateUsersTracked>(new Fake());
    }

    private sealed class Fake : UpdateUsersTrackedEntity<Guid>
    {
        public Fake()
        {
        }

        public Fake(Guid id) : base(id)
        {
        }

        public Fake(
            Guid id,
            DateTimeOffset? created,
            string? createdUser,
            DateTimeOffset? updated,
            string? updatedUser,
            Timestamp? timestamp)
            : base(id, created, createdUser, updated, updatedUser, timestamp)
        {
        }

        public void SetCreatedUserPublic(string user) => SetCreatedUser(user);
        public void SetUpdatedUserPublic(string user) => SetUpdatedUser(user);
    }
}
