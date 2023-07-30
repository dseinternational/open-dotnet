// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Abstractions;
using DSE.Open.DomainModel.Entities;
using DSE.Open.DomainModel.Tests.Events;

namespace DSE.Open.DomainModel.Tests.Entities;

public class EventRaisingEntityFake<TId> : EventRaisingEntity<TId>
    where TId : struct, IEquatable<TId>
{
    public EventRaisingEntityFake()
    {
    }

    [MaterializationConstructor]
    internal EventRaisingEntityFake(TId id, StoredObjectInitialization initialization = StoredObjectInitialization.Created) : base(id, initialization)
    {
    }

    internal DomainEventFake AddFakeEvent()
    {
        var ev = new DomainEventFake("Test");
        AddEvent(ev);
        return ev;
    }

    internal DomainBackgroundEventFake AddFakeBackgroundEvent()
    {
        var ev = new DomainBackgroundEventFake("Background Test");
        AddEvent(ev);
        return ev;
    }
}
