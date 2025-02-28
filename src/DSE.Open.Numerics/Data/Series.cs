// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics.Data;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "dtype")]
[JsonDerivedType(typeof(Series<bool>), VectorDataTypeLabels.Bool)]
[JsonDerivedType(typeof(Series<char>), VectorDataTypeLabels.Char)]
[JsonDerivedType(typeof(Series<string>), VectorDataTypeLabels.String)]
[JsonDerivedType(typeof(Series<DateTime>), VectorDataTypeLabels.DateTime)]
[JsonDerivedType(typeof(Series<DateTimeOffset>), VectorDataTypeLabels.DateTimeOffset)]
[JsonDerivedType(typeof(Series<Guid>), VectorDataTypeLabels.Uuid)]
[JsonDerivedType(typeof(NumericSeries<byte>), VectorDataTypeLabels.UInt8)]
[JsonDerivedType(typeof(NumericSeries<sbyte>), VectorDataTypeLabels.Int8)]
[JsonDerivedType(typeof(NumericSeries<short>), VectorDataTypeLabels.Int16)]
[JsonDerivedType(typeof(NumericSeries<ushort>), VectorDataTypeLabels.UInt16)]
[JsonDerivedType(typeof(NumericSeries<int>), VectorDataTypeLabels.Int32)]
[JsonDerivedType(typeof(NumericSeries<uint>), VectorDataTypeLabels.UInt32)]
[JsonDerivedType(typeof(NumericSeries<long>), VectorDataTypeLabels.Int64)]
[JsonDerivedType(typeof(NumericSeries<ulong>), VectorDataTypeLabels.UInt64)]
[JsonDerivedType(typeof(NumericSeries<Int128>), VectorDataTypeLabels.Int128)]
[JsonDerivedType(typeof(NumericSeries<UInt128>), VectorDataTypeLabels.UInt128)]
[JsonDerivedType(typeof(NumericSeries<float>), VectorDataTypeLabels.Float32)]
[JsonDerivedType(typeof(NumericSeries<double>), VectorDataTypeLabels.Float64)]
[JsonDerivedType(typeof(NumericSeries<DateTime64>), VectorDataTypeLabels.DateTime64)]
public abstract class Series
{
    protected Series(string? name)
    {
        Name = name;
    }

    [JsonPropertyName("name")]
    [JsonPropertyOrder(-1000)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    public static Series<T> Create<T>(Vector<T> values)
        where T : notnull
    {
        return Create(null, values);
    }

    public static Series<T> Create<T>(string? name, Vector<T> values)
        where T : notnull
    {
        return Create(name, values, null);
    }

    public static Series<T> Create<T>(string? name, Vector<T> values, IDictionary<Variant, Variant>? references)
        where T : notnull
    {
        return new Series<T>(name, values, references);
    }

    public static NumericSeries<T> CreateNumeric<T>(Memory<T> values)
        where T : struct, INumber<T>
    {
        return CreateNumeric(null, Vector.CreateNumeric(values), null);
    }

    public static NumericSeries<T> CreateNumeric<T>(NumericVector<T> values)
        where T : struct, INumber<T>
    {
        return CreateNumeric(null, values);
    }

    public static NumericSeries<T> CreateNumeric<T>(string? name, NumericVector<T> values)
        where T : struct, INumber<T>
    {
        return CreateNumeric(name, values, null);
    }

    public static NumericSeries<T> CreateNumeric<T>(string? name, NumericVector<T> values, IDictionary<Variant, Variant>? references)
        where T : struct, INumber<T>
    {
        return new NumericSeries<T>(name, values, references);
    }
}

public abstract class Series<T, TVector> : Series
    where TVector : Vector<T>
{
    protected Series(string? name, TVector values, IDictionary<Variant, Variant>? references) : base(name)
    {
        ArgumentNullException.ThrowIfNull(values);

        Values = values;
        References = references ?? new Dictionary<Variant, Variant>();
    }

    [JsonPropertyName("values")]
    public TVector Values { get; }

    [JsonPropertyName("refs")]
    public IDictionary<Variant, Variant> References { get; }

    [JsonIgnore]
    public bool IsEmpty => Length == 0;

    [JsonPropertyName("length")]
    public int Length => Values.Length;
}

public sealed class Series<T> : Series<T, Vector<T>>
{
    [JsonConstructor]
    public Series(
        string? name,
        Vector<T> values,
        IDictionary<Variant, Variant>? references)
        : base(name, values, references)
    {
    }
}
