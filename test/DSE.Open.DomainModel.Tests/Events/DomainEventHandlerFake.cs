// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Abstractions;

namespace DSE.Open.DomainModel.Tests.Events;

public class DomainEventHandlerFake : IDomainEventMessageHandler<DomainEventFake>
{
    private readonly TestState _state;

    public DomainEventHandlerFake(TestState state)
    {
        _state = state;
    }

    public ValueTask HandleAsync(DomainEventFake message, CancellationToken cancellationToken = default)
    {
        _state.Add(message.Instance.ToString(), message);
        return ValueTask.CompletedTask;
    }
}
