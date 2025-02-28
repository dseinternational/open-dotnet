// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

public readonly record struct HighLowDataPoint<T> : IHighLowDataPoint<T>
    where T : struct, INumber<T>
{
    [SetsRequiredMembers]
    public HighLowDataPoint(T x, T y, T high, T low)
    {
        (X, Y, High, Low) = (x, y, high, low);
    }

    [JsonPropertyName("x")]
    public required T X { get; init; }

    [JsonPropertyName("y")]
    public required T Y { get; init; }

    [JsonPropertyName("h")]
    public T High { get; }

    [JsonPropertyName("l")]
    public T Low { get; }

    public void Deconstruct(out T x, out T y, out T high, out T low)
    {
        (x, y, high, low) = (X, Y, High, Low);
    }
}
