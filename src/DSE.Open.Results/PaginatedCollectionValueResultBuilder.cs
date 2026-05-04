// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Notifications;

namespace DSE.Open.Results;

/// <summary>
/// Base class for building <see cref="PaginatedCollectionValueResult{TValue}"/> instances
/// (or derived result types).
/// </summary>
/// <typeparam name="TResult">The type of result produced.</typeparam>
/// <typeparam name="TValue">The element type of the carried collection.</typeparam>
public abstract class PaginatedCollectionValueResultBuilder<TResult, TValue>
    : CollectionValueResultBuilder<TResult, TValue>
    where TResult : PaginatedCollectionValueResult<TValue>
{
    /// <summary>
    /// Gets or sets the pagination information that will be assigned to the built result.
    /// </summary>
    public Pagination Pagination { get; set; }

    /// <summary>
    /// Merges notifications, items and pagination from <paramref name="valueResult"/>.
    /// </summary>
    public override void MergeNotificationsAndValue(TResult valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);
        base.MergeNotificationsAndValue(valueResult);
        Pagination = valueResult.Pagination;
    }

    /// <summary>
    /// Validates that <see cref="Pagination"/> is consistent with the accumulated
    /// <see cref="CollectionValueResultBuilder{TResult,TValue}.Items"/> count.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <see cref="Pagination"/> is invalid for the current items (or unspecified
    /// in the absence of error notifications).
    /// </exception>
    protected void ValidatePagination()
    {
        if (Notifications.AnyErrors())
        {
            // Pagination can be `None` if there are errors (e.g., if the provided command was invalid).
            return;
        }

        if (Pagination == Pagination.None)
        {
            throw new InvalidOperationException("Pagination must be specified.");
        }

        if (Pagination.TotalItems < Items.Count)
        {
            throw new InvalidOperationException("TotalItems must be equal to or greater than the count of items.");
        }

        if (Pagination.PageSize < Items.Count)
        {
            throw new InvalidOperationException("PageSize must be equal to or greater than the count of items.");
        }
    }
}

/// <summary>
/// Builds a <see cref="PaginatedCollectionValueResult{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The element type of the carried collection.</typeparam>
public class PaginatedCollectionValueResultBuilder<TValue>
    : PaginatedCollectionValueResultBuilder<PaginatedCollectionValueResult<TValue>, TValue>
{
    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Thrown when <see cref="PaginatedCollectionValueResultBuilder{TResult, TValue}.Pagination"/>
    /// is invalid for the current items (or unspecified in the absence of error notifications).
    /// </exception>
    public override PaginatedCollectionValueResult<TValue> Build()
    {
        ValidatePagination();

        return new()
        {
            Status = Status,
            Value = [.. Items],
            Notifications = [.. Notifications],
            Pagination = Pagination,
        };
    }
}
