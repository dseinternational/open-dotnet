// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Tests.Events;

public class DomainEventFake : DomainEvent<string>
{
    public DomainEventFake(string data) : base(data)
    {
    }

    public Guid Instance { get; } = Guid.NewGuid();

    public override string Type => "org.dsegroup.testing.domain_event_fake";
}
