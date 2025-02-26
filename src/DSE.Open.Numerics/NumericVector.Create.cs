// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class NumericVector
{
    public static NumericVector<T> Create<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumber<T>
    {
        return new(sequence.ToArray());
    }

    public static NumericVector<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new(data);
    }

    public static NumericVector<T> CreateDefault<T>(int length)
        where T : struct, INumber<T>
    {
        return new(new T[length]);
    }

    public static NumericVector<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create<T>(length, T.Zero);
    }

    public static NumericVector<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create<T>(length, T.One);
    }
}
