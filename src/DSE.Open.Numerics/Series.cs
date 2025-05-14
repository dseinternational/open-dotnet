// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
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
    protected internal Series(Vector vector, string? name, Index? index)
        : base(vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        Name = name;
        Index = index!;
    }

    /// <summary>
    /// Gets or sets a name for the series (optional).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Reserved for future use.
    /// </summary>
    public Index Index { get; }

    internal Vector Data => (Vector)BaseVector;

#pragma warning disable CA1033 // Interface methods should be callable by child types
    IReadOnlyVector IReadOnlySeries.Data => BaseVector;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    protected abstract ReadOnlySeries CreateReadOnly();

    public ReadOnlySeries AsReadOnly()
    {
        return CreateReadOnly();
    }

    IReadOnlySeries ISeries.AsReadOnly()
    {
        return AsReadOnly();
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
        return new Series<T>(data, null, null);
    }

    public static Series<T> Create<T>(string name, Memory<T> data)
        where T : IEquatable<T>
    {
        return new Series<T>(data, name, null);
    }

    /// <summary>
    /// Creates a vector from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Series<T> Create<T>(T[] data)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Series<T>(data, null, null);
    }

    /// <summary>
    /// Creates a vector from the given data with the specified name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Series<T> Create<T>(string name, T[] data)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Series<T>(data, name, null);
    }

    public static Series<T> Create<T>(ReadOnlySpan<T> data)
        where T : IEquatable<T>
    {
        return Create(data.ToArray());
    }

    public static Series<T> Create<T>(string name, ReadOnlySpan<T> data)
        where T : IEquatable<T>
    {
        return Create(name, data.ToArray());
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

    public static Series<T> Create<T>(string name, int length)
        where T : struct, INumber<T>
    {
        return Create(name, new T[length]);
    }
}
