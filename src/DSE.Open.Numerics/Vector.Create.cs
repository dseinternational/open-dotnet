// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Vector
{
    public static Vector<T> Create<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumber<T>
    {
        return new Vector<T>(sequence.ToArray());
    }

    public static Vector<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new Vector<T>(data);
    }

    public static Vector<T> CreateDefault<T>(int length)
        where T : struct, INumber<T>
    {
        return new Vector<T>(new T[length]);
    }

    public static Vector<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create<T>(length, T.Zero);
    }

    public static Vector<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create<T>(length, T.One);
    }
}
