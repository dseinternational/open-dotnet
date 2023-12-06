// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;

namespace DSE.Open.Results;

public class PaginatedCollectionValueResultBuilder<TValue> : CollectionValueResultBuilder<TValue>
{
    public Pagination Pagination { get; set; }

    public virtual void MergeNotificationsAndValue(PaginatedCollectionValueResult<TValue> valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);

        MergeNotifications(valueResult);

        Items.AddRange(valueResult.Value);
    }

    public override PaginatedCollectionValueResult<TValue> GetResult()
    {
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

        return new PaginatedCollectionValueResult<TValue>
        {
            Value = [.. Items],
            Notifications = [.. Notifications],
            Pagination = Pagination,
        };
    }
}
