// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Notifications;

namespace DSE.Open.Results;

public abstract class ResultBuilder<TResult>
    where TResult : Result
{
    public ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual void MergeNotifications(Result result)
    {
        Guard.IsNotNull(result);

        if (result.HasNotifications)
        {
            Notifications.AddRange(result.Notifications);
        }
    }

    public abstract TResult GetResult();
}

public class ResultBuilder : ResultBuilder<Result>
{
    public override Result GetResult() => new()
    {
        Notifications = [.. Notifications],
    };
}
