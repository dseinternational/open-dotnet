// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;
using DSE.Open.DomainModel.Tests.Entities;

namespace DSE.Open.DomainModel.Tests.Events;

public class DomainLoopingEventFake : DomainEvent<EventRaisingEntityFake<Guid>>
{
    public DomainLoopingEventFake(EventRaisingEntityFake<Guid> entity) : base(entity)
    {
    }

    public override string Type => "org.dsegroup.testing.domain_looping_event_fake";
}
