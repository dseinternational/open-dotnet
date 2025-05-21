// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Numerics;
using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

// Note: these use IEqualityOperators{TSelf,TOther,Boolen} rather than IEquatable<T> as SequenceEqual does

public static partial class VectorPrimitives
{
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
    /// <remarks>
    /// For types that can represent <see langword="null"/>, <c>NA</c> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result of <see cref="IEqualityOperators{TSelf, TOther, Boolean}"/> may be different from the result
    /// of <see cref="IEquatable{T}.Equals(T)"/> provided by the type.
    /// </remarks>
    public static Vector<bool> Equals<T>(scoped in ReadOnlySpan<T> x, scoped in ReadOnlySpan<T> y)
        where T : IEqualityOperators<T, T, bool>
    {
        NumericsException.ThrowIfNot(x.Length == y.Length);
        Span<bool> dest = new bool[y.Length];
        _ = Equals(x, y, dest);
        return Vector.Create(dest);
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
    /// <remarks>
    /// For types that can represent <see langword="null"/>, <c>NA</c> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result of <see cref="IEqualityOperators{TSelf, TOther, Boolean}"/> may be different from the result
    /// of <see cref="IEquatable{T}.Equals(T)"/> provided by the type.
    /// </remarks>
    public static Vector<bool> Equals<T>(
        scoped in ReadOnlySpan<T> x,
        scoped in ReadOnlySpan<T> y,
        in Span<bool> destination)
        where T : IEqualityOperators<T, T, bool>
    {
        NumericsException.ThrowIfNot(x.Length == y.Length && y.Length == destination.Length);

        _ = Tensor.Equals(
            new ReadOnlyTensorSpan<T>(x),
            new ReadOnlyTensorSpan<T>(y),
            new TensorSpan<bool>(destination));

        return Vector.Create(destination);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <remarks>
    /// For types that can represent <see langword="null"/>, <c>NA</c> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result of <see cref="IEqualityOperators{TSelf, TOther, Boolean}"/> may be different from the result
    /// of <see cref="IEquatable{T}.Equals(T)"/> provided by the type.
    /// </remarks>
    public static Vector<bool> Equals<T>(scoped in ReadOnlySpan<T> x, in T y)
        where T : IEqualityOperators<T, T, bool>
    {
        Span<bool> dest = new bool[x.Length];
        _ = Equals(x, y, dest);
        return Vector.Create(dest);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    /// <remarks>
    /// For types that can represent <see langword="null"/>, <c>NA</c> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result of <see cref="IEqualityOperators{TSelf, TOther, Boolean}"/> may be different from the result
    /// of <see cref="IEquatable{T}.Equals(T)"/> provided by the type.
    /// </remarks>
    public static Vector<bool> Equals<T>(
        scoped in ReadOnlySpan<T> x,
        in T y,
        in Span<bool> destination)
        where T : IEqualityOperators<T, T, bool>
    {
        NumericsException.ThrowIfNot(x.Length == destination.Length);

        _ = Tensor.Equals(
            new ReadOnlyTensorSpan<T>(x),
            y,
            new TensorSpan<bool>(destination));

        return Vector.Create(destination);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <remarks>
    /// For types that can represent <see langword="null"/>, <c>NA</c> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result of <see cref="IEqualityOperators{TSelf, TOther, Boolean}"/> may be different from the result
    /// of <see cref="IEquatable{T}.Equals(T)"/> provided by the type. Therefore, the result of this method
    /// may differ from the result of <see cref="SequenceEqual{T}(IReadOnlyVector{T}, in ReadOnlySpan{T})"/>.
    /// </remarks>
    public static bool EqualsAll<T>(in ReadOnlySpan<T> x, in ReadOnlySpan<T> y)
        where T : IEqualityOperators<T, T, bool>
    {
        NumericsException.ThrowIfNot(x.Length == y.Length);

        return Tensor.EqualsAll(
            new ReadOnlyTensorSpan<T>(x),
            new ReadOnlyTensorSpan<T>(y));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <remarks>
    /// For types that can represent <see langword="null"/>, <c>NA</c> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result of <see cref="IEqualityOperators{TSelf, TOther, Boolean}"/> may be different from the result
    /// of <see cref="IEquatable{T}.Equals(T)"/> provided by the type.
    /// </remarks>
    public static bool EqualsAll<T>(scoped in ReadOnlySpan<T> x, in T y)
        where T : IEqualityOperators<T, T, bool>
    {
        return Tensor.EqualsAll(
            new ReadOnlyTensorSpan<T>(x),
            y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <remarks>
    /// For types that can represent <see langword="null"/>, <c>NA</c> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result of <see cref="IEqualityOperators{TSelf, TOther, Boolean}"/> may be different from the result
    /// of <see cref="IEquatable{T}.Equals(T)"/> provided by the type.
    /// </remarks>
    public static bool EqualsAny<T>(in ReadOnlySpan<T> x, in ReadOnlySpan<T> y)
        where T : IEqualityOperators<T, T, bool>
    {
        NumericsException.ThrowIfNot(x.Length == y.Length);

        return Tensor.EqualsAny(
            new ReadOnlyTensorSpan<T>(x),
            new ReadOnlyTensorSpan<T>(y));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <remarks>
    /// For types that can represent <see langword="null"/>, <c>NA</c> or <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result of <see cref="IEqualityOperators{TSelf, TOther, Boolean}"/> may be different from the result
    /// of <see cref="IEquatable{T}.Equals(T)"/> provided by the type.
    /// </remarks>
    public static bool EqualsAny<T>(scoped in ReadOnlySpan<T> x, in T y)
        where T : IEqualityOperators<T, T, bool>
    {
        return Tensor.EqualsAny(
            new ReadOnlyTensorSpan<T>(x),
            y);
    }
}
