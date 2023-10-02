// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Notifications;
using DSE.Open.Sessions;

namespace DSE.Open.Results;

public abstract class ResultBuilder<TResult>
    where TResult : Result
{
    public ICollection<Notification> Notifications { get; } = new List<Notification>();

    public SessionContext? Session
    {
        get
        {
            if (Sessions.TryGetValue("default", out var session))
            {
                return session;
            }

            return null;
        }
        set
        {
            if (value is not null)
            {
                Sessions.AddOrSet("default", value);
            }
            else
            {
                Sessions.Remove("default");
            }
        }
    }

    public Dictionary<string, SessionContext> Sessions { get; } = new();

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
        Sessions = Sessions,
    };
}
