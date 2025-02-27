// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "dtype")]
[JsonDerivedType(typeof(Series<bool>), "bool")]
[JsonDerivedType(typeof(Series<char>), "char")]
[JsonDerivedType(typeof(Series<string>), "string")]
[JsonDerivedType(typeof(Series<DateTime>), "datetime")]
[JsonDerivedType(typeof(Series<DateTimeOffset>), "datetimeoffset")]
[JsonDerivedType(typeof(Series<Guid>), "uuid")]
[JsonDerivedType(typeof(NumericSeries<byte>), "uint8")]
[JsonDerivedType(typeof(NumericSeries<DateTime64>), "datetime64")]
[JsonDerivedType(typeof(NumericSeries<double>), "float64")]
[JsonDerivedType(typeof(NumericSeries<float>), "float32")]
[JsonDerivedType(typeof(NumericSeries<int>), "int32")]
[JsonDerivedType(typeof(NumericSeries<Int128>), "int128")]
[JsonDerivedType(typeof(NumericSeries<long>), "int64")]
[JsonDerivedType(typeof(NumericSeries<sbyte>), "int8")]
[JsonDerivedType(typeof(NumericSeries<short>), "int16")]
[JsonDerivedType(typeof(NumericSeries<uint>), "uint32")]
[JsonDerivedType(typeof(NumericSeries<ulong>), "uint64")]
[JsonDerivedType(typeof(NumericSeries<UInt128>), "uint128")]
[JsonDerivedType(typeof(NumericSeries<ushort>), "uint16")]
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

    public static Series<T> Create<T>(string? name, Vector<T> values, IDictionary<T, Variant>? references)
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

    public static NumericSeries<T> CreateNumeric<T>(string? name, NumericVector<T> values, IDictionary<T, Variant>? references)
        where T : struct, INumber<T>
    {
        return new NumericSeries<T>(name, values, references);
    }
}

public abstract class Series<T, TVector> : Series
   where T : notnull
    where TVector : Vector<T>
{
    protected Series(string? name, TVector values, IDictionary<T, Variant>? references) : base(name)
    {
        ArgumentNullException.ThrowIfNull(values);

        Values = values;
        References = references ?? new Dictionary<T, Variant>();
    }

    [JsonPropertyName("values")]
    public TVector Values { get; }

    [JsonPropertyName("refs")]
    public IDictionary<T, Variant> References { get; }

    [JsonIgnore]
    public bool IsEmpty => Values.Length == 0;

    [JsonIgnore]
    public int Length => Values.Length;
}

public sealed class Series<T> : Series<T, Vector<T>>
   where T : notnull
{
    [JsonConstructor]
    public Series(
        string? name,
        Vector<T> values,
        IDictionary<T, Variant>? references)
        : base(name, values, references)
    {
    }
}
