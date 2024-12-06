// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Tests.Events;

public class DomainBackgroundEventHandlerFake : IDomainEventMessageHandler<DomainBackgroundEventFake>
{
    private readonly TestState _state;

    public DomainBackgroundEventHandlerFake(TestState state)
    {
        _state = state;
    }

    public ValueTask HandleAsync(DomainBackgroundEventFake message, CancellationToken cancellationToken = default)
    {
        _ = _state.TryAdd(message.Instance.ToString(), message);
        return ValueTask.CompletedTask;
    }
}
