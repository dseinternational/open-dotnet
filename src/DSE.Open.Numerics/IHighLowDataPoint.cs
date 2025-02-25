// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public interface IHighLowDataPoint<TX, TY> : IDataPoint<TX, TY>
    where TX : INumber<TX>
    where TY : INumber<TY>
{
    TY High { get; }

    TY Low { get; }

    virtual void Deconstruct(out TX x, out TY y, out TY high, out TY low)
    {
        (x, y, high, low) = (X, Y, High, Low);
    }
}
