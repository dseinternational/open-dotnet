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

    public abstract VectorValue GetVectorValue(int index);

    protected abstract ReadOnlySeries CreateReadOnly();

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

    protected abstract IReadOnlyValueLabelCollection GetReadOnlyValueLabelCollection();

    public abstract bool HasValueLabels { get; }

    protected abstract IReadOnlyCategorySet GetReadOnlyCategorySet();

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

    public static Series<T> Create<T>(
        Memory<T> vector,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        return new Series<T>(vector, name, categories, valueLabels);
    }

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

    // for collection initializers
    public static Series<T> Create<T>(ReadOnlySpan<T> vector)
        where T : IEquatable<T>
    {
        return Create(vector, null, null);
    }

    public static Series<T> Create<T>(
        ReadOnlySpan<T> vector,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        return new Series<T>(vector.ToArray(), name, categories, valueLabels);
    }

    public static Series<T> Create<T>(
        int length,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        return Create(new T[length], name, categories, valueLabels);
    }

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

    public static Series<T> CreateZeroes<T>(
        int length,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero, name, categories, valueLabels);
    }

    public static Series<T> CreateOnes<T>(
        int length,
        string? name = null,
        CategorySet<T>? categories = null,
        ValueLabelCollection<T>? valueLabels = null)
        where T : struct, INumber<T>
    {
        return Create(length, T.One, name, categories, valueLabels);
    }

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
