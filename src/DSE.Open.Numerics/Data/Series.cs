// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics.Data;

/// <summary>
/// A serializable sequence of data with a label.
/// </summary>
[JsonConverter(typeof(SeriesJsonConverter))]
public abstract class Series
{
    protected Series(string? name, IDictionary<string, Variant>? annotations)
    {
        Name = name;
        Annotations = annotations;
    }

    public string? Name { get; set; }

    public IDictionary<string, Variant>? Annotations { get; }

    internal abstract Vector GetData();

    public static Series Create(Vector data)
    {
        return Create(null, data, null);
    }

    public static Series Create(string? name, Vector data)
    {
        return Create(name, data, null);
    }

    public static Series Create(string? name, Vector data, IDictionary<string, Variant>? annotations)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (data is NumericVector<int> intVector)
        {
            return CreateNumeric(name, intVector, annotations);
        }

        if (data is NumericVector<long> longVector)
        {
            return CreateNumeric(name, longVector, annotations);
        }

        if (data is NumericVector<short> shortVector)
        {
            return CreateNumeric(name, shortVector, annotations);
        }

        if (data is NumericVector<float> floatVector)
        {
            return CreateNumeric(name, floatVector, annotations);
        }

        if (data is NumericVector<double> doubleVector)
        {
            return CreateNumeric(name, doubleVector, annotations);
        }

        if (data is NumericVector<uint> uintVector)
        {
            return CreateNumeric(name, uintVector, annotations);
        }

        if (data is NumericVector<ulong> ulongVector)
        {
            return CreateNumeric(name, ulongVector, annotations);
        }

        if (data is NumericVector<ushort> ushortVector)
        {
            return CreateNumeric(name, ushortVector, annotations);
        }

        if (data is NumericVector<DateTime64> dateTime64Vector)
        {
            return CreateNumeric(name, dateTime64Vector, annotations);
        }

        if (data is Vector<string> stringVector)
        {
            return Create(name, stringVector, annotations);
        }

        if (data is Vector<char> charVector)
        {
            return Create(name, charVector, annotations);
        }

        if (data is Vector<bool> boolVector)
        {
            return Create(name, boolVector, annotations);
        }

        if (data is Vector<Guid> guidVector)
        {
            return Create(name, guidVector, annotations);
        }

        if (data is Vector<DateTime> DateTimeVector)
        {
            return Create(name, DateTimeVector, annotations);
        }

        if (data is Vector<DateTimeOffset> DateTimeOffsetVector)
        {
            return Create(name, DateTimeOffsetVector, annotations);
        }

        throw new InvalidOperationException($"Unsupported data type {data.GetType().Name}");
    }

    public static Series<T> Create<T>(Vector<T> data)
        where T : notnull
    {
        return Create(null, data);
    }

    public static Series<T> Create<T>(string? name, Vector<T> data)
        where T : notnull
    {
        return Create(name, data, null);
    }

    public static Series<T> Create<T>(string? name, Vector<T> data, IDictionary<string, Variant>? annotations)
        where T : notnull
    {
        return new Series<T>(name, data, annotations);
    }

    public static NumericSeries<T> CreateNumeric<T>(Memory<T> data)
        where T : struct, INumber<T>
    {
        return CreateNumeric(null, Vector.CreateNumeric(data), null);
    }

    public static NumericSeries<T> CreateNumeric<T>(NumericVector<T> data)
        where T : struct, INumber<T>
    {
        return CreateNumeric(null, data);
    }

    public static NumericSeries<T> CreateNumeric<T>(string? name, NumericVector<T> data)
        where T : struct, INumber<T>
    {
        return CreateNumeric(name, data, null);
    }

    public static NumericSeries<T> CreateNumeric<T>(string? name, NumericVector<T> data, IDictionary<string, Variant>? annotations)
        where T : struct, INumber<T>
    {
        return new NumericSeries<T>(name, data, annotations);
    }
}

[JsonConverter(typeof(SeriesJsonConverter))]
public abstract class Series<T, TVector> : Series
    where TVector : Vector<T>
{
    protected Series(string? name, TVector data, IDictionary<string, Variant>? annotations) : base(name, annotations)
    {
        ArgumentNullException.ThrowIfNull(data);
        Data = data;
    }

    public TVector Data { get; }

    internal override Vector GetData()
    {
        return Data;
    }
}

[JsonConverter(typeof(SeriesJsonConverter))]
public sealed class Series<T> : Series<T, Vector<T>>
{
    public Series(
        string? name,
        Vector<T> data,
        IDictionary<string, Variant>? annotations)
        : base(name, data, annotations)
    {
    }
}
