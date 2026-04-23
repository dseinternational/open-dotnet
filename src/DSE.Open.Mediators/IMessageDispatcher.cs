// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Mediators;

/// <summary>
/// Sends messages to registered <see cref="IMessageHandler{TMessage}"/> implementations.
/// </summary>
public interface IMessageDispatcher
{
    /// <summary>
    /// Publishes <paramref name="message"/> to every registered
    /// <see cref="IMessageHandler{TMessage}"/> whose <c>TMessage</c> matches the
    /// runtime type of the message. Handlers run sequentially in registration order.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation.</param>
    /// <remarks>
    /// Publishing to a message type with no registered handlers is a no-op and logs a
    /// warning; it does not throw.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// A handler's <c>HandleAsync</c> returned a value that is not a <see cref="ValueTask"/>,
    /// or the handler threw a non-cancellation exception.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// A handler reported cancellation, or <paramref name="cancellationToken"/> was already cancelled.
    /// </exception>
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    ValueTask PublishAsync(IMessage message, CancellationToken cancellationToken = default);
}
