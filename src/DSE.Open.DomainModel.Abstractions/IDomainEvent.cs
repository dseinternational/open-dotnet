// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Events.Abstractions;
using DSE.Open.Mediators;

namespace DSE.Open.DomainModel.Abstractions;

/// <summary>
/// An object that describes something that happened in the domain that other parts of
/// the same domain (in-process) may choose to respond to.
/// </summary>
/// <remarks>
/// By default, <see cref="IDomainEvent"/> instances are sent <strong>after</strong> any
/// changes in the current unit of work are committed (i.e. an EntitySavedEvent). To signal
/// an imminent change - to be sent <strong>before</strong> any changes in the current
/// unit of work are committed - implement <see cref="IBeforeSaveChangesDomainEvent"/>.
/// </remarks>
public interface IDomainEvent : IEvent, IMessage
{
}

/// <summary>
/// An object that describes something that happened in the domain that other parts of
/// the same domain (in-process) may choose to respond to.
/// </summary>
/// <remarks>
/// By default, <see cref="IDomainEvent{TData}"/> instances are sent <strong>after</strong> any
/// changes in the current unit of work are committed (i.e. an EntitySavedEvent). To signal
/// an imminent change - to be sent <strong>before</strong> any changes in the current
/// unit of work are committed - implement <see cref="IBeforeSaveChangesDomainEvent"/>.
/// </remarks>
public interface IDomainEvent<TData> : IDomainEvent, IEvent<TData>
{
}
