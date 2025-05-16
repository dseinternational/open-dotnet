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
}
