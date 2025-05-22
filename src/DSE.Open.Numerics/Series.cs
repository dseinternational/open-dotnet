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

    internal static Series CreateUntyped(
        string? name,
        Vector data,
        ICategorySet? categorySet) // todo
    {
        ArgumentNullException.ThrowIfNull(data);

        return data.DataType switch
        {
            VectorDataType.Float64 => new Series<double>((Vector<double>)data, name, categorySet as CategorySet<double>),
            VectorDataType.Float32 => new Series<float>((Vector<float>)data, name, categorySet as CategorySet<float>),
            VectorDataType.Float16 => new Series<Half>((Vector<Half>)data, name, categorySet as CategorySet<Half>),
            VectorDataType.Int64 => new Series<long>((Vector<long>)data, name, categorySet as CategorySet<long>),
            VectorDataType.UInt64 => new Series<ulong>((Vector<ulong>)data, name, categorySet as CategorySet<ulong>),
            VectorDataType.Int32 => new Series<int>((Vector<int>)data, name, categorySet as CategorySet<int>),
            VectorDataType.UInt32 => new Series<uint>((Vector<uint>)data, name, categorySet as CategorySet<uint>),
            VectorDataType.Int16 => new Series<short>((Vector<short>)data, name, categorySet as CategorySet<short>),
            VectorDataType.UInt16 => new Series<ushort>((Vector<ushort>)data, name, categorySet as CategorySet<ushort>),
            VectorDataType.Int8 => new Series<sbyte>((Vector<sbyte>)data, name, categorySet as CategorySet<sbyte>),
            VectorDataType.UInt8 => new Series<byte>((Vector<byte>)data, name, categorySet as CategorySet<byte>),
            VectorDataType.DateTime64 => new Series<DateTime64>((Vector<DateTime64>)data, name, categorySet as CategorySet<DateTime64>),
            VectorDataType.DateTime => new Series<DateTime>((Vector<DateTime>)data, name, categorySet as CategorySet<DateTime>),
            VectorDataType.DateTimeOffset => new Series<DateTimeOffset>((Vector<DateTimeOffset>)data, name, categorySet as CategorySet<DateTimeOffset>),
            VectorDataType.Bool => new Series<bool>((Vector<bool>)data, name, categorySet as CategorySet<bool>),
            VectorDataType.Char => new Series<char>((Vector<char>)data, name, categorySet as CategorySet<char>),
            VectorDataType.String => new Series<string>((Vector<string>)data, name, categorySet as CategorySet<string>),
            VectorDataType.NaFloat64 => throw new NotImplementedException(),
            VectorDataType.NaFloat32 => throw new NotImplementedException(),
            VectorDataType.NaFloat16 => throw new NotImplementedException(),
            VectorDataType.NaInt64 => throw new NotImplementedException(),
            VectorDataType.NaUInt64 => throw new NotImplementedException(),
            VectorDataType.NaInt32 => throw new NotImplementedException(),
            VectorDataType.NaUInt32 => throw new NotImplementedException(),
            VectorDataType.NaInt16 => throw new NotImplementedException(),
            VectorDataType.NaUInt16 => throw new NotImplementedException(),
            VectorDataType.NaInt8 => throw new NotImplementedException(),
            VectorDataType.NaUInt8 => throw new NotImplementedException(),
            VectorDataType.NaDateTime64 => throw new NotImplementedException(),
            VectorDataType.NaDateTime => throw new NotImplementedException(),
            VectorDataType.NaDateTimeOffset => throw new NotImplementedException(),
            VectorDataType.NaBool => throw new NotImplementedException(),
            VectorDataType.NaChar => throw new NotImplementedException(),
            VectorDataType.NaString => throw new NotImplementedException(),
            _ => throw new InvalidOperationException("Unsupported data type: " + data.DataType),
        };
    }

    [OverloadResolutionPriority(1)]
    public static Series<T> Create<T>(Vector<T> data)
        where T : IEquatable<T>
    {
        return new Series<T>(data);
    }

    [OverloadResolutionPriority(1)]
    public static Series<T> Create<T>(string name, Vector<T> data)
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

    public static Series<T> Create<T>(string name, Memory<T> data)
        where T : IEquatable<T>
    {
        return new Series<T>(data, name);
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
        return new Series<T>(data, name);
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
