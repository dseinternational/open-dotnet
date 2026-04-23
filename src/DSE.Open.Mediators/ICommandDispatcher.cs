// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Mediators;

/// <summary>
/// Dispatches commands to the single registered
/// <see cref="ICommandHandler{TCommand, TCommandResult}"/> for each command type.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Resolves the handler for <typeparamref name="TCommand"/> producing
    /// <typeparamref name="TCommandResult"/>, invokes it with <paramref name="command"/>,
    /// and returns its result.
    /// </summary>
    /// <typeparam name="TCommand">The command type; must implement <see cref="ICommand"/>.</typeparam>
    /// <typeparam name="TCommandResult">The result type produced by the handler.</typeparam>
    /// <param name="command">The command instance to dispatch.</param>
    /// <param name="cancellation">A token that can be used to request cancellation.</param>
    /// <returns>The value produced by the handler.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="command"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Zero or more than one handler is registered for <typeparamref name="TCommand"/>,
    /// or the handler threw a non-cancellation exception.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The handler reported cancellation.
    /// </exception>
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation = default)
        where TCommand : ICommand;
}
