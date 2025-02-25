// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

public readonly record struct HighLowDataPoint<TX, TY> : IHighLowDataPoint<TX, TY>
    where TX : struct, INumber<TX>
    where TY : struct, INumber<TY>
{
    [SetsRequiredMembers]
    public HighLowDataPoint(TX x, TY y, TY high, TY low)
    {
        (X, Y, High, Low) = (x, y, high, low);
    }

    [JsonPropertyName("x")]
    public required TX X { get; init; }

    [JsonPropertyName("y")]
    public required TY Y { get; init; }

    [JsonPropertyName("h")]
    public TY High { get; }

    [JsonPropertyName("l")]
    public TY Low { get; }

    public void Deconstruct(out TX x, out TY y, out TY high, out TY low)
    {
        (x, y, high, low) = (X, Y, High, Low);
    }
}
