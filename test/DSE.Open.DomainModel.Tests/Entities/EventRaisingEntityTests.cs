// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.DomainModel.Events;
using DSE.Open.DomainModel.Tests.Events;

namespace DSE.Open.DomainModel.Tests.Entities;

public class EventRaisingEntityTests
{
    [Fact]
    public void HasEvents_WhenNoEventsAdded_IsFalse()
    {
        var entity = new EventRaisingEntityFake<Guid>();

        Assert.False(entity.HasEvents);
        Assert.Empty(entity.Events);
    }

    [Fact]
    public void AddEvent_AppendsEvent()
    {
        var entity = new EventRaisingEntityFake<Guid>();

        var ev = entity.AddFakeEvent();

        Assert.True(entity.HasEvents);
        Assert.Contains(ev, entity.Events);
    }

    [Fact]
    public void AddEvent_Null_Throws()
    {
        var entity = new EventRaisingEntityFake<Guid>();

        _ = Assert.Throws<ArgumentNullException>(() => entity.AddDomainEvent(null!));
    }

    [Fact]
    public void ClearEvents_RemovesAllEvents()
    {
        var entity = new EventRaisingEntityFake<Guid>();
        _ = entity.AddFakeEvent();
        entity.AddDomainEvent(new DomainBeforeSaveChangesEventFake("before"));

        ((IEventRaisingEntity)entity).ClearEvents();

        Assert.False(entity.HasEvents);
    }

    [Fact]
    public void ClearBeforeSaveChangesEvents_LeavesNonBeforeSaveEvents()
    {
        var entity = new EventRaisingEntityFake<Guid>();
        var regular = entity.AddFakeEvent();
        entity.AddDomainEvent(new DomainBeforeSaveChangesEventFake("before"));

        ((IEventRaisingEntity)entity).ClearBeforeSaveChangesEvents();

        Assert.True(entity.HasEvents);
        Assert.Single(entity.Events);
        Assert.Contains(regular, entity.Events);
    }

    [Fact]
    public void ClearEvents_OnEmptyEntity_DoesNotThrow()
    {
        var entity = new EventRaisingEntityFake<Guid>();

        ((IEventRaisingEntity)entity).ClearEvents();
        ((IEventRaisingEntity)entity).ClearBeforeSaveChangesEvents();

        Assert.False(entity.HasEvents);
    }

    [Fact]
    public void HasEvents_AfterClearingAll_IsFalse()
    {
        var entity = new EventRaisingEntityFake<Guid>();
        _ = entity.AddFakeEvent();
        _ = entity.AddFakeEvent();

        ((IEventRaisingEntity)entity).ClearEvents();

        Assert.False(entity.HasEvents);
        Assert.Empty(entity.Events);
    }
}
