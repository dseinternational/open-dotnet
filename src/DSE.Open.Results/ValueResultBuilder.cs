// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

public abstract class ValueResultBuilder<TResult, TValue> : ResultBuilder<TResult>
    where TResult : ValueResult<TValue>
{
    public virtual TValue? Value { get; set; }

    public virtual void MergeNotificationsAndValue(TResult valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);
        MergeNotifications(valueResult);
        Value = valueResult.Value;
    }
}

public class ValueResultBuilder<TValue> : ValueResultBuilder<ValueResult<TValue>, TValue>
{
    public override ValueResult<TValue> Build()
    {
        return new()
        {
            Status = Status,
            Value = Value,
            Notifications = [.. Notifications],
        };
    }
}
