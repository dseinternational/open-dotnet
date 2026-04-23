// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Mediators;

namespace DSE.Open.DomainModel.Events;

/// <summary>
/// A <see cref="IMessageHandler{TMessage}"/> that handles a specific kind of
/// <see cref="IDomainEvent"/>.
/// </summary>
/// <typeparam name="TEvent">The concrete domain-event type handled.</typeparam>
public interface IDomainEventMessageHandler<TEvent> : IMessageHandler<TEvent>
    where TEvent : IDomainEvent;
