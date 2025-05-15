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
public class Series<T> : Series, ISeries<T>, IReadOnlySeries<T>, IEquatable<Series<T>>, IEquatable<ReadOnlySeries<T>>
    where T : IEquatable<T>
{
#pragma warning disable IDE1006 // Naming Styles
    internal static readonly Series<T> Empty = new([], null, null);
#pragma warning restore IDE1006 // Naming Styles

    private readonly Vector<T> _vector;
    private readonly DataLabelCollection<T> _labels;

    internal Series(
        [NotNull] Vector<T> vector,
        string? name,
        Index? index,
        DataLabelCollection<T>? labels = null)
        : base(vector, name, index)
    {
        _vector = vector;
        _labels = labels ?? [];
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _vector[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _vector[index] = value;
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public DataLabelCollection<T> Labels => _labels;

    IDataLabelCollection<T> ISeries<T>.Labels => Labels;

    IReadOnlyDataLabelCollection<T> IReadOnlySeries<T>.Labels => Labels;

    public IEnumerable<string> LabelledValues => new SeriesLabelEnumerable<T>(this);

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
        return new ReadOnlySeries<T>(_vector, Name);
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
        return new Series<T>(_vector[start..], Name, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Series<T> Slice(int start, int length)
    {
        return new Series<T>(_vector.Slice(start, length), Name, null);
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

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Series<T>(Memory<T> vector)
    {
        return new(vector, null, null);
    }

    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Series<T>(T[] vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        return new(vector, null, null);
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
