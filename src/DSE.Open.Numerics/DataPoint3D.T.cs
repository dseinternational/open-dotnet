// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// An immutable 3-D point with X, Y, and Z coordinates of type <typeparamref name="T"/>.
/// JSON-serialised as <c>{ "x": …, "y": …, "z": … }</c>.
/// </summary>
/// <typeparam name="T">The numeric type used for all three coordinates.</typeparam>
public readonly record struct DataPoint3D<T> : IDataPoint<T>
    where T : struct, INumber<T>
{
    /// <summary>
    /// Initialises a new <see cref="DataPoint3D{T}"/> with the specified coordinates.
    /// </summary>
    [SetsRequiredMembers]
    public DataPoint3D(T x, T y, T z)
    {
        (X, Y, Z) = (x, y, z);
    }

    /// <summary>The X coordinate.</summary>
    [JsonPropertyName("x")]
    public required T X { get; init; }

    /// <summary>The Y coordinate.</summary>
    [JsonPropertyName("y")]
    public required T Y { get; init; }

    /// <summary>The Z (depth) coordinate.</summary>
    [JsonPropertyName("z")]
    public required T Z { get; init; }

    /// <summary>
    /// Deconstructs the data point into its three coordinates.
    /// </summary>
    public void Deconstruct(out T x, out T y, out T z)
    {
        (x, y, z) = (X, Y, Z);
    }
}
