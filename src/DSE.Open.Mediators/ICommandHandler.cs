// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Mediators;

/// <summary>
/// Handles a command of type <typeparamref name="TCommand"/> and produces a
/// <typeparamref name="TCommandResult"/>. Exactly one implementation must be
/// registered per command type.
/// </summary>
/// <typeparam name="TCommand">The command type handled by this handler.</typeparam>
/// <typeparam name="TCommandResult">The result type produced by this handler.</typeparam>
public interface ICommandHandler<in TCommand, TCommandResult>
    where TCommand : ICommand
{
    /// <summary>
    /// Handles <paramref name="command"/> and returns the produced result.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellation">A token that can be used to request cancellation.</param>
    Task<TCommandResult> HandleAsync(TCommand command, CancellationToken cancellation = default);
}
