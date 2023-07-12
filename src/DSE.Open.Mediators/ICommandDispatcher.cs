// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Mediators;

public interface ICommandDispatcher
{
    [RequiresDynamicCode("May break functionality when AOT compiling")]
    Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation = default)
        where TCommand : ICommand;
}
