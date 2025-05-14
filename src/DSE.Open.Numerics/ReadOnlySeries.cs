// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, fixed-length, contiguous sequence of read-only values 
///.
/// </summary>
[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public abstract class ReadOnlySeries : SeriesBase, IReadOnlySeries
{
    protected ReadOnlySeries(
        [NotNull] ReadOnlyVector vector,
        string? name = null,
        Index? index = null)
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

    internal ReadOnlyVector Data => (ReadOnlyVector)BaseVector;

#pragma warning disable CA1033 // Interface methods should be callable by child types
    IReadOnlyVector IReadOnlySeries.Data => BaseVector;
#pragma warning restore CA1033 // Interface methods should be callable by child types

    /// <summary>
    /// Creates a series from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static ReadOnlySeries<T> Create<T>(ReadOnlyMemory<T> vector)
        where T : IEquatable<T>
    {
        if (vector.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlySeries<T>(vector);
    }

    /// <summary>
    /// Creates a series from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static ReadOnlySeries<T> Create<T>(T[] array)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(array);

        if (array.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlySeries<T>(array);
    }

    public static ReadOnlySeries<T> Create<T>(ReadOnlySpan<T> data)
        where T : IEquatable<T>
    {
        if (data.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return Create(data.ToArray());
    }

    public static ReadOnlySeries<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new(data);
    }

    public static ReadOnlySeries<T> Create<T>(int length)
        where T : struct, INumber<T>
    {
        return new(new T[length]);
    }

    public static ReadOnlySeries<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero);
    }

    public static ReadOnlySeries<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.One);
    }
}
