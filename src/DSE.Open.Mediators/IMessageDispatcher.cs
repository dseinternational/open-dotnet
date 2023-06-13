// Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
// Information contained herein is PROPRIETARY AND CONFIDENTIAL.

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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">No handlers are registered for the type of the message.</exception>
    Task PublishAsync(IMessage message, CancellationToken cancellationToken = default);
}
