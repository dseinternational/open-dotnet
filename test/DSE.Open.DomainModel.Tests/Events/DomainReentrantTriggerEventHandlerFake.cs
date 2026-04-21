// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Tests.Events;

public class DomainReentrantTriggerEventHandlerFake : IDomainEventMessageHandler<DomainReentrantTriggerEventFake>
{
    public const string FollowupKeyPrefix = "reentrant-followup:";

    private readonly TestState _state;

    public DomainReentrantTriggerEventHandlerFake(TestState state)
    {
        _state = state;
    }

    public ValueTask HandleAsync(DomainReentrantTriggerEventFake message, CancellationToken cancellationToken = default)
    {
        _ = _state.TryAdd(message.Instance.ToString(), message);

        // Raise a further event on the entity *during* dispatch. Without the
        // re-entrant loop in DomainEventDispatcher.PublishEventsAsync this event
        // would be silently dropped.
        var followup = message.Data.AddFakeEvent();
        _ = _state.TryAdd(FollowupKeyPrefix + followup.Instance, followup);

        return ValueTask.CompletedTask;
    }
}
