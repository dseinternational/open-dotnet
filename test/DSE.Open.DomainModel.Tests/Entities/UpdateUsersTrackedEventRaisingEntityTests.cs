// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.DomainModel.Events;
using DSE.Open.DomainModel.Tests.Events;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedEventRaisingEntityTests
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
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(Guid.NewGuid(), DateTimeOffset.UtcNow, null, DateTimeOffset.UtcNow, "bob", s_sampleTimestamp));
    }

    [Fact]
    public void Materialization_Ctor_NullUpdatedUser_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(Guid.NewGuid(), DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, null, s_sampleTimestamp));
    }

    [Fact]
    public void Materialization_Ctor_NullTimestamp_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(Guid.NewGuid(), DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, "bob", null));
    }

    [Fact]
    public void IUpdateUsersTracked_SetCreatedUser_Stamps()
    {
        var entity = new Fake();
        ((IUpdateUsersTracked)entity).SetCreatedUser("alice");

        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal("alice", entity.UpdatedUser);
    }

    [Fact]
    public void IUpdateUsersTracked_SetUpdatedUser_AfterCreated_Stamps()
    {
        var entity = new Fake();
        ((IUpdateUsersTracked)entity).SetCreatedUser("alice");
        ((IUpdateUsersTracked)entity).SetUpdatedUser("bob");

        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal("bob", entity.UpdatedUser);
    }

    [Fact]
    public void IUpdateUsersTracked_SetUpdatedUser_BeforeCreatedUser_Throws()
    {
        var entity = new Fake();
        _ = Assert.Throws<InvalidOperationException>(() =>
            ((IUpdateUsersTracked)entity).SetUpdatedUser("bob"));
    }

    [Fact]
    public void IUpdateUsersTracked_SetCreatedUser_WhenAlreadySet_Throws()
    {
        var entity = new Fake();
        ((IUpdateUsersTracked)entity).SetCreatedUser("alice");

        _ = Assert.Throws<InvalidOperationException>(() =>
            ((IUpdateUsersTracked)entity).SetCreatedUser("bob"));
    }

    [Fact]
    public void Implements_IUpdateUsersTracked_And_IEventRaisingEntity()
    {
        var entity = new Fake();
        Assert.IsAssignableFrom<IUpdateUsersTracked>(entity);
        Assert.IsAssignableFrom<IEventRaisingEntity>(entity);
    }

    [Fact]
    public void AddEvent_AppendsToEventsCollection()
    {
        var entity = new Fake();
        var domainEvent = new DomainEventFake("test");

        entity.AddFakeEvent(domainEvent);

        Assert.Single(entity.Events);
        Assert.Same(domainEvent, entity.Events.First());
    }

    [Fact]
    public void SetTimestamp_AssignsTimestamp_OnNewEntity()
    {
        var entity = new Fake();

        Assert.Null(entity.Timestamp);
        entity.AssignTimestamp(s_sampleTimestamp);

        Assert.Equal(s_sampleTimestamp, entity.Timestamp);
    }

    [Fact]
    public void SetTimestamp_DefaultValue_Throws()
    {
        var entity = new Fake();

        _ = Assert.Throws<EntityDataInitializationException>(() => entity.AssignTimestamp(default));
    }

    private sealed class Fake : UpdateUsersTrackedEventRaisingEntity<Guid>
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

        public void AddFakeEvent(IDomainEvent @event) => AddEvent(@event);

        public void AssignTimestamp(Timestamp value) => SetTimestamp(value);
    }
}
