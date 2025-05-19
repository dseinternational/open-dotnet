// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Identifies the type of data stored in a <see cref="IVector{T}"/> or  <see cref="IReadOnlyVector{T}"/>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<VectorDataType>))]
public enum VectorDataType
{
    [JsonStringEnumMemberName(VectorDataTypeLabels.Float64)]
    Float64,
    [JsonStringEnumMemberName(VectorDataTypeLabels.Float32)]
    Float32,
    [JsonStringEnumMemberName(VectorDataTypeLabels.Int64)]
    Int64,
    [JsonStringEnumMemberName(VectorDataTypeLabels.UInt64)]
    UInt64,
    [JsonStringEnumMemberName(VectorDataTypeLabels.Int32)]
    Int32,
    [JsonStringEnumMemberName(VectorDataTypeLabels.UInt32)]
    UInt32,
    [JsonStringEnumMemberName(VectorDataTypeLabels.Int16)]
    Int16,
    [JsonStringEnumMemberName(VectorDataTypeLabels.UInt16)]
    UInt16,
    [JsonStringEnumMemberName(VectorDataTypeLabels.Int8)]
    Int8,
    [JsonStringEnumMemberName(VectorDataTypeLabels.UInt8)]
    UInt8,
    [JsonStringEnumMemberName(VectorDataTypeLabels.Int128)]
    Int128,
    [JsonStringEnumMemberName(VectorDataTypeLabels.UInt128)]
    UInt128,
    [JsonStringEnumMemberName(VectorDataTypeLabels.DateTime64)]
    DateTime64,
    [JsonStringEnumMemberName(VectorDataTypeLabels.DateTime)]
    DateTime,
    [JsonStringEnumMemberName(VectorDataTypeLabels.DateTimeOffset)]
    DateTimeOffset,
    [JsonStringEnumMemberName(VectorDataTypeLabels.Uuid)]
    Uuid,
    [JsonStringEnumMemberName(VectorDataTypeLabels.Bool)]
    Bool,
    [JsonStringEnumMemberName(VectorDataTypeLabels.Char)]
    Char,
    [JsonStringEnumMemberName(VectorDataTypeLabels.String)]
    String,

    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableFloat64)]
    NullableFloat64,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableFloat32)]
    NullableFloat32,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableInt64)]
    NullableInt64,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableUInt64)]
    NullableUInt64,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableInt32)]
    NullableInt32,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableUInt32)]
    NullableUInt32,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableInt16)]
    NullableInt16,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableUInt16)]
    NullableUInt16,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableInt8)]
    NullableInt8,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableUInt8)]
    NullableUInt8,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableInt128)]
    NullableInt128,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableUInt128)]
    NullableUInt128,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableDateTime64)]
    NullableDateTime64,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableDateTime)]
    NullableDateTime,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableDateTimeOffset)]
    NullableDateTimeOffset,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableUuid)]
    NullableUuid,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableBool)]
    NullableBool,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableChar)]
    NullableChar,
    [JsonStringEnumMemberName(VectorDataTypeLabels.NullableString)]
    NullableString,
}
