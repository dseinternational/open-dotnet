// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public interface IHighLowDataPoint<T> : IDataPoint<T>
    where T : INumber<T>
{
    T High { get; }

    T Low { get; }

    virtual void Deconstruct(out T x, out T y, out T high, out T low)
    {
        (x, y, high, low) = (X, Y, High, Low);
    }
}
