// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Notifications;

namespace DSE.Open.Results;

/// <summary>
/// Base class for building <see cref="PaginatedCollectionValueAsyncResult{TValue}"/> instances.
/// </summary>
/// <typeparam name="TResult">The type of result produced.</typeparam>
/// <typeparam name="TValue">The element type of the async enumerable value.</typeparam>
public abstract class PaginatedCollectionValueAsyncResultBuilder<TResult, TValue> : CollectionValueAsyncResultBuilder<TResult, TValue>
    where TResult : PaginatedCollectionValueAsyncResult<TValue>
{
    /// <summary>
    /// Gets or sets the pagination information that will be assigned to the built result.
    /// </summary>
    public Pagination Pagination { get; set; }

    /// <inheritdoc />
    public override void MergeNotificationsAndValue(TResult valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);

        base.MergeNotificationsAndValue(valueResult);
        Pagination = valueResult.Pagination;
    }
}

/// <summary>
/// Builds a <see cref="PaginatedCollectionValueAsyncResult{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The element type of the async enumerable value.</typeparam>
public class PaginatedCollectionValueAsyncResultBuilder<TValue> : PaginatedCollectionValueAsyncResultBuilder<PaginatedCollectionValueAsyncResult<TValue>, TValue>
{
    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Thrown when <see cref="PaginatedCollectionValueAsyncResultBuilder{TResult, TValue}.Pagination"/>
    /// is <see cref="Pagination.None"/> and there are no error notifications.
    /// </exception>
    public override PaginatedCollectionValueAsyncResult<TValue> Build()
    {
        // Pagination can be `None` if there are errors (e.g., if the provided command was invalid).
        if (Pagination == Pagination.None && !Notifications.AnyErrors())
        {
            throw new InvalidOperationException("Pagination must be specified.");
        }

        return new()
        {
            Status = Status,
            Value = Value ?? AsyncEnumerable.Empty<TValue>(),
            Notifications = [.. Notifications],
            Pagination = Pagination,
        };
    }
}
