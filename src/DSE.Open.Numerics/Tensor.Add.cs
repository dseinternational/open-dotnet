// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

public static partial class Tensor
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add<T>(ReadOnlyTensor<T> x, ReadOnlyTensor<T> y, Tensor<T> destination)
        where T : struct, INumber<T>
    {
        MultiSpanPrimitives.Add(x.Span, y.Span, destination.TensorSpan.Span);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddInPlace<T>(Tensor<T> x, ReadOnlyTensor<T> y)
        where T : struct, INumber<T>
    {
        MultiSpanPrimitives.AddInPlace(x.TensorSpan.Span, y.Span);
    }
}


#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
public static partial class Tensor
{
    /// <summary>
    /// Creates a new 1-dimensional tensor containing copies of the elements
    /// from a 1-dimensional array. To create without copying, use
    /// <see cref="MultiMemory{T}(Memory{T})"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static Tensor<T> Create<T>(T[] elements)
        where T : struct, INumber<T>
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new 2-dimensional tensor containing copies of the elements
    /// from a 2-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static Tensor<T> Create<T>(T[,] elements)
        where T : struct, INumber<T>
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new 3-dimensional tensor containing copies of the elements
    /// from a 3-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static Tensor<T> Create<T>(T[,,] elements)
        where T : struct, INumber<T>
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new 4-dimensional tensor containing copies of the elements
    /// from a 4-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static Tensor<T> Create<T>(T[,,,] elements)
        where T : struct, INumber<T>
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new n-dimensional tensor containing copies of the elements
    /// from a n-dimensional tensor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static Tensor<T> Create<T>(MultiSpan<T> elements)
        where T : struct, INumber<T>
    {
        return new Tensor<T>(elements.Elements.ToArray(), elements.Shape);
    }

    /// <summary>
    /// Creates a new 2-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <returns></returns>
    public static Tensor<T> CreateWithDimensions<T>(uint d1Length, uint d2Length)
        where T : struct, INumber<T>
    {
        uint[] d = [d1Length, d2Length];
        return new Tensor<T>(default, d);
    }

    /// <summary>
    /// Creates a new 3-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <param name="d3Length"></param>
    /// <returns></returns>
    public static Tensor<T> CreateWithDimensions<T>(
        uint d1Length,
        uint d2Length,
        uint d3Length)
        where T : struct, INumber<T>
    {
        uint[] d = [d1Length, d2Length, d3Length];
        return new Tensor<T>(default, d);
    }

    /// <summary>
    /// Creates a new n-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static Tensor<T> CreateWithDimensions<T>(ReadOnlySpan<uint> shape)
        where T : struct, INumber<T>
    {
        return new Tensor<T>(default, shape);
    }

    public static Tensor<T> CreateUninitialized<T>(ReadOnlySpan<uint> shape)
        where T : struct, INumber<T>
    {
        return new Tensor<T>(MultiMemory.CreateUninitialized<T>(shape));
    }
}
