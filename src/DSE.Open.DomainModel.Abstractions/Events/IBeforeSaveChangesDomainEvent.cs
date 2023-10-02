// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Events;

/// <summary>
///     Marker interface to identify a <see cref="IDomainEvent"/> that is raised
///     <strong>before</strong> any changes in the current unit of work are
///     committed - for example, an EntitySavingEvent.
/// </summary>
public interface IBeforeSaveChangesDomainEvent : IDomainEvent
{
}
