// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// An immutable 2-D point with X and Y coordinates of type <typeparamref name="T"/>.
/// JSON-serialised as <c>{ "x": …, "y": … }</c>.
/// </summary>
/// <typeparam name="T">The numeric type used for both coordinates.</typeparam>
public readonly record struct DataPoint<T> : IDataPoint<T>
    where T : struct, INumber<T>
{
    /// <summary>
    /// Initialises a new <see cref="DataPoint{T}"/> with the specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    [SetsRequiredMembers]
    public DataPoint(T x, T y)
    {
        (X, Y) = (x, y);
    }

    /// <summary>The X (horizontal) coordinate.</summary>
    [JsonPropertyName("x")]
    public required T X { get; init; }

    /// <summary>The Y (vertical) coordinate.</summary>
    [JsonPropertyName("y")]
    public required T Y { get; init; }

    /// <summary>
    /// Deconstructs the data point into its two coordinates.
    /// </summary>
    /// <param name="x">Receives the X coordinate.</param>
    /// <param name="y">Receives the Y coordinate.</param>
    public void Deconstruct(out T x, out T y)
    {
        (x, y) = (X, Y);
    }
}
