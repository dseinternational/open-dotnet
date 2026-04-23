// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// A point in two-dimensional space, parameterised by the numeric type
/// of its coordinates.
/// </summary>
/// <typeparam name="T">The numeric type used for both coordinates.</typeparam>
public interface IDataPoint<T>
    where T : INumber<T>
{
    /// <summary>The X (horizontal) coordinate.</summary>
    T X { get; }

    /// <summary>The Y (vertical) coordinate.</summary>
    T Y { get; }

    /// <summary>
    /// Deconstructs the data point into its two coordinates. Convenient for
    /// pattern matching and tuple destructuring (<c>var (x, y) = point</c>).
    /// </summary>
    /// <param name="x">Receives the X coordinate.</param>
    /// <param name="y">Receives the Y coordinate.</param>
    virtual void Deconstruct(out T x, out T y)
    {
        (x, y) = (X, Y);
    }
}
