// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Results;

public record CollectionValueResult<T> : ValueResult<IEnumerable<T>>
{
    public static new readonly CollectionValueResult<T> Empty = new() { Value = Array.Empty<T>() };

    [JsonPropertyName("value")]
    public override required IEnumerable<T> Value
    {
        get => base.Value ?? Array.Empty<T>();
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        init => base.Value = value;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    }
}
