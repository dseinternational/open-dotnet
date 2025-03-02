// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Numerics.Data;

namespace DSE.Open.Numerics;

[JsonSourceGenerationOptions(
    UseStringEnumConverter = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DictionaryKeyPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    WriteIndented = false)]
[JsonSerializable(typeof(DataSet))]
[JsonSerializable(typeof(DataFrame))]
[JsonSerializable(typeof(Vector))]
[JsonSerializable(typeof(Vector<char>))]
[JsonSerializable(typeof(Vector<string>))]
[JsonSerializable(typeof(Vector<Guid>))]
[JsonSerializable(typeof(Vector<DateTime>))]
[JsonSerializable(typeof(Vector<DateTimeOffset>))]
[JsonSerializable(typeof(NumericVector<byte>))]
[JsonSerializable(typeof(NumericVector<sbyte>))]
[JsonSerializable(typeof(NumericVector<short>))]
[JsonSerializable(typeof(NumericVector<ushort>))]
[JsonSerializable(typeof(NumericVector<int>))]
[JsonSerializable(typeof(NumericVector<uint>))]
[JsonSerializable(typeof(NumericVector<long>))]
[JsonSerializable(typeof(NumericVector<ulong>))]
[JsonSerializable(typeof(NumericVector<Int128>))]
[JsonSerializable(typeof(NumericVector<UInt128>))]
[JsonSerializable(typeof(NumericVector<Half>))]
[JsonSerializable(typeof(NumericVector<float>))]
[JsonSerializable(typeof(NumericVector<double>))]
[JsonSerializable(typeof(NumericVector<decimal>))]
[JsonSerializable(typeof(NumericVector<DateTime64>))]
[JsonSerializable(typeof(Series))]
[JsonSerializable(typeof(Series<char>))]
[JsonSerializable(typeof(Series<string>))]
[JsonSerializable(typeof(Series<Guid>))]
[JsonSerializable(typeof(Series<DateTime>))]
[JsonSerializable(typeof(Series<DateTimeOffset>))]
[JsonSerializable(typeof(NumericSeries<byte>))]
[JsonSerializable(typeof(NumericSeries<sbyte>))]
[JsonSerializable(typeof(NumericSeries<short>))]
[JsonSerializable(typeof(NumericSeries<ushort>))]
[JsonSerializable(typeof(NumericSeries<int>))]
[JsonSerializable(typeof(NumericSeries<uint>))]
[JsonSerializable(typeof(NumericSeries<long>))]
[JsonSerializable(typeof(NumericSeries<ulong>))]
[JsonSerializable(typeof(NumericSeries<Int128>))]
[JsonSerializable(typeof(NumericSeries<UInt128>))]
[JsonSerializable(typeof(NumericSeries<Half>))]
[JsonSerializable(typeof(NumericSeries<float>))]
[JsonSerializable(typeof(NumericSeries<double>))]
[JsonSerializable(typeof(NumericSeries<decimal>))]
[JsonSerializable(typeof(NumericSeries<DateTime64>))]
public sealed partial class NumericsJsonSerializationContext : JsonSerializerContext;
