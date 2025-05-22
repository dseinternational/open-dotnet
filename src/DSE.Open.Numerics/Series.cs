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
    public static Series<T> Create<T>(Vector<T> data)
        where T : IEquatable<T>
    {
        return new Series<T>(data);
    }

    [OverloadResolutionPriority(1)]
    public static Series<T> Create<T>(Vector<T> data, string name)
        where T : IEquatable<T>
    {
        return new Series<T>(data, name);
    }

    /// <summary>
    /// Creates a vector from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Series<T> Create<T>(Memory<T> data)
        where T : IEquatable<T>
    {
        return new Series<T>(data);
    }

    public static Series<T> Create<T>(Memory<T> data, string name)
        where T : IEquatable<T>
    {
        return new Series<T>(data, name);
    }

    public static Series<T> Create<T>(T[] data, string name)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Series<T>(data, name);
    }

    public static Series<T> Create<T>(ReadOnlySpan<T> data)
        where T : IEquatable<T>
    {
        return new Series<T>(data.ToArray());
    }

    public static Series<T> Create<T>(ReadOnlySpan<T> data, string name)
        where T : IEquatable<T>
    {
        return new Series<T>(data.ToArray(), name);
    }

    public static Series<T> Create<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(new T[length]);
    }

    public static Series<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return Create(data);
    }

    public static Series<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero);
    }

    public static Series<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.One);
    }

    public static Series<T> Create<T>(int length, string name)
        where T : struct, INumber<T>
    {
        return Create(new T[length], name);
    }
}
