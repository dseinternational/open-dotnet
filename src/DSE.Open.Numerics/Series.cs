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

    [OverloadResolutionPriority(1)]
    public static Series<T> Create<T>(Vector<T> data, string? name = null, CategorySet<T>? categories = null)
        where T : IEquatable<T>
    {
        return new Series<T>(data, name, categories);
    }

    public static Series<T> Create<T>(Memory<T> data, string? name = null, CategorySet<T>? categories = null)
        where T : IEquatable<T>
    {
        return new Series<T>(data, name, categories);
    }

    public static Series<T> Create<T>(T[] data, string? name = null, CategorySet<T>? categories = null)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Series<T>(data, name, categories);
    }

    // for collection initializers
    public static Series<T> Create<T>(ReadOnlySpan<T> data)
        where T : IEquatable<T>
    {
        return Create(data, null, null);
    }

    public static Series<T> Create<T>(ReadOnlySpan<T> data, string? name = null, CategorySet<T>? categories = null)
        where T : IEquatable<T>
    {
        return new Series<T>(data.ToArray(), name, categories);
    }

    public static Series<T> Create<T>(int length, string? name = null, CategorySet<T>? categories = null)
        where T : IEquatable<T>
    {
        return Create(new T[length], name, categories);
    }

    public static Series<T> Create<T>(int length, T scalar, string? name = null, CategorySet<T>? categories = null)
        where T : IEquatable<T>
    {
        var data = GC.AllocateUninitializedArray<T>(length);
        data.AsSpan().Fill(scalar);
        return Create(data, name, categories);
    }

    public static Series<T> CreateZeroes<T>(int length, string? name = null, CategorySet<T>? categories = null)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero, name, categories);
    }

    public static Series<T> CreateOnes<T>(int length, string? name = null, CategorySet<T>? categories = null)
        where T : struct, INumber<T>
    {
        return Create(length, T.One, name, categories);
    }

    public static Series<T> CreateUninitialized<T>(int length, string? name = null, CategorySet<T>? categories = null)
        where T : IEquatable<T>
    {
        return Create(GC.AllocateUninitializedArray<T>(length), name, categories);
    }
}
