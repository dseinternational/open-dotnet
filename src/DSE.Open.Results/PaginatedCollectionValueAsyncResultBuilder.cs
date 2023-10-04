// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

public abstract class PaginatedCollectionValueAsyncResultBuilder<TResult, TValue> : CollectionValueAsyncResultBuilder<TResult, TValue>
    where TResult : PaginatedCollectionValueAsyncResult<TValue>
{
    public Pagination Pagination { get; set; }

    public new IAsyncEnumerable<TValue>? Value { get; set; }
}

public class PaginatedCollectionValueAsyncResultBuilder<TValue> : PaginatedCollectionValueAsyncResultBuilder<PaginatedCollectionValueAsyncResult<TValue>, TValue>
{
    public override PaginatedCollectionValueAsyncResult<TValue> GetResult()
    {
        return Pagination == Pagination.None
            ? throw new InvalidOperationException("Pagination must be specified.")
            : new PaginatedCollectionValueAsyncResult<TValue>
            {
                Value = Value ?? AsyncEnumerable.Empty<TValue>(),
                Notifications = [.. Notifications],
                Pagination = Pagination,
            };
    }
}
