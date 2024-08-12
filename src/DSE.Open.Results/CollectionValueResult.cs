// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Results;

public record CollectionValueResult<T> : ValueResult<ReadOnlyValueCollection<T>>
{
    public static new readonly CollectionValueResult<T> Empty = new() { Value = [] };

    [JsonPropertyName("value")]
    public override required ReadOnlyValueCollection<T> Value
    {
        get => base.Value ?? [];
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        init => base.Value = value;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    }
}
