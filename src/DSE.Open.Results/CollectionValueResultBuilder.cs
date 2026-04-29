// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;

namespace DSE.Open.Results;

/// <summary>
/// Base class for building <see cref="CollectionValueResult{TValue}"/> instances.
/// </summary>
/// <typeparam name="TResult">The type of result produced.</typeparam>
/// <typeparam name="TValue">The element type of the carried collection.</typeparam>
public abstract class CollectionValueResultBuilder<TResult, TValue>
    : ValueResultBuilder<TResult, ReadOnlyValueCollection<TValue>>
    where TResult : CollectionValueResult<TValue>
{
    private Collection<TValue> _items = [];

    /// <summary>
    /// Gets the collection of items being accumulated for the built result.
    /// </summary>
    public ICollection<TValue> Items => _items;

    /// <summary>
    /// Gets or sets the value of the result as a snapshot of <see cref="Items"/>. Setting this
    /// replaces the contents of <see cref="Items"/>.
    /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
    public override ReadOnlyValueCollection<TValue>? Value
#pragma warning restore CA2227 // Collection properties should be read only
    {
        get => [.. _items];
        set => base.Value = [.. _items = [.. value ?? []]];
    }

    /// <inheritdoc />
    public override void MergeNotificationsAndValue(TResult valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);

        MergeNotifications(valueResult);

        _items.AddRange(valueResult.Value);
    }
}

/// <summary>
/// Builds a <see cref="CollectionValueResult{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The element type of the carried collection.</typeparam>
public class CollectionValueResultBuilder<TValue>
    : CollectionValueResultBuilder<CollectionValueResult<TValue>, TValue>
{
    /// <inheritdoc />
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
