// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

public readonly record struct DataPoint<T> : IDataPoint<T>
    where T : struct, INumber<T>
{
    [SetsRequiredMembers]
    public DataPoint(T x, T y)
    {
        (X, Y) = (x, y);
    }

    [JsonPropertyName("x")]
    public required T X { get; init; }

    [JsonPropertyName("y")]
    public required T Y { get; init; }

    public void Deconstruct(out T x, out T y)
    {
        (x, y) = (X, Y);
    }
}
