// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Mediators;

/// <summary>
/// Handles messages of type <typeparamref name="TMessage"/>. Zero, one, or many
/// implementations may be registered per message type; each is invoked in turn
/// when a matching message is published via <see cref="IMessageDispatcher"/>.
/// </summary>
/// <typeparam name="TMessage">The message type handled by this handler.</typeparam>
public interface IMessageHandler<in TMessage>
    where TMessage : IMessage
{
    /// <summary>
    /// Handles <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The message to handle.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation.</param>
    ValueTask HandleAsync(TMessage message, CancellationToken cancellationToken = default);
}
