// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Notifications;

namespace DSE.Open.Results;

public abstract class PaginatedCollectionValueAsyncResultBuilder<TResult, TValue> : CollectionValueAsyncResultBuilder<TResult, TValue>
    where TResult : PaginatedCollectionValueAsyncResult<TValue>
{
    public Pagination Pagination { get; set; }

    public override void MergeNotificationsAndValue(TResult valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);

        base.MergeNotificationsAndValue(valueResult);
        Pagination = valueResult.Pagination;
    }
}

public class PaginatedCollectionValueAsyncResultBuilder<TValue> : PaginatedCollectionValueAsyncResultBuilder<PaginatedCollectionValueAsyncResult<TValue>, TValue>
{
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
