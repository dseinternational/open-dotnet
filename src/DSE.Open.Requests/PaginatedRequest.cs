// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Requests;

public record PaginatedRequest : Request
{
    [JsonPropertyName("pagination")]
    [JsonPropertyOrder(-899800)]
    public PaginationOptions Pagination { get; init; }
}
