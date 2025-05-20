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
    [JsonStringEnumMemberName(VectorDataTypeLabel.Float16)]
    Float16,
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
    [JsonStringEnumMemberName(VectorDataTypeLabel.Bool)]
    Bool,
    [JsonStringEnumMemberName(VectorDataTypeLabel.Char)]
    Char,
    [JsonStringEnumMemberName(VectorDataTypeLabel.String)]
    String,

    [JsonStringEnumMemberName(VectorDataTypeLabel.NaFloat64)]
    NaFloat64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaFloat32)]
    NaFloat32,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaFloat16)]
    NaFloat16,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaInt64)]
    NaInt64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaUInt64)]
    NaUInt64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaInt32)]
    NaInt32,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaUInt32)]
    NaUInt32,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaInt16)]
    NaInt16,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaUInt16)]
    NaUInt16,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaInt8)]
    NaInt8,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaUInt8)]
    NaUInt8,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaDateTime64)]
    NaDateTime64,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaDateTime)]
    NaDateTime,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaDateTimeOffset)]
    NaDateTimeOffset,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaBool)]
    NaBool,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaChar)]
    NaChar,
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaString)]
    NaString,
}
