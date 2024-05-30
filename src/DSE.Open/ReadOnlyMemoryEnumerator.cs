// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace DSE.Open;

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

    public T Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _span[_index];
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

public static class ReadOnlyMemoryEnumerator
{
    public static ReadOnlyMemoryEnumerator<T> GetEnumerator<T>(this ReadOnlyMemory<T> data)
    {
        return new(data);
    }
}
