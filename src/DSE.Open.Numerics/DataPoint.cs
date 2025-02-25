// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

public readonly record struct DataPoint<TX, TY>
    : IDataPoint<TX, TY>,
      IDataPointDouble
    where TX : struct, INumber<TX>
    where TY : struct, INumber<TY>
{
    [SetsRequiredMembers]
    public DataPoint(TX x, TY y)
    {
        (X, Y) = (x, y);
    }

    [JsonPropertyName("x")]
    public required TX X { get; init; }

    [JsonPropertyName("y")]
    public required TY Y { get; init; }

    double IDataPointDouble.X => double.CreateTruncating(X);

    double IDataPointDouble.Y => double.CreateTruncating(Y);

    public void Deconstruct(out TX x, out TY y)
    {
        (x, y) = (X, Y);
    }
}
