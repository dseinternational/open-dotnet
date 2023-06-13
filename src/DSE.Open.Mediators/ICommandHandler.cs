// Copyright (c) Down Syndrome Education Enterprises CIC. All Rights Reserved.
// Information contained herein is PROPRIETARY AND CONFIDENTIAL.

namespace DSE.Open.Mediators;

public interface ICommandHandler<in TCommand, TCommandResult>
    where TCommand : ICommand
{
    Task<TCommandResult> HandleAsync(TCommand command, CancellationToken cancellation = default);
}
