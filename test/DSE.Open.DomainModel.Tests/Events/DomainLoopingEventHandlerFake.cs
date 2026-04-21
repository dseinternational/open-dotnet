// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Tests.Events;

/// <summary>
/// Handler that always raises a further <see cref="DomainLoopingEventFake"/> on the
/// entity it is handling — used to exercise the iteration cap in
/// <c>DomainEventDispatcher.PublishEventsAsync</c>.
/// </summary>
public class DomainLoopingEventHandlerFake : IDomainEventMessageHandler<DomainLoopingEventFake>
{
    public ValueTask HandleAsync(DomainLoopingEventFake message, CancellationToken cancellationToken = default)
    {
        message.Data.AddDomainEvent(new DomainLoopingEventFake(message.Data));
        return ValueTask.CompletedTask;
    }
}
