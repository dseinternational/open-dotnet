// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DSE.Open.Memory;

public static class ArrayBuilder
{
    internal const int DefaultOwnedCapacity = 256;
    internal const int DefaultRentedCapacity = 1024;
}

/// <summary>
/// Supports the construction of arrays using stack-allocated, pooled and owned buffers.
/// <b>⚠ Warning:</b> only pass this struct by reference.
/// </summary>
/// <typeparam name="T"></typeparam>
[Experimental(DiagnosticIds.ArrayBuilderRefStructWarning,
    UrlFormat = "https://github.com/dseinternational/open-dotnet/blob/main/src/DSE.Open/Memory/ArrayBuilder.cs")]
public ref struct ArrayBuilder<T>
{
    private T[]? _pooledBuffer;
    private T[]? _ownedBuffer;
    private Span<T> _buffer;
    private int _count;
    private bool _rentFromPool;

    public ArrayBuilder() : this(false) { }

    public ArrayBuilder(bool rentFromPool)
        : this(rentFromPool ? ArrayBuilder.DefaultRentedCapacity : ArrayBuilder.DefaultOwnedCapacity, rentFromPool) { }

    /// <summary>
    /// Initializes the <see cref="ArrayBuilder{T}"/> with a specified capacity.
    /// </summary>
    /// <param name="capacity">The capacity of the array to allocate.</param>
    /// <param name="rentFromPool">Whether the buffer should be rented from the shared ArrayPool.</param>
    public ArrayBuilder(int capacity, bool rentFromPool = false)
    {
        Guard.IsGreaterThanOrEqualTo(capacity, 0);

        if (capacity > 0)
        {
            if (rentFromPool)
            {
                _buffer = _pooledBuffer = ArrayPool<T>.Shared.Rent(capacity);
            }
            else
            {
                _buffer = _ownedBuffer = new T[capacity];
            }
        }

        _rentFromPool = rentFromPool;
    }

    public ArrayBuilder(Span<T> initialBuffer) : this(initialBuffer, false) { }

    /// <summary>
    /// Initializes the <see cref="ArrayBuilder{T}"/> with a specified initial buffer.
    /// </summary>
    /// <param name="initialBuffer"></param>
    /// <param name="rentFromPool">Whether new buffers should be rented from the shared ArrayPool.</param>
    public ArrayBuilder(Span<T> initialBuffer, bool rentFromPool)
    {
        _buffer = initialBuffer;
        _rentFromPool = rentFromPool;
    }

    /// <summary>
    /// Gets the number of items this instance can store without re-sizing.
    /// </summary>
    public readonly int Capacity => _buffer.Length;

    public readonly int Count => _count;

    /// <summary>
    /// Gets or sets the item at a certain index in the array.
    /// </summary>
    /// <param name="index">The index into the array.</param>
    public readonly T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Guard.IsInRange(index, 0, _count);
            return _buffer[index];
        }
    }

    /// <summary>
    /// Indicated if a buffer has been allocated by the ArrayBuilder. This will be false
    /// if an initial buffer was provided and its capacity has not been exceeded.
    /// </summary>
    public readonly bool BufferAllocated => _ownedBuffer is not null || _pooledBuffer is not null;

    /// <summary>
    /// Adds an item to the backing array, resizing it if necessary.
    /// </summary>
    /// <param name="item">The item to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        if (_count == Capacity)
        {
            EnsureCapacity(_count + 1);
        }

        UncheckedAdd(item);
    }

    public void AddRange(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        foreach (var item in items)
        {
            Add(item);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UncheckedAdd(T item)
    {
        Debug.Assert(_count < Capacity);
        _buffer[_count++] = item;
    }

    private void EnsureCapacity(int minimum)
    {
        Debug.Assert(minimum > Capacity);

        var capacity = Capacity;
        var newCapacity = capacity == 0 ? ArrayBuilder.DefaultOwnedCapacity : 2 * capacity;

        if ((uint)newCapacity > (uint)Array.MaxLength)
        {
            newCapacity = Math.Max(capacity + 1, Array.MaxLength);
        }

        newCapacity = Math.Max(newCapacity, minimum);

        var newBuffer = _rentFromPool
            ? _pooledBuffer = ArrayPool<T>.Shared.Rent(capacity)
            : _ownedBuffer = new T[newCapacity];

        if (_count > 0)
        {
            _buffer.CopyTo(newBuffer);
        }

        _buffer = newBuffer;
    }

    /// <summary>
    /// Copies the contents of this builder to a new array.
    /// </summary>
    /// <returns></returns>
    public readonly T[] ToArray()
    {
        if (_count == 0)
        {
            return [];
        }

        if (_ownedBuffer is not null && _ownedBuffer.Length == _count)
        {
            return _ownedBuffer;
        }

        var toReturn = new T[_count];

        _buffer[.._count].CopyTo(toReturn);

        return toReturn;
    }

    /// <summary>
    /// If the buffer is owned (rather than rented from the pool), returns a <see cref="Memory{T}"/>
    /// over the owned array, otherwise returns a <see cref="Memory{T}"/> over a new array into which
    /// the items are copied.
    /// </summary>
    /// <returns></returns>
    public readonly Memory<T> ToMemory()
    {
        if (_ownedBuffer is not null)
        {
            return new Memory<T>(_ownedBuffer, 0, _count);
        }
        else
        {
            return _buffer[.._count].ToArray();
        }
    }

    /// <summary>
    /// If the buffer is owned (rather than rented from the pool), returns a <see cref="Memory{T}"/>
    /// over the owned array, otherwise returns a <see cref="Memory{T}"/> over a new array into which
    /// the items are copied.
    /// </summary>
    /// <returns></returns>
    public readonly ReadOnlyMemory<T> ToReadOnlyMemory()
    {
        if (_ownedBuffer is not null)
        {
            return new ReadOnlyMemory<T>(_ownedBuffer, 0, _count);
        }
        else
        {
            return _buffer[.._count].ToArray();
        }
    }

    public readonly ReadOnlySpan<T> Buffer => _buffer;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        var toReturn = _pooledBuffer;
        this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
        if (toReturn != null)
        {
            ArrayPool<T>.Shared.Return(toReturn);
        }
    }
}
