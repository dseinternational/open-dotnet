// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// Identifies the type of data stored in a <see cref="IVector{T}"/> or  <see cref="IReadOnlyVector{T}"/>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<VectorDataType>))]
public enum VectorDataType : byte
{
    [JsonStringEnumMemberName(VectorDataTypeLabel.Float64)]
    Float64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.Float32)]
    Float32,
    [JsonStringEnumMemberName(VectorDataTypeLabel.Int64)]
    Int64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.UInt64)]
    UInt64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.Int32)]
    Int32,
    [JsonStringEnumMemberName(VectorDataTypeLabel.UInt32)]
    UInt32,
    [JsonStringEnumMemberName(VectorDataTypeLabel.Int16)]
    Int16,
    [JsonStringEnumMemberName(VectorDataTypeLabel.UInt16)]
    UInt16,
    [JsonStringEnumMemberName(VectorDataTypeLabel.Int8)]
    Int8,
    [JsonStringEnumMemberName(VectorDataTypeLabel.UInt8)]
    UInt8,
    [JsonStringEnumMemberName(VectorDataTypeLabel.DateTime64)]
    DateTime64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.DateTime)]
    DateTime,
    [JsonStringEnumMemberName(VectorDataTypeLabel.DateTimeOffset)]
    DateTimeOffset,
    [JsonStringEnumMemberName(VectorDataTypeLabel.Uuid)]
    Uuid,
    [JsonStringEnumMemberName(VectorDataTypeLabel.Bool)]
    Bool,
    [JsonStringEnumMemberName(VectorDataTypeLabel.Char)]
    Char,
    [JsonStringEnumMemberName(VectorDataTypeLabel.String)]
    String,

    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableFloat64)]
    NaFloat64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableFloat32)]
    NaFloat32,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableInt64)]
    NaInt64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableUInt64)]
    NaUInt64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableInt32)]
    NaInt32,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableUInt32)]
    NaUInt32,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableInt16)]
    NaInt16,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableUInt16)]
    NaUInt16,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableInt8)]
    NaInt8,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableUInt8)]
    NaUInt8,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableDateTime64)]
    NaDateTime64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableDateTime)]
    NaDateTime,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableDateTimeOffset)]
    NaDateTimeOffset,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableUuid)]
    NaUuid,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableBool)]
    NaBool,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableChar)]
    NaChar,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NullableString)]
    NaString,
}
