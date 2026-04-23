// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Requests.Tests;

public class PaginatedRequestTests
{
    [Fact]
    public void DefaultPagination_FallsBackToDefaultPageSizeAndNumber()
    {
        var request = new PaginatedRequest();

        Assert.Equal(PaginationOptions.DefaultPageSize, request.Pagination.PageSize);
        Assert.Equal(PaginationOptions.DefaultPageNumber, request.Pagination.PageNumber);
    }

    [Fact]
    public void ExplicitPagination_IsPreserved()
    {
        var request = new PaginatedRequest
        {
            Pagination = new PaginationOptions(pageSize: 25, pageNumber: 3),
        };

        Assert.Equal(25, request.Pagination.PageSize);
        Assert.Equal(3, request.Pagination.PageNumber);
    }

    [Fact]
    public void SerializeDeserialize_RoundtripsPagination()
    {
        var request = new PaginatedRequest
        {
            Pagination = new PaginationOptions(pageSize: 10, pageNumber: 4),
        };

        var json = JsonSerializer.Serialize(request);
        var deserialized = JsonSerializer.Deserialize<PaginatedRequest>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(10, deserialized.Pagination.PageSize);
        Assert.Equal(4, deserialized.Pagination.PageNumber);
    }

    [Fact]
    public void SerializeDeserialize_PreservesRequestId()
    {
        var request = new PaginatedRequest
        {
            RequestId = new RequestId("req_paged"),
            Pagination = new PaginationOptions(pageSize: 5),
        };

        var json = JsonSerializer.Serialize(request);
        var deserialized = JsonSerializer.Deserialize<PaginatedRequest>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(request.RequestId, deserialized.RequestId);
    }
}
