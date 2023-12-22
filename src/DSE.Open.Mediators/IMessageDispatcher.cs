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
    /// Publishes the message to one or more registered <see cref="IMessageHandler{TMessage}"/>
    /// implementations where TMessage is the type of <paramref name="message"/>.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken">A token that can be used to request cancellation.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">No handlers are registered for the type of the message.</exception>
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    ValueTask PublishAsync(IMessage message, CancellationToken cancellationToken = default);
}
