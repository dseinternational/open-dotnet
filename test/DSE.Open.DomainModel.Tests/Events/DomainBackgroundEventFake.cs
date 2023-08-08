// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Tests.Events;

public class DomainBackgroundEventFake : DomainEvent<string>, IBackgroundDomainEvent
{
    public DomainBackgroundEventFake(string data) : base(data)
    {
    }

    public Guid Instance { get; } = Guid.NewGuid();

    public override string Type => "org.dsegroup.testing.domain_background_event_fake";
}
