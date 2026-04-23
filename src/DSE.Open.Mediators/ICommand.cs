// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Mediators;

/// <summary>
/// Marks a type as representing a command that can be dispatched via an
/// <see cref="ICommandDispatcher"/> to exactly one registered
/// <see cref="ICommandHandler{TCommand, TCommandResult}"/>.
/// </summary>
/// <remarks>
/// Commands model a single intent to change state and must have exactly one handler.
/// Use <see cref="IMessage"/> for fire-and-forget notifications that may have zero
/// or more handlers.
/// </remarks>
#pragma warning disable CA1040 // Avoid empty interfaces
public interface ICommand
#pragma warning restore CA1040 // Avoid empty interfaces
{
}
