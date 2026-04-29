// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

/// <summary>
/// Base class for building <see cref="CollectionValueAsyncResult{TValue}"/> instances.
/// </summary>
/// <typeparam name="TResult">The type of result produced.</typeparam>
/// <typeparam name="TValue">The element type of the async enumerable value.</typeparam>
public abstract class CollectionValueAsyncResultBuilder<TResult, TValue>
    : ValueResultBuilder<TResult, IAsyncEnumerable<TValue>>
    where TResult : CollectionValueAsyncResult<TValue>
{
    /// <summary>
    /// Gets or sets the async enumerable value. Returns an empty sequence if not yet assigned;
    /// assigning <see langword="null"/> stores an empty sequence.
    /// </summary>
    public override IAsyncEnumerable<TValue>? Value
    {
        get => base.Value ??= AsyncEnumerable.Empty<TValue>();
        set => base.Value = value ?? AsyncEnumerable.Empty<TValue>();
    }

    /// <inheritdoc />
    public override void MergeNotificationsAndValue(TResult valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);

        MergeNotifications(valueResult);
        Value = valueResult.Value ?? AsyncEnumerable.Empty<TValue>();
    }
}

/// <summary>
/// Builds a <see cref="CollectionValueAsyncResult{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The element type of the async enumerable value.</typeparam>
public class CollectionValueAsyncResultBuilder<TValue>
    : CollectionValueAsyncResultBuilder<CollectionValueAsyncResult<TValue>, TValue>
{
    /// <inheritdoc />
    public override CollectionValueAsyncResult<TValue> Build()
    {
        return new()
        {
            Status = Status,
            Value = Value ?? AsyncEnumerable.Empty<TValue>(),
            Notifications = [.. Notifications],
        };
    }
}
