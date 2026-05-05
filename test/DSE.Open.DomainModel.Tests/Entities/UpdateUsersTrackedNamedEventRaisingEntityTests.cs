// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.DomainModel.Events;
using DSE.Open.DomainModel.Tests.Events;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedNamedEventRaisingEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Ctor_WithName_LeavesIdAndAuditFieldsUnset()
    {
        var entity = new Fake("widget");

        Assert.Equal("widget", entity.Name);
        Assert.Equal(default, entity.Id);
        Assert.Null(entity.Created);
        Assert.Null(entity.Updated);
        Assert.Null(entity.CreatedUser);
        Assert.Null(entity.UpdatedUser);
        Assert.Null(entity.Timestamp);
        Assert.Equal(StoredObjectInitialization.Created, entity.Initialization);
    }

    [Fact]
    public void Ctor_WithIdAndName_PopulatesId()
    {
        var id = Guid.NewGuid();
        var entity = new Fake(id, "widget");

        Assert.Equal(id, entity.Id);
        Assert.Equal("widget", entity.Name);
    }

    [Fact]
    public void Ctor_NullName_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() => new Fake(null!));
    }

    [Fact]
    public void Ctor_WhitespaceName_Throws()
    {
        _ = Assert.Throws<ArgumentException>(() => new Fake(" "));
    }

    [Fact]
    public void Materialization_Ctor_PopulatesAllFields()
    {
        var id = Guid.NewGuid();
        var created = DateTimeOffset.Parse("2024-01-01T00:00:00Z", CultureInfo.InvariantCulture);
        var updated = DateTimeOffset.Parse("2024-06-01T12:00:00Z", CultureInfo.InvariantCulture);

        var entity = new Fake(id, "widget", created, "alice", updated, "bob", s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal("widget", entity.Name);
        Assert.Equal(created, entity.Created);
        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal(updated, entity.Updated);
        Assert.Equal("bob", entity.UpdatedUser);
        Assert.Equal(s_sampleTimestamp, entity.Timestamp);
    }

    [Fact]
    public void Materialization_Ctor_NullName_Throws()
    {
        _ = Assert.Throws<EntityDataInitializationException>(() =>
            new Fake(Guid.NewGuid(), null!, DateTimeOffset.UtcNow, "alice", DateTimeOffset.UtcNow, "bob", s_sampleTimestamp));
    }

    [Fact]
    public void Name_Setter_RejectsWhitespace()
    {
        var entity = new Fake("a");
        _ = Assert.Throws<ArgumentException>(() => entity.Name = " ");
    }

    [Fact]
    public void Implements_INamed_IUpdateUsersTracked_IEventRaisingEntity()
    {
        var entity = new Fake("x");
        Assert.IsAssignableFrom<INamed>(entity);
        Assert.IsAssignableFrom<IUpdateUsersTracked>(entity);
        Assert.IsAssignableFrom<IEventRaisingEntity>(entity);
    }

    [Fact]
    public void AddEvent_AppendsToEventsCollection()
    {
        var entity = new Fake("x");
        var domainEvent = new DomainEventFake("test");

        entity.AddFakeEvent(domainEvent);

        Assert.Single(entity.Events);
        Assert.Same(domainEvent, entity.Events.First());
    }

    private sealed class Fake : UpdateUsersTrackedNamedEventRaisingEntity<Guid>
    {
        public Fake(string name) : base(name)
        {
        }

        public Fake(Guid id, string name) : base(id, name)
        {
        }

        public Fake(
            Guid id,
            string name,
            DateTimeOffset? created,
            string? createdUser,
            DateTimeOffset? updated,
            string? updatedUser,
            Timestamp? timestamp)
            : base(id, name, created, createdUser, updated, updatedUser, timestamp)
        {
        }

        public void AddFakeEvent(IDomainEvent @event) => AddEvent(@event);
    }
}
