// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open;

/// <summary>
/// An enumerator over the elements of a <see cref="Memory{T}"/> exposed by reference.
/// </summary>
public ref struct MemoryEnumerator<T>
{
    private readonly Span<T> _span;
    private int _index;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal MemoryEnumerator(Memory<T> data)
    {
        _span = data.Span;
        _index = -1;
    }

    /// <summary>
    /// Gets a reference to the current element.
    /// </summary>
    public readonly ref T Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref _span[_index];
    }

    /// <summary>
    /// Advances to the next element. Returns <see langword="false"/> when past the end.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        return ++_index < _span.Length;
    }

    /// <summary>
    /// Resets the enumerator to its initial position.
    /// </summary>
    public void Reset()
    {
        _index = -1;
    }
}

/// <summary>
/// Provides extensions for enumerating <see cref="Memory{T}"/>.
/// </summary>
public static class MemoryEnumerator
{
    /// <summary>
    /// Returns a <see cref="MemoryEnumerator{T}"/> over <paramref name="data"/>.
    /// </summary>
    public static MemoryEnumerator<T> GetEnumerator<T>(this Memory<T> data)
    {
        return new(data);
    }
}
