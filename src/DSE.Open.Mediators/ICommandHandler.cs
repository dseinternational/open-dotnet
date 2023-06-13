// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Mediators;

public interface ICommandHandler<in TCommand, TCommandResult>
    where TCommand : ICommand
{
    Task<TCommandResult> HandleAsync(TCommand command, CancellationToken cancellation = default);
}
