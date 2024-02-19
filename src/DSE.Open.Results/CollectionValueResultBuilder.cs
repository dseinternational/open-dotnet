// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

public abstract class CollectionValueResultBuilder<TResult, TValue>
    : ValueResultBuilder<TResult, IEnumerable<TValue>>
    where TResult : CollectionValueResult<TValue>
{
    private List<TValue> _items = [];

    public ICollection<TValue> Items => _items;

    public override IEnumerable<TValue>? Value
    {
        get => _items;
        set => base.Value = _items = [..value];
    }

    public override void MergeNotificationsAndValue(TResult valueResult)
    {
        Guard.IsNotNull(valueResult);

        MergeNotifications(valueResult);

        _items.AddRange(valueResult.Value);
    }
}

public class CollectionValueResultBuilder<TValue>
    : CollectionValueResultBuilder<CollectionValueResult<TValue>, TValue>
{
    public override CollectionValueResult<TValue> GetResult()
    {
        return new CollectionValueResult<TValue>
        {
            Value = [.. Items],
            Notifications = [.. Notifications],
        };
    }
}
