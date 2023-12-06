// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Mediators;

/// <summary>
/// Sends messages to message handlers provided by a <see cref="IServiceProvider"/>.
/// Message handlers must be registered in DI as implementing <see cref="IMessageHandler{TMessage}"/>.
/// </summary>
public sealed partial class MessageDispatcher : IMessageDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessageDispatcher> _logger;

    public MessageDispatcher(IServiceProvider serviceProvider, ILogger<MessageDispatcher> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    public async ValueTask PublishAsync(IMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        var messageType = message.GetType();

        var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);

        var handlers = _serviceProvider.GetServices(handlerType).ToArray();

        if (handlers.Length == 0)
        {
            Log.NoHandlersRegistered(_logger, messageType.Name);
            return;
        }

        Log.SendingMessageToHandlers(_logger, messageType.Name, handlers.Length);

        foreach (var handler in handlers)
        {
            if (handler is null)
            {
                continue;
            }

            _ = await Task.Run(() =>
            {
                ValueTask task;

                try
                {
                    var result = handlerType.InvokeMember(
                        nameof(IMessageHandler<IMessage>.HandleAsync),
                        BindingFlags.InvokeMethod,
                        null, handler, new object[] { message, cancellationToken }, null);

                    if (result is not ValueTask resultTask)
                    {
                        ThrowHelper.ThrowInvalidOperationException("Handler is expected to return ValueTask.");
                        return default; // unreachable
                    }

                    task = resultTask;
                }
                catch (Exception e) when (e is not (StackOverflowException or OutOfMemoryException or OperationCanceledException))
                {
                    ThrowHelper.ThrowInvalidOperationException(
                        $"Error invoking {handler.GetType().Name} handler for {messageType.Name} message.", e);
                    return default; // unreachable
                }

                return task;

            }, cancellationToken).ConfigureAwait(false);

            Log.SentMessageToHandler(_logger, handler.GetType().Name);
        }
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 5050501,
            Level = LogLevel.Debug,
            Message = "Sending '{typeName}' message to {count} handler(s)")]
        public static partial void SendingMessageToHandlers(ILogger logger, string typeName, int count);

        [LoggerMessage(
            EventId = 5050502,
            Level = LogLevel.Debug,
            Message = "Sent message to '{type}' handler")]
        public static partial void SentMessageToHandler(ILogger logger, string type);

        [LoggerMessage(
            EventId = 5050503,
            Level = LogLevel.Warning,
            Message = "No handlers are registered for messages of type '{type}'")]
        public static partial void NoHandlersRegistered(ILogger logger, string type);
    }
}
