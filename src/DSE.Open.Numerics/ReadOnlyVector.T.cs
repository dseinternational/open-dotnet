// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

[CollectionBuilder(typeof(ReadOnlyVector), nameof(ReadOnlyVector.Create))]
public readonly struct ReadOnlyVector<T> : IEquatable<ReadOnlyVector<T>>
    where T : struct, INumber<T>
{
    private readonly ReadOnlyMemory<T> _sequence;

    public ReadOnlyVector(T[] sequence)
        : this(new ReadOnlyMemory<T>(sequence))
    {
    }

    public ReadOnlyVector(T[] sequence, int start, int length)
        : this(new ReadOnlyMemory<T>(sequence, start, length))
    {
    }

    public ReadOnlyVector(ReadOnlyMemory<T> sequence)
    {
        _sequence = sequence;
    }

    public ReadOnlySpan<T> Sequence => _sequence.Span;

    public int Length => _sequence.Length;

    public T this[int index] => _sequence.Span[index];

    public override bool Equals(object? obj)
    {
        return obj is Vector<T> vector && Equals(vector);
    }

    public override int GetHashCode()
    {
        return _sequence.GetHashCode();
    }

    public bool Equals(ReadOnlyVector<T> other)
    {
        return _sequence.Equals(other._sequence);
    }

    public static bool operator ==(ReadOnlyVector<T> left, ReadOnlyVector<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ReadOnlyVector<T> left, ReadOnlyVector<T> right)
    {
        return !(left == right);
    }

    public Enumerator GetEnumerator()
    {
        return new(_sequence);
    }

#pragma warning disable CA1034 // Nested types should not be visible
    public ref struct Enumerator
#pragma warning restore CA1034 // Nested types should not be visible
    {
        private readonly ReadOnlyMemory<T> _sequence;
        private int _index;

        internal Enumerator(ReadOnlyMemory<T> sequence)
        {
            _sequence = sequence;
            _index = -1;
        }

        public T Current => _sequence.Span[_index];

        public bool MoveNext()
        {
            return ++_index < _sequence.Length;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
