// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

#pragma warning disable DSEOPEN001 // ArrayBuilder ref struct


/// <summary>
/// Extensions for materialising sequences of <see cref="DataPoint{T}"/> values
/// into <see cref="Memory{T}"/>, <see cref="TensorSpan{T}"/>, or <see cref="Tensor{T}"/>
/// for downstream processing.
/// </summary>
public static class DataPointCollectionExtensions
{
    /// <summary>Materialises <paramref name="source"/> into a <see cref="Memory{T}"/> of <see cref="DataPoint{T}"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    public static Memory<DataPoint<T>> ToMemory<T>(
        this IEnumerable<DataPoint<T>> source)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(source);

        using var builder = new ArrayBuilder<DataPoint<T>>();

        foreach (var item in source)
        {
            builder.Add(item);
        }

        return builder.ToMemory();
    }

    /// <summary>
    /// Materialises <paramref name="source"/> into an <c>[N, 2]</c> tensor span:
    /// the first column holds X values, the second holds Y values.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    public static TensorSpan<T> ToTensorSpan<T>(
        this IEnumerable<DataPoint<T>> source)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(source);

        var initialCapacity = 512;

        if (source is ICollection<DataPoint<T>> collection)
        {
            initialCapacity = collection.Count * 2;
        }
        else if (source is IReadOnlyCollection<DataPoint<T>> readOnlyCollection)
        {
            initialCapacity = readOnlyCollection.Count * 2;
        }

        using var builder = new ArrayBuilder<T>(initialCapacity, rentFromPool: true);

        builder.AddRange(source.Select(d => d.X));

        var length = builder.Count;

        builder.AddRange(source.Select(d => d.Y));

        return new TensorSpan<T>(builder.ToMemory().Span, [length, 2], []);
    }

    /// <summary>
    /// Materialises <paramref name="source"/> into an <c>[N, 2]</c> <see cref="Tensor{T}"/>:
    /// the first column holds X values, the second holds Y values.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    public static Tensor<T> ToTensor<T>(
        this IEnumerable<DataPoint<T>> source)
        where T : struct, INumber<T>
    {
        ArgumentNullException.ThrowIfNull(source);

        var initialCapacity = 512;

        if (source is ICollection<DataPoint<T>> collection)
        {
            initialCapacity = collection.Count * 2;
        }
        else if (source is IReadOnlyCollection<DataPoint<T>> readOnlyCollection)
        {
            initialCapacity = readOnlyCollection.Count * 2;
        }

        using var builder = new ArrayBuilder<T>(initialCapacity, rentFromPool: true);

        builder.AddRange(source.Select(d => d.X));

        var length = builder.Count;

        builder.AddRange(source.Select(d => d.Y));

        return Tensor.Create(builder.ToArray(), [length, 2]);
    }
}
