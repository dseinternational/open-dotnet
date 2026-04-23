// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Entities;
using DSE.Open.DomainModel.Tests.Events;

namespace DSE.Open.DomainModel.Tests.Entities;

public class EventRaisingEntityExtensionsTests
{
    [Fact]
    public void GetBeforeSaveChangesEvents_NoEvents_ReturnsEmpty()
    {
        var entity = new EventRaisingEntityFake<Guid>();

        var result = entity.GetBeforeSaveChangesEvents();

        Assert.Empty(result);
    }

    [Fact]
    public void GetBeforeSaveChangesEvents_OnlyRegularEvents_ReturnsEmpty()
    {
        var entity = new EventRaisingEntityFake<Guid>();
        _ = entity.AddFakeEvent();

        var result = entity.GetBeforeSaveChangesEvents();

        Assert.Empty(result);
    }

    [Fact]
    public void GetBeforeSaveChangesEvents_ReturnsOnlyBeforeSaveChangesEvents()
    {
        var entity = new EventRaisingEntityFake<Guid>();
        _ = entity.AddFakeEvent();
        var before = new DomainBeforeSaveChangesEventFake("before");
        entity.AddDomainEvent(before);
        _ = entity.AddFakeBackgroundEvent();

        var result = entity.GetBeforeSaveChangesEvents().ToList();

        _ = Assert.Single(result);
        Assert.Same(before, result[0]);
    }

    [Fact]
    public void HasBeforeSaveChangesEvents_NoEvents_ReturnsFalse()
    {
        var entity = new EventRaisingEntityFake<Guid>();

        Assert.False(entity.HasBeforeSaveChangesEvents());
    }

    [Fact]
    public void HasBeforeSaveChangesEvents_OnlyRegularEvents_ReturnsFalse()
    {
        var entity = new EventRaisingEntityFake<Guid>();
        _ = entity.AddFakeEvent();

        Assert.False(entity.HasBeforeSaveChangesEvents());
    }

    [Fact]
    public void HasBeforeSaveChangesEvents_ContainsBeforeSaveChangesEvent_ReturnsTrue()
    {
        var entity = new EventRaisingEntityFake<Guid>();
        entity.AddDomainEvent(new DomainBeforeSaveChangesEventFake("before"));

        Assert.True(entity.HasBeforeSaveChangesEvents());
    }

    [Fact]
    public void GetBeforeSaveChangesEvents_NullEntity_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            ((IEventRaisingEntity)null!).GetBeforeSaveChangesEvents());
    }

    [Fact]
    public void HasBeforeSaveChangesEvents_NullEntity_Throws()
    {
        _ = Assert.Throws<ArgumentNullException>(() =>
            ((IEventRaisingEntity)null!).HasBeforeSaveChangesEvents());
    }
}
