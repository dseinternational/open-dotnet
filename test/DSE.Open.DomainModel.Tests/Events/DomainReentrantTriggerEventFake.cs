// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;
using DSE.Open.DomainModel.Tests.Entities;

namespace DSE.Open.DomainModel.Tests.Events;

public class DomainReentrantTriggerEventFake : DomainEvent<EventRaisingEntityFake<Guid>>
{
    public DomainReentrantTriggerEventFake(EventRaisingEntityFake<Guid> entity) : base(entity)
    {
    }

    public Guid Instance { get; } = Guid.NewGuid();

    public override string Type => "org.dsegroup.testing.domain_reentrant_trigger_event_fake";
}
