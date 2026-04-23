// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A 2-D data point augmented with <see cref="High"/> and <see cref="Low"/>
/// values describing a range around it — for example, an OHLC candle, an
/// error bar, or a confidence interval. JSON-serialised as
/// <c>{ "x": …, "y": …, "h": …, "l": … }</c>.
/// </summary>
/// <typeparam name="T">The numeric type used for all four values.</typeparam>
public readonly record struct HighLowDataPoint<T> : IHighLowDataPoint<T>
    where T : struct, INumber<T>
{
    /// <summary>
    /// Initialises a new <see cref="HighLowDataPoint{T}"/>.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="high">The upper bound of the range at <paramref name="x"/>.</param>
    /// <param name="low">The lower bound of the range at <paramref name="x"/>.</param>
    [SetsRequiredMembers]
    public HighLowDataPoint(T x, T y, T high, T low)
    {
        (X, Y, High, Low) = (x, y, high, low);
    }

    /// <summary>The X (horizontal) coordinate.</summary>
    [JsonPropertyName("x")]
    public required T X { get; init; }

    /// <summary>The Y (vertical) coordinate.</summary>
    [JsonPropertyName("y")]
    public required T Y { get; init; }

    /// <summary>The upper bound of the range at <see cref="X"/>.</summary>
    [JsonPropertyName("h")]
    public T High { get; }

    /// <summary>The lower bound of the range at <see cref="X"/>.</summary>
    [JsonPropertyName("l")]
    public T Low { get; }

    /// <summary>
    /// Deconstructs the data point into its four component values.
    /// </summary>
    public void Deconstruct(out T x, out T y, out T high, out T low)
    {
        (x, y, high, low) = (X, Y, High, Low);
    }
}
