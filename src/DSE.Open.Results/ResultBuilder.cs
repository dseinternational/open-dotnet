// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Notifications;

namespace DSE.Open.Results;

public abstract class ResultBuilder<TResult>
    where TResult : Result
{
    public virtual ResultStatus Status { get; set; }

    public ICollection<Notification> Notifications { get; } = [];

    public virtual void MergeNotifications(Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.HasNotifications)
        {
            Notifications.AddRange(result.Notifications);
        }
    }

    public abstract TResult Build();
}

public class ResultBuilder : ResultBuilder<Result>
{
    public override Result Build()
    {
        return new()
        {
            Status = Status,
            Notifications = [.. Notifications],
        };
    }
}
