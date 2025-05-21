// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DSE.Open.Numerics;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public partial class Vector
{
    #region Equals

    [Obsolete("Not supported", error: true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static new bool Equals(object? a, object? b)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Compares the elements of two vectors for equality using the equality operator provided by the
    /// <see cref="IEqualityOperators{TSelf,TOther,Boolen}"/> implementation for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector<bool> Equals<T>(IReadOnlyVector<T> x, IReadOnlyVector<T> y)
        where T : IEquatable<T>, IEqualityOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);
        var xspan = x.AsSpan();
        var yspan = y.AsSpan();
        return Equals(in xspan, in yspan);
    }

    /// <summary>
    /// Compares the elements of two vectors for equality using the equality operator provided by the
    /// <see cref="IEqualityOperators{TSelf,TOther,Boolen}"/> implementation for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector<bool> Equals<T>(scoped in ReadOnlySpan<T> x, scoped in ReadOnlySpan<T> y)
        where T : IEqualityOperators<T, T, bool>
    {
        return Create(VectorPrimitives.Equals(x, y));
    }

    /// <summary>
    /// Compares the elements of two vectors for equality using the equality operator provided by the
    /// <see cref="IEqualityOperators{TSelf,TOther,Boolen}"/> implementation for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static Vector<bool> Equals<T>(
        scoped in ReadOnlySpan<T> x,
        scoped in ReadOnlySpan<T> y,
        in Span<bool> destination)
        where T : IEqualityOperators<T, T, bool>
    {
        return Create(VectorPrimitives.Equals(x, y, destination));
    }

    /// <summary>
    /// Compares the elements of a vector with a scalar for equality using the equality operator provided by the
    /// <see cref="IEqualityOperators{TSelf,TOther,Boolen}"/> implementation for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector<bool> Equals<T>(scoped in ReadOnlySpan<T> x, in T y)
        where T : IEqualityOperators<T, T, bool>
    {
        return Create(VectorPrimitives.Equals(x, y));
    }

    /// <summary>
    /// Compares the elements of a vector with a scalar for equality using the equality operator provided by the
    /// <see cref="IEqualityOperators{TSelf,TOther,Boolen}"/> implementation for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static Vector<bool> Equals<T>(
        scoped in ReadOnlySpan<T> x,
        in T y,
        in Span<bool> destination)
        where T : IEqualityOperators<T, T, bool>
    {
        return Create(VectorPrimitives.Equals(x, y, destination));
    }

    #endregion

    #region EqualsAll

    public static bool EqualsAll<T>(in ReadOnlySpan<T> x, in ReadOnlySpan<T> y)
        where T : IEqualityOperators<T, T, bool>
    {
        return VectorPrimitives.EqualsAll(x, y);
    }

    public static bool EqualsAll<T>(scoped in ReadOnlySpan<T> x, in T y)
        where T : IEqualityOperators<T, T, bool>
    {
        return VectorPrimitives.EqualsAll(x, y);
    }

    #endregion

    #region EqualsAny

    public static bool EqualsAny<T>(in ReadOnlySpan<T> x, in ReadOnlySpan<T> y)
        where T : IEqualityOperators<T, T, bool>
    {
        return VectorPrimitives.EqualsAny(x, y);
    }

    public static bool EqualsAny<T>(scoped in ReadOnlySpan<T> x, in T y)
        where T : IEqualityOperators<T, T, bool>
    {
        return VectorPrimitives.EqualsAny(x, y);
    }

    #endregion

    // ------------------------- TODO -------------------------

    /// <summary>
    /// Determines whether two vectors are equal by comparing the elements using
    /// <see cref="IEquatable{T}.Equals(T)"/>. This comparison considers corresponding
    /// unknown values (<see langword="null"/> or <see cref="INaValue{TSelf, T}.Na"/> or
    /// <see cref="IFloatingPointIeee754{TSelf}.NaN"/>) to be equal. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns><see langword="true"/> if the vectors are the same length and each corresponding
    /// element is determined to be equal using <see cref="IEquatable{T}.Equals(T)"/>.</returns>
    public static bool SequenceEqual<T>(IReadOnlyVector<T> v1, IReadOnlyVector<T> v2)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        return SequenceEqual(v1, v2.AsSpan());
    }

    /// <summary>
    /// Determines whether two vectors are equal by comparing the elements using
    /// <see cref="IEquatable{T}.Equals(T)"/>. This comparison considers corresponding
    /// unknown values (<see langword="null"/> or <see cref="INaValue{TSelf, T}.Na"/> or
    /// <see cref="IFloatingPointIeee754{TSelf}.NaN"/>) to be equal. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static bool SequenceEqual<T>(IReadOnlyVector<T> v1, ReadOnlySpan<T> v2)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(v1);

        return SequenceEqual(v1, v2, true);
    }

    internal static bool TryCastToPrimtive<T>(IReadOnlyVector<T> v1, out ReadOnlySpan<T> span)
        where T : struct, IEquatable<T>
    {
        if (v1 is IReadOnlyVector<NaInt<int>> naInt32Vector)
        {
            span = CastDangerous<NaInt<int>, T>(naInt32Vector.AsSpan());
            return true;
        }

        if (v1 is IReadOnlyVector<NaInt<long>> naInt64Vector)
        {
            span = CastDangerous<NaInt<long>, T>(naInt64Vector.AsSpan());
            return true;
        }

        span = default;
        return false;
    }

    private static ReadOnlySpan<TTo> CastDangerous<TFrom, TTo>(ReadOnlySpan<TFrom> v1)
        where TFrom : struct
        where TTo : struct
    {
        ref var first = ref Unsafe.As<TFrom, TTo>(ref MemoryMarshal.GetReference(v1));
        return MemoryMarshal.CreateReadOnlySpan(ref first, v1.Length);
    }

    public static bool SequenceEqual(
        IReadOnlyVector<string> v1,
        ReadOnlySpan<string> v2,
        StringComparer? stringComparer = default)
    {
        ArgumentNullException.ThrowIfNull(v1);
        return v1.AsSpan().SequenceEqual(v2, stringComparer);
    }

    public static bool SequenceEqual<T>(IReadOnlyVector<T> v1, ReadOnlySpan<T> v2, bool unknownsEqual)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(v1);

        if (v1.Length != v2.Length)
        {
            return false;
        }

        if (v1.IsEmpty) // already established that lengths are equal
        {
            return true;
        }

        if (unknownsEqual)
        {
            //if (TryCastToPrimtive(v1, out var v1Span))
            //{
            //    return v1Span.SequenceEqual(v2);
            //}
            // TODO: if nullable number, cast to underlying primitive type span

            return v1.AsSpan().SequenceEqual(v2);
        }

        // TODO: optimise

        for (var i = 0; i < v1.Length; i++)
        {
            var e1 = v1[i];
            var e2 = v2[i];

            if ((e1 is INaValue n1 && n1.IsNa)
                || (e2 is INaValue n2 && n2.IsNa)
                || !e1.Equals(e2))
            {
                return false;
            }
        }

        return true;
    }

    public static void ElementsEqual<T>(IReadOnlyVector<T> v1, IReadOnlyVector<T> v2, Span<bool> result)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        if (v1.Length != v2.Length)
        {
            throw new ArgumentException("Vectors must be of the same length.");
        }

        if (result.Length != v1.Length)
        {
            throw new ArgumentException("Result span must be of the same length as the vectors.");
        }

        for (var i = 0; i < v1.Length; i++)
        {
            result[i] = v1[i].Equals(v2[i]);
        }
    }
}
