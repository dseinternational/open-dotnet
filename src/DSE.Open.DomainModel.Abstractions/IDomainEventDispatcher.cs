// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.DomainModel.Abstractions;

/// <summary>
/// Dispatches domain events attached to entities.
/// </summary>
/// <remarks>Event dispatchers should be scoped to the current unit of work.</remarks>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Publishes any pending events that implement <see cref="IBeforeSaveChangesDomainEvent"/>.
    /// Events are published consecutively.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    Task PublishBeforeSaveChangesEventsAsync(IEnumerable<IEventRaisingEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes any pending domain events. Events are dispatched to handlers consecutively, unless
    /// they implement <see cref="IBackgroundDomainEvent"/>. Events implementing
    /// <see cref="IBackgroundDomainEvent"/> are dispatched in parallel and may be running while
    /// other, consecutive event handlers are running.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    Task PublishEventsAsync(IEnumerable<IEventRaisingEntity> entities, CancellationToken cancellationToken = default);
}
