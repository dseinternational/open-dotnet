// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.DomainModel.Events;
using DSE.Open.DomainModel.Tests.Events;

namespace DSE.Open.DomainModel.Tests.Entities;

public class UpdateUsersTrackedTitledEventRaisingEntityTests
{
    private static readonly Timestamp s_sampleTimestamp = new([1, 2, 3, 4, 5, 6, 7, 8]);

    [Fact]
    public void Ctor_WithTitle_LeavesIdAndAuditFieldsUnset()
    {
        var entity = new Fake("Widget");

        Assert.Equal("Widget", entity.Title);
        Assert.Equal(default, entity.Id);
        Assert.Equal(StoredObjectInitialization.Created, entity.Initialization);
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
        var created = DateTimeOffset.UtcNow.AddDays(-1);
        var updated = DateTimeOffset.UtcNow;

        var entity = new Fake(id, "Widget", created, "alice", updated, "bob", s_sampleTimestamp);

        Assert.Equal(StoredObjectInitialization.Materialized, entity.Initialization);
        Assert.Equal(id, entity.Id);
        Assert.Equal("Widget", entity.Title);
        Assert.Equal(created, entity.Created);
        Assert.Equal("alice", entity.CreatedUser);
        Assert.Equal(updated, entity.Updated);
        Assert.Equal("bob", entity.UpdatedUser);
        Assert.Equal(s_sampleTimestamp, entity.Timestamp);
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
        var entity = new Fake("t");
        _ = Assert.Throws<ArgumentException>(() => entity.Title = " ");
    }

    [Fact]
    public void Implements_ITitled_IUpdateUsersTracked_IEventRaisingEntity()
    {
        var entity = new Fake("t");
        Assert.IsAssignableFrom<ITitled>(entity);
        Assert.IsAssignableFrom<IUpdateUsersTracked>(entity);
        Assert.IsAssignableFrom<IEventRaisingEntity>(entity);
    }

    [Fact]
    public void AddEvent_AppendsToEventsCollection()
    {
        var entity = new Fake("t");
        var domainEvent = new DomainEventFake("test");

        entity.AddFakeEvent(domainEvent);

        Assert.Single(entity.Events);
    }

    private sealed class Fake : UpdateUsersTrackedTitledEventRaisingEntity<Guid>
    {
        public Fake(string title) : base(title)
        {
        }

        public Fake(Guid id, string title) : base(id, title)
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

        public void AddFakeEvent(IDomainEvent @event) => AddEvent(@event);
    }
}
