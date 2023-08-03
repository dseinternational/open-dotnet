// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.DomainModel.Events;

namespace DSE.Open.DomainModel.Entities;

public interface IEventRaisingEntity : IEntity
{
    /// <summary>
    /// A collection of domain events created by the entity within the current unit of work.
    /// </summary>
    IEnumerable<IDomainEvent> Events { get; }

    /// <summary>
    /// Indicates if there are any <see cref="Events"/>.
    /// </summary>
    bool HasEvents { get; }

    void ClearEvents();

    void ClearBeforeSaveChangesEvents();
}

public interface IEventRaisingEntity<TId> : IEventRaisingEntity, IEntity<TId>
    where TId : struct, IEquatable<TId>
{
}
