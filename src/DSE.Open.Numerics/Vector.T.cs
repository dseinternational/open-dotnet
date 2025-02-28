// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using DSE.Open.Numerics.Serialization;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable sequence of values of known length of type <typeparamref name="T"/> 
/// with value equality semantics.
/// </summary>
/// <typeparam name="T"></typeparam>
[CollectionBuilder(typeof(Vector), nameof(Create))]
[JsonConverter(typeof(VectorJsonConverter))]
public class Vector<T> : Vector, IEquatable<Vector<T>>
{
    public static readonly Vector<T> Empty = new([]);

    internal Vector(T[] data) : this(new Memory<T>(data))
    {
    }

    internal Vector(T[] data, int start, int length) : this(new Memory<T>(data, start, length))
    {
    }

    internal Vector(Memory<T> data) : base(VectorDataTypeHelper.GetVectorDataType<T>(), typeof(T), data.Length)
    {
        Memory = data;
    }

    protected Memory<T> Memory { get; }

    public Span<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Memory.Span;
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Memory.Span[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Memory.Span[index] = value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector<T> vector && Equals(vector);
    }

    public MemoryEnumerator<T> GetEnumerator()
    {
        return Memory.GetEnumerator();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        var hash = new HashCode();

        foreach (var i in this)
        {
            hash.Add(i);
        }

        return hash.ToHashCode();
    }

    public bool Equals(Vector<T>? other)
    {
        return other is not null && Equals(other.Memory.Span);
    }

    public bool Equals(ReadOnlySpan<T> other)
    {
        return Memory.Span.SequenceEqual(other);
    }

    public T[] ToArray()
    {
        return Memory.ToArray();
    }

    public static bool operator ==(Vector<T> left, Vector<T> right)
    {
        return left is not null && (right is null || left.Equals(right));
    }

    public static bool operator !=(Vector<T> left, Vector<T> right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator Vector<T>(T[] vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator Vector<T>(Memory<T> vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator Memory<T>(Vector<T> vector)
    {
        return vector is not null ? vector.Memory : default;
    }
}
