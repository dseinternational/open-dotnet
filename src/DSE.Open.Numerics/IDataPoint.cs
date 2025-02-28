// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public interface IDataPoint<T>
    where T : INumber<T>
{
    T X { get; }

    T Y { get; }

    virtual void Deconstruct(out T x, out T y)
    {
        (x, y) = (X, Y);
    }
}
