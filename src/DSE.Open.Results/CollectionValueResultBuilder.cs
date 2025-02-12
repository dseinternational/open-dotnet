// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;

namespace DSE.Open.Results;

public abstract class CollectionValueResultBuilder<TResult, TValue>
    : ValueResultBuilder<TResult, ReadOnlyValueCollection<TValue>>
    where TResult : CollectionValueResult<TValue>
{
    private Collection<TValue> _items = [];

    public ICollection<TValue> Items => _items;

#pragma warning disable CA2227 // Collection properties should be read only
    public override ReadOnlyValueCollection<TValue>? Value
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get => [.. _items];
        set => base.Value = [.. _items = [.. value ?? []]];
    }

    public override void MergeNotificationsAndValue(TResult valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);

        MergeNotifications(valueResult);

        _items.AddRange(valueResult.Value);
    }
}

public class CollectionValueResultBuilder<TValue>
    : CollectionValueResultBuilder<CollectionValueResult<TValue>, TValue>
{
    public override CollectionValueResult<TValue> Build()
    {
        return new()
        {
            Status = Status,
            Value = [.. Items],
            Notifications = [.. Notifications],
        };
    }
}
