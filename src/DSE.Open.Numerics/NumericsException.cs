// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

public class NumericsException : Exception
{
    private const string DefaultMessage = "Numerics error.";

    public NumericsException()
        : base(DefaultMessage)
    {
    }

    public NumericsException(string message)
        : base(message ?? DefaultMessage)
    {
    }

    public NumericsException(string message, Exception innerException)
        : base(message ?? DefaultMessage, innerException)
    {
    }

    public static void Throw()
    {
        throw new NumericsException(DefaultMessage);
    }

    public static void Throw(string message)
    {
        throw new NumericsException(message);
    }

    public static void Throw(string message, Exception innerException)
    {
        throw new NumericsException(message, innerException);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(ICollection<T> x, ICollection<T> y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count)
        {
            Throw("Collections must be the same length.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(ReadOnlyNumericVector<T> x, ReadOnlyNumericVector<T> y)
        where T : struct, INumber<T>
    {
        if (x.Length != y.Length)
        {
            Throw("Vectors must be the same length.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
    {
        if (x.Length != y.Length)
        {
            Throw("Vectors must be the same length.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(T[] x, T[] y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Length != y.Length)
        {
            Throw("Vectors must be the same length.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y, ReadOnlySpan<T> z)
    {
        if (x.Length != y.Length || x.Length != z.Length)
        {
            Throw("Vectors must be the same length.");
        }
    }
}
