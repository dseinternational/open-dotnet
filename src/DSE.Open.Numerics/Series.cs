// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Net.Http.Headers;
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

    protected abstract ReadOnlySeries CreateReadOnly();

    public ReadOnlySeries AsReadOnly()
    {
        return CreateReadOnly();
    }

    IReadOnlySeries ISeries.AsReadOnly()
    {
        return AsReadOnly();
    }

    internal static Series CreateUntyped(
        string? name,
        Vector data,
        Index? index,
        object? labels) // todo
    {
        ArgumentNullException.ThrowIfNull(data);

        return data.DataType switch
        {
            VectorDataType.Float64 => new Series<double>((Vector<double>)data, name, index, labels as ValueLabelCollection<double>),
            VectorDataType.Float32 => new Series<float>((Vector<float>)data, name, index, labels as ValueLabelCollection<float>),
            VectorDataType.Int64 => new Series<long>((Vector<long>)data, name, index, labels as ValueLabelCollection<long>),
            VectorDataType.UInt64 => new Series<ulong>((Vector<ulong>)data, name, index, labels as ValueLabelCollection<ulong>),
            VectorDataType.Int32 => new Series<int>((Vector<int>)data, name, index, labels as ValueLabelCollection<int>),
            VectorDataType.UInt32 => new Series<uint>((Vector<uint>)data, name, index, labels as ValueLabelCollection<uint>),
            VectorDataType.Int16 => new Series<short>((Vector<short>)data, name, index, labels as ValueLabelCollection<short>),
            VectorDataType.UInt16 => new Series<ushort>((Vector<ushort>)data, name, index, labels as ValueLabelCollection<ushort>),
            VectorDataType.Int8 => new Series<sbyte>((Vector<sbyte>)data, name, index, labels as ValueLabelCollection<sbyte>),
            VectorDataType.UInt8 => new Series<byte>((Vector<byte>)data, name, index, labels as ValueLabelCollection<byte>),
            VectorDataType.Int128 => new Series<Int128>((Vector<Int128>)data, name, index, labels as ValueLabelCollection<Int128>),
            VectorDataType.UInt128 => new Series<UInt128>((Vector<UInt128>)data, name, index, labels as ValueLabelCollection<UInt128>),
            VectorDataType.DateTime64 => new Series<DateTime64>((Vector<DateTime64>)data, name, index, labels as ValueLabelCollection<DateTime64>),
            VectorDataType.DateTime => new Series<DateTime>((Vector<DateTime>)data, name, index, labels as ValueLabelCollection<DateTime>),
            VectorDataType.DateTimeOffset => new Series<DateTimeOffset>((Vector<DateTimeOffset>)data, name, index, labels as ValueLabelCollection<DateTimeOffset>),
            VectorDataType.Uuid => new Series<Guid>((Vector<Guid>)data, name, index, labels as ValueLabelCollection<Guid>),
            VectorDataType.Bool => new Series<bool>((Vector<bool>)data, name, index, labels as ValueLabelCollection<bool>),
            VectorDataType.Char => new Series<char>((Vector<char>)data, name, index, labels as ValueLabelCollection<char>),
            VectorDataType.String => new Series<string>((Vector<string>)data, name, index, labels as ValueLabelCollection<string>),
            _ => throw new InvalidOperationException("Unsupported data type: " + data.DataType),
        };
    }

    [OverloadResolutionPriority(1)]
    public static Series<T> Create<T>(Vector<T> data)
        where T : IEquatable<T>
    {
        return new Series<T>(data, null, null, null);
    }

    [OverloadResolutionPriority(1)]
    public static Series<T> Create<T>(string name, Vector<T> data)
        where T : IEquatable<T>
    {
        return new Series<T>(data, name, null, null);
    }

    [OverloadResolutionPriority(1)]
    public static Series<T> Create<T>(string name, Vector<T> data, ValueLabelCollection<T> labels)
        where T : IEquatable<T>
    {
        return new Series<T>(data, name, null, labels);
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
        return new Series<T>(data, null, null, null);
    }

    public static Series<T> Create<T>(string name, Memory<T> data)
        where T : IEquatable<T>
    {
        return new Series<T>(data, name, null, null);
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
        return new Series<T>(data, null, null, null);
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
        return new Series<T>(data, name, null, null);
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
