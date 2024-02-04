// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Memory;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

public static partial class MultiMemory
{
    /// <summary>
    /// Creates a new 1-dimensional view over memory containing copies of the elements
    /// from a 1-dimensional array. To create without copying, use
    /// <see cref="MultiMemory{T}(Memory{T})"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(T[] elements)
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new 2-dimensional view over memory containing copies of the elements
    /// from a 2-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(T[,] elements)
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new 3-dimensional view over memory containing copies of the elements
    /// from a 3-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(T[,,] elements)
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new 4-dimensional view over memory containing copies of the elements
    /// from a 4-dimensional array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(T[,,,] elements)
    {
        return Create(new MultiSpan<T>(elements));
    }

    /// <summary>
    /// Creates a new n-dimensional view over memory containing copies of the elements
    /// from a n-dimensional view over memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elements"></param>
    /// <returns></returns>
    public static MultiMemory<T> Create<T>(MultiSpan<T> elements)
    {
        return new MultiMemory<T>(elements.Elements.ToArray(), elements.Shape, false);
    }

    /// <summary>
    /// Creates a new 2-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <returns></returns>
    public static MultiMemory<T> CreateWithDimensions<T>(uint d1Length, uint d2Length)
    {
        uint[] d = [d1Length, d2Length];
        return new MultiMemory<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new 3-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d1Length"></param>
    /// <param name="d2Length"></param>
    /// <param name="d3Length"></param>
    /// <returns></returns>
    public static MultiMemory<T> CreateWithDimensions<T>(
        uint d1Length,
        uint d2Length,
        uint d3Length)
    {
        uint[] d = [d1Length, d2Length, d3Length];
        return new MultiMemory<T>(default, d, true);
    }

    /// <summary>
    /// Creates a new n-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static MultiMemory<T> CreateWithDimensions<T>(ReadOnlySpan<uint> shape)
    {
        return new MultiMemory<T>(default, shape, true);
    }

    /// <summary>
    /// Creates a new n-dimensional <see cref="MultiMemory{T}"/> with dimensions of the specified lengths.
    /// <b>Warning</b>: the underlying memory is not initialised. This should only be used when the caller
    /// is setting all of the values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static MultiMemory<T> CreateUninitialized<T>(ReadOnlySpan<uint> shape)
    {
        return new MultiMemory<T>(default, shape, true, true);
    }
}
