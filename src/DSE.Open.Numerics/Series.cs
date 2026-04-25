// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type.
/// Optionally named, labelled or categorised for use with a <see cref="DataFrame"/>.
/// </summary>
[JsonConverter(typeof(SeriesJsonConverter))]
public abstract class Series : SeriesBase, ISeries
{
    /// <summary>
    /// Initializes a series wrapping <paramref name="vector"/> with optional name.
    /// </summary>
    /// <param name="vector">The backing vector. Must not be <see langword="null"/>.</param>
    /// <param name="name">The optional series name.</param>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    protected internal Series([NotNull] Vector vector, string? name)
        : base(vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        Name = name;
    }

    /// <summary>
    /// Gets or sets a name for the series (optional).
    /// </summary>
    public string? Name { get; set; }

    internal Vector Data => (Vector)BaseVector;

    /// <summary>
    /// Returns the element at <paramref name="index"/> boxed into a
    /// type-erased <see cref="VectorValue"/>.
    /// </summary>
    public abstract VectorValue GetVectorValue(int index);

    /// <summary>
    /// Returns a read-only view of this series. Implemented by derived types to
    /// produce a strongly-typed <see cref="ReadOnlySeries{T}"/>.
    /// </summary>
    protected abstract ReadOnlySeries CreateReadOnly();

    /// <summary>
    /// Returns a read-only view of this series. Categories and value-label
    /// collections are aliased — see derived <see cref="Series{T}.AsReadOnly"/>
    /// for typed semantics.
    /// </summary>
    public ReadOnlySeries AsReadOnly()
    {
        return CreateReadOnly();
    }

    IReadOnlySeries ISeries.AsReadOnly()
    {
        return AsReadOnly();
    }

#pragma warning disable CA1033 // Interface methods should be callable by child types

    IReadOnlyCategorySet IReadOnlySeries.Categories => GetReadOnlyCategorySet();

    IReadOnlyValueLabelCollection IReadOnlySeries.ValueLabels => GetReadOnlyValueLabelCollection();

#pragma warning restore CA1033 // Interface methods should be callable by child types

    /// <summary>
    /// Returns the value-label collection as a read-only view. Implemented by
    /// derived types.
    /// </summary>
    protected abstract IReadOnlyValueLabelCollection GetReadOnlyValueLabelCollection();

    /// <summary>
    /// Gets <see langword="true"/> when at least one value-label is attached
    /// to the series.
    /// </summary>
    public abstract bool HasValueLabels { get; }

    /// <summary>
    /// Returns the category set as a read-only view. Implemented by derived types.
    /// </summary>
    protected abstract IReadOnlyCategorySet GetReadOnlyCategorySet();

    /// <summary>
    /// Creates a series wrapping the supplied <paramref name="vector"/> with
    /// optional name, category set and value-label collection.
    /// </summary>
    [OverloadResolutionPriority(1)]
    public static Series<T> Create<T>(
        Vector<T> vector,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        return new Series<T>(vector, name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a series backed by <paramref name="vector"/> (taken by reference)
    /// with optional name, category set and value-label collection.
    /// </summary>
    public static Series<T> Create<T>(
        Memory<T> vector,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        return new Series<T>(vector, name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a series backed by <paramref name="vector"/> (taken by reference)
    /// with optional name, category set and value-label collection.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    public static Series<T> Create<T>(
        T[] vector,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(vector);
        return new Series<T>(vector, name, categories, valueLabels);
    }

    /// <summary>
    /// Collection-initializer-friendly overload that copies <paramref name="vector"/>
    /// into a fresh array.
    /// </summary>
    public static Series<T> Create<T>(ReadOnlySpan<T> vector)
        where T : IEquatable<T>
    {
        return Create(vector, null, null);
    }

    /// <summary>
    /// Creates a series by copying <paramref name="vector"/> into a fresh
    /// array, with optional name, category set and value-label collection.
    /// </summary>
    public static Series<T> Create<T>(
        ReadOnlySpan<T> vector,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        return new Series<T>(vector.ToArray(), name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a zero-initialised series of the given <paramref name="length"/>
    /// with optional name, category set and value-label collection.
    /// </summary>
    public static Series<T> Create<T>(
        int length,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        return Create(new T[length], name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a series of the given <paramref name="length"/> with all
    /// elements initialised to <paramref name="scalar"/>.
    /// </summary>
    public static Series<T> Create<T>(
        int length,
        T scalar,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        var vector = GC.AllocateUninitializedArray<T>(length);
        vector.AsSpan().Fill(scalar);
        return Create(vector, name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a series of the given <paramref name="length"/> with all
    /// elements initialised to <see cref="INumberBase{TSelf}.Zero"/>.
    /// </summary>
    public static Series<T> CreateZeroes<T>(
        int length,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero, name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a series of the given <paramref name="length"/> with all
    /// elements initialised to <see cref="INumberBase{TSelf}.One"/>.
    /// </summary>
    public static Series<T> CreateOnes<T>(
        int length,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : struct, INumber<T>
    {
        return Create(length, T.One, name, categories, valueLabels);
    }

    /// <summary>
    /// Creates a series of the given <paramref name="length"/> backed by an
    /// uninitialised array — contents are unspecified until written. Useful as
    /// a destination for primitive operations that overwrite every element.
    /// </summary>
    public static Series<T> CreateUninitialized<T>(
        int length,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        return Create(GC.AllocateUninitializedArray<T>(length), name, categories);
    }
}
