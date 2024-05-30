// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open;

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

    public ref T Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref _span[_index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        return ++_index < _span.Length;
    }

    public void Reset()
    {
        _index = -1;
    }
}

public static class MemoryEnumerator
{
    public static MemoryEnumerator<T> GetEnumerator<T>(this Memory<T> data)
    {
        return new(data);
    }
}
