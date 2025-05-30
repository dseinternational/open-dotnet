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

    protected abstract IReadOnlyValueLabelCollection GetReadOnlyValueLabelCollection();

    public abstract bool HasValueLabels { get; }

    protected abstract IReadOnlyCategorySet GetReadOnlyCategorySet();

    public abstract VectorValue GetVectorValue(int index);

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
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlySeries<T>(vector, name, categories, valueLabels);
    }

    public static ReadOnlySeries<T> Create<T>(
        ReadOnlyMemory<T> vector,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        if (vector.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlySeries<T>(vector, name, categories, valueLabels);
    }

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
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlySeries<T>(vector);
    }

    // for collection initializers
    public static ReadOnlySeries<T> Create<T>(ReadOnlySpan<T> vector)
        where T : IEquatable<T>
    {
        if (vector.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return Create(vector.ToArray());
    }

    public static ReadOnlySeries<T> Create<T>(
        ReadOnlySpan<T> vector,
        string? name = null,
        ReadOnlyCategorySet<T>? categories = null,
        ReadOnlyValueLabelCollection<T>? valueLabels = null)
        where T : IEquatable<T>
    {
        if (vector.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return Create(vector.ToArray(), name, categories, valueLabels);
    }

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
