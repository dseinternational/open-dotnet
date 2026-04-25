// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, fixed-length, contiguous sequence of read-only values
///.
/// </summary>
[JsonConverter(typeof(ReadOnlySeriesJsonConverter))]
public abstract class ReadOnlySeries : SeriesBase, IReadOnlySeries
{
    /// <summary>
    /// Initializes a read-only series wrapping <paramref name="vector"/> with optional name.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    protected ReadOnlySeries([NotNull] ReadOnlyVector vector, string? name = null)
        : base(vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        Name = name;
    }

    /// <summary>
    /// Gets a name for the series (optional).
    /// </summary>
    public string? Name { get; }

#pragma warning disable CA1033 // Interface methods should be callable by child types

    IReadOnlyCategorySet IReadOnlySeries.Categories => GetReadOnlyCategorySet();

    IReadOnlyValueLabelCollection IReadOnlySeries.ValueLabels => GetReadOnlyValueLabelCollection();

#pragma warning restore CA1033 // Interface methods should be callable by child types

    /// <summary>
    /// Returns the value-label collection as a read-only view.
    /// </summary>
    protected abstract IReadOnlyValueLabelCollection GetReadOnlyValueLabelCollection();

    /// <summary>
    /// Gets <see langword="true"/> when at least one value-label is attached to the series.
    /// </summary>
    public abstract bool HasValueLabels { get; }

    /// <summary>
    /// Returns the category set as a read-only view.
    /// </summary>
    protected abstract IReadOnlyCategorySet GetReadOnlyCategorySet();

    /// <summary>
    /// Returns the element at <paramref name="index"/> boxed into a
    /// type-erased <see cref="VectorValue"/>.
    /// </summary>
    public abstract VectorValue GetVectorValue(int index);

    /// <summary>
    /// Creates a read-only series wrapping <paramref name="vector"/>, with
    /// optional name, category set and value-label collection.
    /// </summary>
    [OverloadResolutionPriority(1)]
    public static ReadOnlySeries<T> Create<T>(
        ReadOnlyVector<T> vector,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);

        if (vector.Length == 0)
        {
            return ReadOnlySeries<T>.Empty;
        }

        return new ReadOnlySeries<T>(vector, name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a read-only series wrapping <paramref name="vector"/>, with
    /// optional name, category set and value-label collection.
    /// </summary>
    public static ReadOnlySeries<T> Create<T>(
        ReadOnlyMemory<T> vector,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        if (vector.Length == 0)
        {
            return ReadOnlySeries<T>.Empty;
        }

        return new ReadOnlySeries<T>(vector, name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a read-only series wrapping <paramref name="vector"/> by
    /// reference, with optional name, category set and value-label collection.
    /// The caller must not mutate <paramref name="vector"/> while the series
    /// is in use.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    public static ReadOnlySeries<T> Create<T>(
        T[] vector,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);

        if (vector.Length == 0)
        {
            return ReadOnlySeries<T>.Empty;
        }

        return new ReadOnlySeries<T>(vector);
    }

    /// <summary>
    /// Collection-initializer-friendly overload that copies <paramref name="vector"/>
    /// into a fresh array.
    /// </summary>
    public static ReadOnlySeries<T> Create<T>(ReadOnlySpan<T> vector)
        where T : IEquatable<T>
    {
        if (vector.Length == 0)
        {
            return ReadOnlySeries<T>.Empty;
        }

        return Create(vector.ToArray());
    }

    /// <summary>
    /// Creates a read-only series by copying <paramref name="vector"/> into a
    /// fresh array, with optional name, category set and value-label collection.
    /// </summary>
    public static ReadOnlySeries<T> Create<T>(
        ReadOnlySpan<T> vector,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        if (vector.Length == 0)
        {
            return ReadOnlySeries<T>.Empty;
        }

        return Create(vector.ToArray(), name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a read-only series of the given <paramref name="length"/> with
    /// every element initialised to <paramref name="scalar"/>.
    /// </summary>
    public static ReadOnlySeries<T> Create<T>(
        int length,
        T scalar,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        var vector = new T[length];
        vector.AsSpan().Fill(scalar);
        return Create(vector, name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a read-only series of the given <paramref name="length"/>
    /// initialised to <see cref="INumberBase{TSelf}.Zero"/>.
    /// </summary>
    public static ReadOnlySeries<T> CreateZeroes<T>(
        int length,
        T scalar,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero, name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a read-only series of the given <paramref name="length"/>
    /// initialised to <see cref="INumberBase{TSelf}.One"/>.
    /// </summary>
    public static ReadOnlySeries<T> CreateOnes<T>(
        int length,
        T scalar,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        where T : struct, INumber<T>
    {
        return Create(length, T.One, name, categories, valueLabels);
    }
}
