// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Identifies the type of data stored in a <see cref="Series{T}"/>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<SeriesDataType>))]
public enum SeriesDataType
{
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Float64)]
    Float64,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Float32)]
    Float32,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Int64)]
    Int64,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.UInt64)]
    UInt64,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Int32)]
    Int32,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.UInt32)]
    UInt32,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Int16)]
    Int16,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.UInt16)]
    UInt16,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Int8)]
    Int8,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.UInt8)]
    UInt8,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Int128)]
    Int128,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.UInt128)]
    UInt128,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.DateTime64)]
    DateTime64,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.DateTime)]
    DateTime,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.DateTimeOffset)]
    DateTimeOffset,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Uuid)]
    Uuid,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Bool)]
    Bool,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.Char)]
    Char,
    [JsonStringEnumMemberName(SeriesDataTypeLabels.String)]
    String,
}
