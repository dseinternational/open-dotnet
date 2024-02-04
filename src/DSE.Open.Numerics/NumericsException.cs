// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

public class NumericsException : Exception
{
    private const string s_defaultMessage = "Numerics error.";

    public NumericsException()
        : base(s_defaultMessage)
    {
    }

    public NumericsException(string message)
        : base(message ?? s_defaultMessage)
    {
    }

    public NumericsException(string message, Exception innerException)
        : base(message ?? s_defaultMessage, innerException)
    {
    }

    public static void Throw()
    {
        throw new NumericsException(s_defaultMessage);
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
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count)
        {
            Throw("Collections must be the same length.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength(IReadOnlyVector x, IReadOnlyVector y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Length != y.Length)
        {
            Throw("Vectors must be the same length.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(ReadOnlyVector<T> x, ReadOnlyVector<T> y)
        where T : struct, INumber<T>
    {
        if (x.Length != y.Length)
        {
            Throw("Vectors must be the same length.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotEqualLength<T>(ReadOnlySpanVector<T> x, ReadOnlySpanVector<T> y)
        where T : struct, INumber<T>
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
    public static void ThrowIfNotEqualLength<T>(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
    {
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotSameShape<T>(ReadOnlyMultiSpan<T> x, ReadOnlyMultiSpan<T> y)
    {
        if (x.Length != y.Length || !x.Shape.SequenceEqual(y.Shape))
        {
            Throw("Tensors must be the same shape.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotSameShape<T>(ReadOnlyMultiSpan<T> x, ReadOnlyMultiSpan<T> y, ReadOnlyMultiSpan<T> z)
    {
        if (x.Length != y.Length || x.Length != z.Length
            || !x.Shape.SequenceEqual(y.Shape) || !x.Shape.SequenceEqual(z.Shape))
        {
            Throw("Tensors must be the same shape.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotSameShape<T>(ReadOnlyTensorSpan<T> x, ReadOnlyTensorSpan<T> y)
        where T : struct, INumber<T>
    {
        if (x.Length != y.Length || !x.Shape.SequenceEqual(y.Shape))
        {
            Throw("Tensors must be the same shape.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNotSameShape<T>(ReadOnlyTensorSpan<T> x, ReadOnlyTensorSpan<T> y, ReadOnlyTensorSpan<T> z)
        where T : struct, INumber<T>
    {
        if (x.Length != y.Length || x.Length != z.Length
            || !x.Shape.SequenceEqual(y.Shape) || !x.Shape.SequenceEqual(z.Shape))
        {
            Throw("Tensors must be the same shape.");
        }
    }

}
