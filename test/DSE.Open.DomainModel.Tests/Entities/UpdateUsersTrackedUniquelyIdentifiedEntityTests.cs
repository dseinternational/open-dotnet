// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedUniquelyIdentifiedEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Parameterless_Ctor_GeneratesUniqueId()
    {
        var entity = new Fake();
        Assert.NotEqual(Guid.Empty, entity.UniqueId);
        Assert.Null(entity.CreatedUser);
        Assert.Null(entity.UpdatedUser);
    }

    [Fact]
    public void Ctor_WithUniqueId_PopulatesIt()
    {
        var uniqueId = Guid.NewGuid();
        var entity = new Fake(uniqueId);
        Assert.Equal(uniqueId, entity.UniqueId);
    }

    [Fact]
    public void Ctor_WithEmptyUniqueId_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake(Guid.Empty));
    }

    [Fact]
    public void Ctor_WithIdAndUniqueId_PopulatesBoth()
    {
        const int id = 42;
        var uniqueId = Guid.NewGuid();
        var entity = new IntKeyFake(id, uniqueId);

        Assert.Equal(id, entity.Id);
        Assert.Equal(uniqueId, entity.UniqueId);
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        const int id = 42;
        var uniqueId = Guid.NewGuid();
        var created = DateTimeOffset.UtcNow.AddDays(-1);
        var updated = DateTimeOffset.UtcNow;

        var entity = new IntKeyFake(id, uniqueId, created, "alice", updated, "bob", s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal(id, entity.Id);
        Assert.Equal(uniqueId, entity.UniqueId);
        Assert.Equal(created, entity.Created);
        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal(updated, entity.Updated);
        Assert.Equal("bob", entity.UpdatedUser);
        Assert.Equal(s_sampleTimestamp, entity.Timestamp);
    }

    [Fact]
    public void Materialization_Ctor_EmptyUniqueId_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new IntKeyFake(42, Guid.Empty, DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, "bob", s_sampleTimestamp));
    }

    [Fact]
    public void Implements_IUniquelyIdentified_And_IUpdateUsersTracked()
    {
        var entity = new Fake();
        Assert.IsAssignableFrom<IUniquelyIdentified>(entity);
        Assert.IsAssignableFrom<IUniquelyIdentifiedEntity<Guid>>(entity);
        Assert.IsAssignableFrom<IUpdateUsersTracked>(entity);
    }

    private sealed class Fake : UpdateUsersTrackedUniquelyIdentifiedEntity<Guid>
    {
        public Fake()
        {
        }

        public Fake(Guid uniqueId) : base(uniqueId)
        {
        }
    }

    private sealed class IntKeyFake : UpdateUsersTrackedUniquelyIdentifiedEntity<int>
    {
        public IntKeyFake(int id, Guid uniqueId) : base(id, uniqueId)
        {
        }

        public IntKeyFake(
            int id,
            Guid uniqueId,
            DateTimeOffset? created,
            string? createdUser,
            DateTimeOffset? updated,
            string? updatedUser,
            Timestamp? timestamp)
            : base(id, uniqueId, created, createdUser, updated, updatedUser, timestamp)
        {
        }
    }
}
