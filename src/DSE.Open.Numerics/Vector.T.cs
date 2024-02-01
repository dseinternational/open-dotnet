// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

[CollectionBuilder(typeof(Vector), nameof(Vector.Create))]
public readonly struct Vector<T> : IVector<T, Vector<T>>, IEquatable<Vector<T>>
    where T : struct, INumber<T>
{
    private readonly Memory<T> _sequence;

    public Vector(T[] sequence) : this(new Memory<T>(sequence))
    {
    }

    public Vector(T[] sequence, int start, int length) : this(new Memory<T>(sequence, start, length))
    {
    }

    public Vector(Memory<T> sequence)
    {
        _sequence = sequence;
    }

#pragma warning disable CA1000 // Do not declare static members on generic types

    public static Vector<T> Create(Span<T> sequence)
    {
        return new Vector<T>(sequence.ToArray());
    }

    public static Vector<T> Create(int length)
    {
        return new Vector<T>(new T[length]);
    }

#pragma warning restore CA1000 // Do not declare static members on generic types

    public Span<T> Sequence => _sequence.Span;

    public int Length => _sequence.Length;

    ReadOnlySpan<T> IReadOnlyVector<T, Vector<T>>.Sequence => Sequence;

    public T this[int index]
    {
        get => _sequence.Span[index];
        set => _sequence.Span[index] = value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector<T> vector && Equals(vector);
    }

    public override int GetHashCode()
    {
        return _sequence.GetHashCode();
    }

    public bool Equals(Vector<T> other)
    {
        return _sequence.Equals(other._sequence);
    }

    static Vector<T> IReadOnlyVector<T, Vector<T>>.Create(ReadOnlySpan<T> sequence)
    {
        // only option is to copy?
        throw new NotImplementedException();
    }

    public Enumerator GetEnumerator()
    {
        return new(_sequence);
    }

#pragma warning disable CA1034 // Nested types should not be visible
    public ref struct Enumerator
#pragma warning restore CA1034 // Nested types should not be visible
    {
        private readonly Memory<T> _sequence;
        private int _index;

        internal Enumerator(Memory<T> sequence)
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

    public static bool operator ==(Vector<T> left, Vector<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector<T> left, Vector<T> right)
    {
        return !(left == right);
    }
}
