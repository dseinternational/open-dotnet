// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable sequence of values of known length of type <typeparamref name="T"/>
///. Optionally named, labelled or categorised for use with
/// a <see cref="DataFrame"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
[CollectionBuilder(typeof(Series), nameof(Create))]
[JsonConverter(typeof(SeriesJsonConverter))]
public class Series<T>
    : Series,
      ISeries<T>,
      IEquatable<Series<T>>,
      IEquatable<ReadOnlySeries<T>>
    where T : IEquatable<T>
{
#pragma warning disable IDE1006 // Naming Styles
    internal static readonly Series<T> Empty = new([]);
#pragma warning restore IDE1006 // Naming Styles

    private readonly Vector<T> _vector;
    private CategorySet<T>? _categories;

    public Series(int length) : this(new Vector<T>(length))
    {
    }

    public Series([NotNull] Vector<T> vector, string? name = null, CategorySet<T>? categories = null)
        : base(vector, name)
    {
        _vector = vector;
        _categories = categories;

        if (_categories is not null)
        {
            NumericsArgumentException.ThrowIfNotInSet(vector, _categories);
        }
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _vector[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (IsCategorical)
            {
                NumericsArgumentException.ThrowIfNotInSet(value, _categories);
            }

            _vector[index] = value;
        }
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public new ReadOnlyVector<T> Vector => _vector;

    [MemberNotNullWhen(true, nameof(_categories))]
    public override bool IsCategorical => _categories is not null && !_categories.IsEmpty;

    public CategorySet<T> Categories => _categories ??= [];

    public override VectorValue GetVectorValue(int index)
    {
        return VectorValue.FromValue(this[index]);
    }

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan()
    {
        return _vector.AsSpan();
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    ReadOnlySpan<T> IReadOnlySeries<T>.AsReadOnlySpan()
#pragma warning restore CA1033 // Interface methods should be callable by child types
    {
        return AsSpan();
    }

    public override bool Equals(object? obj)
    {
        return obj is Series<T> vector && Equals(vector);
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

    public new ReadOnlySeries<T> AsReadOnly()
    {
        return new ReadOnlySeries<T>(_vector, Name, _categories); // todo
    }

    IReadOnlySeries<T> ISeries<T>.AsReadOnly()
    {
        return AsReadOnly();
    }

    protected override ReadOnlySeries CreateReadOnly()
    {
        return AsReadOnly();
    }

    public bool Equals(Series<T>? other)
    {
        return other is not null && Name == other.Name && AsSpan().SequenceEqual(other.AsSpan());
    }

    public bool Equals(ReadOnlySeries<T>? other)
    {
        return other is not null && Name == other.Name && AsSpan().SequenceEqual(other.AsReadOnlySpan());
    }

    bool IEquatable<IReadOnlySeries<T>?>.Equals(IReadOnlySeries<T>? other)
    {
        return other is not null && Name == other.Name && AsSpan().SequenceEqual(other.AsReadOnlySpan());
    }

    public bool SequenceEqual(ReadOnlySpan<T> other)
    {
        return AsSpan().SequenceEqual(other);
    }

    public MemoryEnumerator<T> GetEnumerator()
    {
        return _vector.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return MemoryMarshal.ToEnumerable((ReadOnlyMemory<T>)(Memory<T>)_vector).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Series<T> Slice(int start)
    {
        return new Series<T>(_vector[start..], Name); // todo: slice index and labels
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Series<T> Slice(int start, int length)
    {
        return new Series<T>(_vector.Slice(start, length), Name); // todo: slice index and labels
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IReadOnlySeries<T> IReadOnlySeries<T>.Slice(int start, int length)
    {
        return Slice(start, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ISeries<T> ISeries<T>.Slice(int start, int length)
    {
        return Slice(start, length);
    }

    /// <summary>
    /// Copies the contents of the series to a new array.
    /// </summary>
    /// <returns></returns>
    public T[] ToArray()
    {
#pragma warning disable IDE0305 // Simplify collection initialization
        return _vector.ToArray();
#pragma warning restore IDE0305 // Simplify collection initialization
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Series<T>(Vector<T> vector)
    {
        return Create(vector);
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Series<T>(Memory<T> vector)
    {
        return Create(vector);
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Series<T>(T[] vector)
    {
        return Create(vector);
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Memory<T>(Series<T>? vector)
    {
        return vector is not null ? vector._vector : default;
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ReadOnlySeries<T>(Series<T> vector)
    {
        return vector is not null ? vector.AsReadOnly() : [];
    }
}
