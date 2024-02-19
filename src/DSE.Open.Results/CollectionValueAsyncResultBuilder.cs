// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

public abstract class CollectionValueAsyncResultBuilder<TResult, TValue>
    : ValueResultBuilder<TResult, IAsyncEnumerable<TValue>>
    where TResult : CollectionValueAsyncResult<TValue>
{
    public new IAsyncEnumerable<TValue>? Value
    {
        get => base.Value ??= AsyncEnumerable.Empty<TValue>();
        set => base.Value = value ?? AsyncEnumerable.Empty<TValue>();
    }

    public override void MergeNotificationsAndValue(TResult valueResult)
    {
        Guard.IsNotNull(valueResult);

        MergeNotifications(valueResult);
        Value = valueResult.Value ?? AsyncEnumerable.Empty<TValue>();
    }
}

public class CollectionValueAsyncResultBuilder<TValue>
    : CollectionValueAsyncResultBuilder<CollectionValueAsyncResult<TValue>, TValue>
{
    public override CollectionValueAsyncResult<TValue> GetResult()
    {
        return new CollectionValueAsyncResult<TValue>
        {
            Value = Value ?? AsyncEnumerable.Empty<TValue>(),
            Notifications = [.. Notifications],
        };
    }
}
