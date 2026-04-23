// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Requests;

/// <summary>
/// A <see cref="Request"/> that carries paging parameters for responses that
/// return a page of items.
/// </summary>
public record PaginatedRequest : Request
{
    /// <summary>
    /// The paging window to apply to the response. Defaults to the struct's
    /// default, in which case <see cref="PaginationOptions.PageSize"/> falls back
    /// to <see cref="PaginationOptions.DefaultPageSize"/> and
    /// <see cref="PaginationOptions.PageNumber"/> to
    /// <see cref="PaginationOptions.DefaultPageNumber"/>.
    /// </summary>
    [JsonPropertyName("pagination")]
    [JsonPropertyOrder(-899800)]
    public PaginationOptions Pagination { get; init; }
}
