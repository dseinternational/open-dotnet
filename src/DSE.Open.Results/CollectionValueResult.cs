// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Results;

/// <summary>
/// A <see cref="ValueResult{T}"/> that carries a <see cref="ReadOnlyValueCollection{T}"/>.
/// </summary>
/// <typeparam name="T">The element type of the carried collection.</typeparam>
public record CollectionValueResult<T> : ValueResult<ReadOnlyValueCollection<T>>
{
    /// <summary>
    /// An empty <see cref="CollectionValueResult{T}"/> with no items.
    /// </summary>
    public static new readonly CollectionValueResult<T> Empty = new() { Value = [] };

    /// <summary>
    /// Gets the collection of items carried by this result. Never returns <see langword="null"/>.
    /// </summary>
    [JsonPropertyName("value")]
    public override required ReadOnlyValueCollection<T> Value
    {
        get => base.Value ?? [];
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        init => base.Value = value;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    }
}
