// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static class Vector
{
    public static Vector<T> Create<T>(T[] data)
        where T : notnull
    {
        EnsureNotKnownNumericType(typeof(T));
        return new Vector<T>(data);
    }

    public static Vector<T> Create<T>(Memory<T> data)
        where T : notnull
    {
        EnsureNotKnownNumericType(typeof(T));
        return new Vector<T>(data);
    }

    public static Vector<T> Create<T>(T[] data, int start, int length)
        where T : notnull
    {
        EnsureNotKnownNumericType(typeof(T));
        return new Vector<T>(data, start, length);
    }

    public static NumericVector<T> CreateNumeric<T>(T[] data, bool copy = false)
        where T : struct, INumber<T>
    {
        return new NumericVector<T>(copy ? [.. data] : data);
    }

    public static NumericVector<T> CreateNumeric<T>(Memory<T> data, bool copy = false)
        where T : struct, INumber<T>
    {
        return new NumericVector<T>(copy ? data.ToArray() : data);
    }

    public static NumericVector<T> CreateNumeric<T>(ReadOnlySpan<T> data)
        where T : struct, INumber<T>
    {
        return new NumericVector<T>(data.ToArray());
    }

    public static NumericVector<T> CreateNumeric<T>(T[] data, int start, int length)
        where T : struct, INumber<T>
    {
        return new NumericVector<T>(data, start, length);
    }

    public static NumericVector<T> CreateNumeric<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new(data);
    }

    public static NumericVector<T> CreateNumeric<T>(int length)
        where T : struct, INumber<T>
    {
        return new(new T[length]);
    }

    public static NumericVector<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return CreateNumeric(length, T.Zero);
    }

    public static NumericVector<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return CreateNumeric(length, T.One);
    }

    internal static void EnsureKnownNumericType(Type type)
    {
        if (!NumberHelper.IsKnownNumberType(type))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Expected numeric type but {type.Name} is not numeric.");
        }
    }

    internal static void EnsureNotKnownNumericType(Type type)
    {
        if (NumberHelper.IsKnownNumberType(type))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Expected non-numeric type but {type.Name} is numeric.");
        }
    }
}
