// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Results;

/// <summary>
/// A <see cref="ValueResult{T}"/> that includes <see cref="Pagination"/> information.
/// </summary>
/// <typeparam name="T">The type of the carried value.</typeparam>
public record PaginatedValueResult<T> : ValueResult<T>
{
    /// <summary>
    /// Gets the pagination information for this result.
    /// </summary>
    [JsonPropertyName("pagination")]
    public Pagination Pagination { get; init; }
}
