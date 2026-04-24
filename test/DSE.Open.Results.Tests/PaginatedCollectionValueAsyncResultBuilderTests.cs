// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results.Tests;

public class PaginatedCollectionValueAsyncResultBuilderTests
{
    [Fact]
    public async Task BuildWithValue_ShouldPreserveAsyncValue()
    {
        var builder = new PaginatedCollectionValueAsyncResultBuilder<int>
        {
            Pagination = new(2, 2, 1)
        };

        var result = builder.BuildWithValue(ToAsyncEnumerable([4, 2]));

        Assert.NotNull(result.Value);
        Assert.Equal([4, 2], await ToListAsync(result.Value));
    }

    [Fact]
    public async Task MergeNotificationsAndValue_ShouldPreserveAsyncValueAndPagination()
    {
        var source = new PaginatedCollectionValueAsyncResult<int>
        {
            Value = ToAsyncEnumerable([4, 2]),
            Pagination = new(2, 2, 1)
        };
        var builder = new PaginatedCollectionValueAsyncResultBuilder<int>();

        builder.MergeNotificationsAndValue(source);
        var result = builder.Build();

        Assert.Equal(source.Pagination, result.Pagination);
        Assert.NotNull(result.Value);
        Assert.Equal([4, 2], await ToListAsync(result.Value));
    }

    private static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            yield return value;
            await Task.Yield();
        }
    }

    private static async Task<List<T>> ToListAsync<T>(IAsyncEnumerable<T> values)
    {
        var list = new List<T>();

        await foreach (var value in values)
        {
            list.Add(value);
        }

        return list;
    }
}
