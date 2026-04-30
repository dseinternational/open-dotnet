// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;
using DSE.Open.Notifications;

namespace DSE.Open.Results;

/// <summary>
/// Builds a <see cref="PaginatedCollectionValueResult{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The element type of the carried collection.</typeparam>
public class PaginatedCollectionValueResultBuilder<TValue> : CollectionValueResultBuilder<TValue>
{
    /// <summary>
    /// Gets or sets the pagination information that will be assigned to the built result.
    /// </summary>
    public Pagination Pagination { get; set; }

    /// <summary>
    /// Merges notifications, items and pagination from <paramref name="valueResult"/>.
    /// </summary>
    public virtual void MergeNotificationsAndValue(PaginatedCollectionValueResult<TValue> valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);

        MergeNotifications(valueResult);

        Items.AddRange(valueResult.Value);

        Pagination = valueResult.Pagination;
    }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Thrown when <see cref="Pagination"/> is invalid for the current items (or unspecified
    /// in the absence of error notifications).
    /// </exception>
    public override PaginatedCollectionValueResult<TValue> Build()
    {
        ValidatePagination();
        return Done();
    }

    private void ValidatePagination()
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

    private PaginatedCollectionValueResult<TValue> Done()
    {
        return new()
        {
            Status = Status,
            Value = [.. Items],
            Notifications = [.. Notifications],
            Pagination = Pagination,
        };
    }
}
