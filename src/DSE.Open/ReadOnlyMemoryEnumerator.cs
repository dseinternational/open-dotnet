// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open;

/// <summary>
/// An enumerator over the elements of a <see cref="ReadOnlyMemory{T}"/>.
/// </summary>
public ref struct ReadOnlyMemoryEnumerator<T>
{
    private readonly ReadOnlySpan<T> _span;
    private int _index;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ReadOnlyMemoryEnumerator(ReadOnlyMemory<T> data)
    {
        _span = data.Span;
        _index = -1;
    }

    /// <summary>
    /// Gets the current element.
    /// </summary>
    public readonly T Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _span[_index];
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
/// Provides extensions for enumerating <see cref="ReadOnlyMemory{T}"/>.
/// </summary>
public static class ReadOnlyMemoryEnumerator
{
    /// <summary>
    /// Returns a <see cref="ReadOnlyMemoryEnumerator{T}"/> over <paramref name="data"/>.
    /// </summary>
    public static ReadOnlyMemoryEnumerator<T> GetEnumerator<T>(this ReadOnlyMemory<T> data)
    {
        return new(data);
    }
}
