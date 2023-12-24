// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.DomainModel.Events;

/// <summary>
///     Marker interface to identify a <see cref="IDomainEvent"/> that can be
///     be passed to handlers in parallel with other events implementing
///     <see cref="IBackgroundDomainEvent"/>.
/// </summary>
/// <remarks>
///     <para>
///         Multiple handlers may be processing multiple events implementing
///         <see cref="IBackgroundDomainEvent"/> simultaneously. Handlers of these
///         events must therefore be careful to avoid interacting with the
///         DbContext in scope (which is not thread-safe). Handlers should also
///         note that entities are not routinely thread-safe so mutating the state
///         of entities should be avoided unless explicitly supported.
///     </para>
///     <para>
///         Background domain event handlers are called after changes in the current
///         unit of work have been persisted and may be called while other (synchronous)
///         event handlers are executing. A typical use case is sending event messages
///         to external systems where data is being copied and can be sent via remote
///         API calls in parallel.
///     </para>
/// </remarks>
public interface IBackgroundDomainEvent : IDomainEvent;
