// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics.Tensors;

namespace DSE.Open.Numerics;

#pragma warning disable SYSLIB5001 // Type is for evaluation purposes only and is subject to change or removal in future updates.

public static partial class VectorExtensions
{
    public static ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan<T>(this IReadOnlyVector<T>? vector)
        where T : IEquatable<T>
    {
        if (vector is null)
        {
            return default;
        }

        return new ReadOnlyTensorSpan<T>(vector.AsSpan());
    }

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

    public static TensorSpan<T> AsTensorSpan<T>(this IVector<T>? vector)
        where T : IEquatable<T>
    {
        if (vector is null)
        {
            return default;
        }

        return new TensorSpan<T>(vector.AsSpan());
    }

    public static TensorSpan<T> AsTensorSpan<T>(this IVector<T>? vector, scoped ReadOnlySpan<nint> lengths)
        where T : IEquatable<T>
    {
        if (vector is null)
        {
            return default;
        }

        return new TensorSpan<T>(vector.AsSpan(), lengths);
    }

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
