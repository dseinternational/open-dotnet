// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Results;

/// <summary>
/// A <see cref="CollectionValueResult{T}"/> that includes <see cref="Pagination"/> information.
/// </summary>
/// <typeparam name="T">The element type of the carried collection.</typeparam>
public record PaginatedCollectionValueResult<T> : CollectionValueResult<T>
{
    /// <summary>
    /// Gets the pagination information for this result.
    /// </summary>
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; init; }
}
