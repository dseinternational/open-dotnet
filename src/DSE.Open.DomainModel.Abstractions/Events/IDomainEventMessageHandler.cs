// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Mediators;

namespace DSE.Open.DomainModel.Abstractions.Events;

public interface IDomainEventMessageHandler<TEvent> : IMessageHandler<TEvent>
    where TEvent : IDomainEvent
{
}
