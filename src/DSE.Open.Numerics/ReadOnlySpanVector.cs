// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public readonly ref struct ReadOnlySpanVector<T>
    where T : struct, INumber<T>
{
    private readonly ReadOnlySpan<T> _sequence;

    public ReadOnlySpanVector(ReadOnlySpan<T> sequence)
    {
        _sequence = sequence;
    }

    internal ReadOnlySpan<T> Span => _sequence;

    public int Length => _sequence.Length;

    public T this[int index] => _sequence[index];
}
