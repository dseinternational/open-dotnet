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

}
