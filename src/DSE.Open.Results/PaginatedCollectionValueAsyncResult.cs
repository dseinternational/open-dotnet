// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Results;

public record PaginatedCollectionValueAsyncResult<T> : CollectionValueAsyncResult<T>
{
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; init; }
}
