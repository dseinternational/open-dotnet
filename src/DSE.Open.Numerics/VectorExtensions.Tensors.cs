// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

public static partial class VectorExtensions
{
    /// <summary>
    /// Returns a 1-D <see cref="ReadOnlyTensorSpan{T}"/> over the elements of
    /// <paramref name="vector"/> for use with <see cref="System.Numerics.Tensors.TensorPrimitives"/>,
    /// or <c>default</c> when <paramref name="vector"/> is <see langword="null"/>.
    /// </summary>
    public static ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan<T>(this IReadOnlyVector<T>? vector)
        where T : IEquatable<T>
    {
        if (vector is null)
        {
            return default;
        }

        return new ReadOnlyTensorSpan<T>(vector.AsSpan());
    }

    /// <summary>
    /// Returns a multi-dimensional <see cref="ReadOnlyTensorSpan{T}"/> over the
    /// elements of <paramref name="vector"/>, reshaped to the given
    /// <paramref name="lengths"/>. The product of <paramref name="lengths"/>
    /// must equal <paramref name="vector"/>'s length. Returns <c>default</c>
    /// when <paramref name="vector"/> is <see langword="null"/>.
    /// </summary>
    public static ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan<T>(
        this IReadOnlyVector<T>? vector,
        scoped ReadOnlySpan<nint> lengths)
        where T : IEquatable<T>
    {
        if (vector is null)
        {
            return default;
        }

        return new ReadOnlyTensorSpan<T>(vector.AsSpan(), lengths);
    }

    /// <summary>
    /// Returns a multi-dimensional <see cref="ReadOnlyTensorSpan{T}"/> over the
    /// elements of <paramref name="vector"/> with the given
    /// <paramref name="lengths"/> and <paramref name="strides"/> (in elements).
    /// Returns <c>default</c> when <paramref name="vector"/> is <see langword="null"/>.
    /// </summary>
    public static ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan<T>(
        this IReadOnlyVector<T>? vector,
        scoped ReadOnlySpan<nint> lengths,
        scoped ReadOnlySpan<nint> strides)
        where T : IEquatable<T>
    {
        if (vector is null)
        {
            return default;
        }

        return new ReadOnlyTensorSpan<T>(vector.AsSpan(), lengths, strides);
    }

    /// <summary>
    /// Returns a 1-D mutable <see cref="TensorSpan{T}"/> over the elements of
    /// <paramref name="vector"/>, or <c>default</c> when <paramref name="vector"/>
    /// is <see langword="null"/>.
    /// </summary>
    public static TensorSpan<T> AsTensorSpan<T>(this IVector<T>? vector)
        where T : IEquatable<T>
    {
        if (vector is null)
        {
            return default;
        }

        return new TensorSpan<T>(vector.AsSpan());
    }

    /// <summary>
    /// Returns a multi-dimensional mutable <see cref="TensorSpan{T}"/> over the
    /// elements of <paramref name="vector"/>, reshaped to the given
    /// <paramref name="lengths"/>. Returns <c>default</c> when
    /// <paramref name="vector"/> is <see langword="null"/>.
    /// </summary>
    public static TensorSpan<T> AsTensorSpan<T>(this IVector<T>? vector, scoped ReadOnlySpan<nint> lengths)
        where T : IEquatable<T>
    {
        if (vector is null)
        {
            return default;
        }

        return new TensorSpan<T>(vector.AsSpan(), lengths);
    }

    /// <summary>
    /// Returns a multi-dimensional mutable <see cref="TensorSpan{T}"/> over the
    /// elements of <paramref name="vector"/> with the given
    /// <paramref name="lengths"/> and <paramref name="strides"/> (in elements).
    /// Returns <c>default</c> when <paramref name="vector"/> is <see langword="null"/>.
    /// </summary>
    public static TensorSpan<T> AsTensorSpan<T>(
        this IVector<T>? vector,
        scoped ReadOnlySpan<nint> lengths,
        scoped ReadOnlySpan<nint> strides)
        where T : IEquatable<T>
    {
        if (vector is null)
        {
            return default;
        }

        return new TensorSpan<T>(vector.AsSpan(), lengths, strides);
    }
}
