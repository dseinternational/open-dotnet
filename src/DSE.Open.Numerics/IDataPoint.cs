// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public interface IDataPoint<TX, TY>
    where TX : INumber<TX>
    where TY : INumber<TY>
{
    TX X { get; }

    TY Y { get; }

    virtual void Deconstruct(out TX x, out TY y)
    {
        (x, y) = (X, Y);
    }
}
