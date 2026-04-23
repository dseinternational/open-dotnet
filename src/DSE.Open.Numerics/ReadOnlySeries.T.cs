// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Numerics;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous, fixed-length sequence of read-only values of data type <typeparamref name="T"/>
///. Optionally named, labelled or categorised for use with
/// a <see cref="IReadOnlyDataFrame"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
[CollectionBuilder(typeof(ReadOnlySeries), nameof(Create))]
[JsonConverter(typeof(ReadOnlySeriesJsonConverter))]
public class ReadOnlySeries<T> : ReadOnlySeries, IReadOnlySeries<T>
    where T : IEquatable<T>
{
    public static readonly ReadOnlySeries<T> Empty = new([]);

    private readonly ReadOnlyVector<T> _vector;
    private ReadOnlyCategorySet<T>? _categories;
    private ReadOnlyValueLabelCollection<T>? _valueLabels;

    public ReadOnlySeries(
        [NotNull] ReadOnlyVector<T> vector,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        : base(vector, name)
    {
        _vector = vector;
        _categories = categories;
        _valueLabels = valueLabels;

        if (_categories is not null)
        {
            NumericsArgumentException.ThrowIfNotInSet(vector, _categories);
        }
    }

    /// <summary>
    /// "Trusted" ctor used by <see cref="Slice(int)"/> and <see cref="Slice(int, int)"/>.
    /// The sliced vector inherits metadata from a source that already validated its
    /// full vector against <paramref name="categories"/>, so each element of the
    /// sliced sub-vector is necessarily in the set. Skipping re-validation avoids an
    /// O(length) check on every slice and, crucially, prevents slicing from throwing
    /// in the presence of externally-mutated category sets — consistent with the
    /// documented "no runtime enforcement" contract.
    /// </summary>
    private ReadOnlySeries(
        ReadOnlyVector<T> vector,
        string? name,
        ReadOnlyCategorySet<T>? categories,
        ReadOnlyValueLabelCollection<T>? valueLabels,
        bool skipCategoryValidation)
        : base(vector, name)
    {
        _vector = vector;
        _categories = categories;
        _valueLabels = valueLabels;
        _ = skipCategoryValidation;
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _vector[index];
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types
    int IReadOnlyCollection<T>.Count => Length;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    public new ReadOnlyVector<T> Vector => _vector;

    [MemberNotNullWhen(true, nameof(_categories))]
    public override bool IsCategorical => _categories is not null && !_categories.IsEmpty;

    public ReadOnlyCategorySet<T> Categories => _categories ??= [];

    public ReadOnlyValueLabelCollection<T> ValueLabels => _valueLabels ??= [];

    public override bool HasValueLabels => _valueLabels is not null && _valueLabels.Count > 0;

    IReadOnlyValueLabelCollection<T> IReadOnlySeries<T>.ValueLabels => ValueLabels;

    IReadOnlyValueLabelCollection IReadOnlySeries.ValueLabels => ValueLabels;

    protected override IReadOnlyCategorySet GetReadOnlyCategorySet()
    {
        return Categories;
    }

    protected override IReadOnlyValueLabelCollection GetReadOnlyValueLabelCollection()
    {
        return ValueLabels;
    }

    public override VectorValue GetVectorValue(int index)
    {
        return VectorValue.FromValue(this[index]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsReadOnlySpan()
    {
        return _vector.AsSpan();
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

    public bool Equals(ReadOnlySeries<T>? other)
    {
        return other is not null && Equals(other.AsReadOnlySpan());
    }

    public bool Equals(IReadOnlySeries<T>? other)
    {
        return other is not null && Equals(other.AsReadOnlySpan());
    }

    public bool Equals(ReadOnlySpan<T> other)
    {
        return AsReadOnlySpan().SequenceEqual(other);
    }

    public ReadOnlyMemoryEnumerator<T> GetEnumerator()
    {
        return _vector.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return MemoryMarshal.ToEnumerable((ReadOnlyMemory<T>)_vector).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    /// <summary>
    /// Returns a new <see cref="ReadOnlySeries{T}"/> that wraps the same underlying
    /// vector as this instance but with <see cref="IReadOnlySeries.Name"/> overridden.
    /// Categories and value labels (if any) are carried through by reference.
    /// </summary>
    /// <param name="name">The name to assign to the new series. Pass <see langword="null"/> to clear.</param>
    public ReadOnlySeries<T> WithName(string? name)
    {
        // Only pass _categories through if the series is actually categorical. The
        // lazy Categories getter creates an empty ReadOnlyCategorySet on first
        // access, which would otherwise trip the non-empty-set validation in the ctor.
        return new ReadOnlySeries<T>(_vector, name, IsCategorical ? _categories : null, _valueLabels);
    }

    /// <summary>
    /// Returns a new <see cref="ReadOnlySeries{T}"/> that wraps the same underlying
    /// vector as this instance but with <see cref="Categories"/> overridden. Name and
    /// value labels (if any) are carried through.
    /// </summary>
    /// <param name="categories">The read-only category set to attach. Pass
    /// <see langword="null"/> to remove categorical constraints.</param>
    /// <remarks>
    /// The supplied <paramref name="categories"/> is retained by reference by the
    /// returned series; external mutation of the set is visible to the returned
    /// series, and to this series only if it already references the same instance.
    /// Elements of the vector are validated against <paramref name="categories"/> at
    /// construction time; subsequent mutations of the set are not re-validated.
    /// </remarks>
    public ReadOnlySeries<T> WithCategories(ReadOnlyCategorySet<T>? categories)
    {
        return new ReadOnlySeries<T>(_vector, Name, categories, _valueLabels);
    }

    /// <summary>
    /// Returns a new <see cref="ReadOnlySeries{T}"/> that wraps the same underlying
    /// vector as this instance but with <see cref="ValueLabels"/> overridden. Name and
    /// categories (if any) are carried through.
    /// </summary>
    /// <param name="valueLabels">The read-only value-label collection to attach. Pass
    /// <see langword="null"/> to clear.</param>
    /// <remarks>
    /// The supplied <paramref name="valueLabels"/> is retained by reference by the
    /// returned series; external mutation of the collection is visible to the returned
    /// series and to any other series that shares the same collection instance.
    /// </remarks>
    public ReadOnlySeries<T> WithValueLabels(ReadOnlyValueLabelCollection<T>? valueLabels)
    {
        // Only pass _categories through if the series is actually categorical. The
        // lazy Categories getter creates an empty ReadOnlyCategorySet on first
        // access, which would otherwise trip the non-empty-set validation in the ctor.
        return new ReadOnlySeries<T>(_vector, Name, IsCategorical ? _categories : null, valueLabels);
    }

    /// <summary>
    /// Returns a slice of this series starting at <paramref name="start"/>. The returned
    /// slice preserves the source's <see cref="IReadOnlySeries.Name"/>,
    /// <see cref="Categories"/> and <see cref="ValueLabels"/>. Categories and value
    /// labels are <b>shared by reference</b> with the source; call
    /// <see cref="Slice(int, int, bool)"/> with <c>copy: true</c> to isolate them.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySeries<T> Slice(int start)
    {
        return new ReadOnlySeries<T>(
            _vector[start..], Name, _categories, _valueLabels, skipCategoryValidation: true);
    }

    /// <summary>
    /// Returns a slice of this series. The returned slice preserves the source's
    /// <see cref="IReadOnlySeries.Name"/>, <see cref="Categories"/> and
    /// <see cref="ValueLabels"/>. Categories and value labels are <b>shared by
    /// reference</b> with the source; call <see cref="Slice(int, int, bool)"/> with
    /// <c>copy: true</c> to isolate them.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySeries<T> Slice(int start, int length)
    {
        return new ReadOnlySeries<T>(
            _vector.Slice(start, length), Name, _categories, _valueLabels, skipCategoryValidation: true);
    }

    /// <summary>
    /// Returns a slice of this series.
    /// </summary>
    /// <param name="start">Start index of the slice.</param>
    /// <param name="length">Length of the slice.</param>
    /// <param name="copy">
    /// When <see langword="false"/>, the returned slice shares its
    /// <see cref="Categories"/> and <see cref="ValueLabels"/> references with the
    /// source (matching <see cref="Slice(int, int)"/>). When <see langword="true"/>,
    /// the slice receives deep copies.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySeries<T> Slice(int start, int length, bool copy)
    {
        if (!copy)
        {
            return Slice(start, length);
        }

        // Cast to IEnumerable<T> forces the enumerating copy ctor; passing a
        // ReadOnlyCategorySet<T> directly would select the IReadOnlySet<T> overload,
        // which aliases the underlying storage instead of copying.
        return new ReadOnlySeries<T>(
            _vector.Slice(start, length),
            Name,
            _categories is null ? null : new ReadOnlyCategorySet<T>((IEnumerable<T>)_categories),
            _valueLabels is null ? null : new ReadOnlyValueLabelCollection<T>(_valueLabels));
    }

    IReadOnlySeries<T> IReadOnlySeries<T>.Slice(int start, int length)
    {
        return Slice(start, length);
    }

    public static bool operator ==(ReadOnlySeries<T>? left, ReadOnlySeries<T>? right)
    {
        return left is not null && (right is null || left.Equals(right));
    }

    public static bool operator !=(ReadOnlySeries<T>? left, ReadOnlySeries<T>? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Creates a mutable copy of this read-only series.
    /// </summary>
    public Series<T> ToSeries()
    {
        return new Series<T>(
            Vector.ToVector(),
            Name,
            Categories.ToCategorySet(),
            ValueLabels.ToValueLabelCollection());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlySeries<T>(ReadOnlyMemory<T> vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlySeries<T>(T[] vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates",
        Justification = "By design")]
    public static implicit operator ReadOnlyMemory<T>(ReadOnlySeries<T> vector)
    {
        return vector is not null ? (ReadOnlyMemory<T>)vector._vector : default;
    }
}
