// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DSE.Open.Mediators;

public sealed partial class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandDispatcher> _logger;

    public CommandDispatcher(IServiceProvider serviceProvider, ILogger<CommandDispatcher> logger)
    {
        Guard.IsNotNull(serviceProvider);
        Guard.IsNotNull(logger);

        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    public async Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation = default)
        where TCommand : ICommand
    {
        Guard.IsNotNull(command);

        var commandType = command.GetType();

        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TCommandResult));

        var handlers = _serviceProvider.GetServices(handlerType).ToArray();

        if (handlers.Length == 0)
        {
            ThrowHelper.ThrowInvalidOperationException($"No handler is registered for commands of type '{commandType.Name}'.");
            return default; // unreachable
        }

        if (handlers.Length > 1)
        {
            ThrowHelper.ThrowInvalidOperationException($"More than one handler is registered for commands of type '{commandType.Name}'.");
            return default; // unreachable
        }

        var handler = handlers[0];

        if (handler is null)
        {
            ThrowHelper.ThrowInvalidOperationException($"A null handler is registered for commands of type '{commandType.Name}'.");
            return default; // unreachable
        }

        var commandResult = await Task.Run(() =>
        {
            Task<TCommandResult> task;

            try
            {
                var result = handlerType.InvokeMember(
                    nameof(ICommandHandler<ICommand, object>.HandleAsync),
                    BindingFlags.InvokeMethod,
                    null, handler, new object[] { command, cancellation }, null);

                if (result is null)
                {
                    ThrowHelper.ThrowInvalidOperationException("Handler is expected to return Task<TCommandResult>.");
                }

                task = (Task<TCommandResult>)result;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error invoking {handler.GetType().Name} handler for {commandType.Name} command.", e);
            }

            return task;

        }, cancellation).ConfigureAwait(false);

        Log.SentCommandToHandler(_logger, handler.GetType().Name);

        return commandResult;
    }

    private static partial class Log
    {
        [LoggerMessage(
            EventId = 5050601,
            Level = LogLevel.Debug,
            Message = "Sent command to '{type}' handler")]
        public static partial void SentCommandToHandler(ILogger logger, string type);
    }
}
