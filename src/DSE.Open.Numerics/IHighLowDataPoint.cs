// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// A data point with a low/high range — for instance, OHLC candles, error bars,
/// or confidence intervals around an <see cref="IDataPoint{T}.Y"/> value.
/// </summary>
/// <typeparam name="T">The coordinate value type.</typeparam>
public interface IHighLowDataPoint<T> : IDataPoint<T>
    where T : INumber<T>
{
    /// <summary>Gets the upper bound of the range.</summary>
    T High { get; }

    /// <summary>Gets the lower bound of the range.</summary>
    T Low { get; }

    /// <summary>Deconstructs into <c>x</c>, <c>y</c>, <c>high</c>, and <c>low</c>.</summary>
    virtual void Deconstruct(out T x, out T y, out T high, out T low)
    {
        (x, y, high, low) = (X, Y, High, Low);
    }
}
