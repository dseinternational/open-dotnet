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
    /// <summary>64-bit IEEE 754 floating-point (<see cref="double"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.Float64)]
    Float64,
    /// <summary>32-bit IEEE 754 floating-point (<see cref="float"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.Float32)]
    Float32,
    /// <summary>16-bit IEEE 754 floating-point (<see cref="Half"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.Float16)]
    Float16,
    /// <summary>64-bit signed integer (<see cref="long"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.Int64)]
    Int64,
    /// <summary>64-bit unsigned integer (<see cref="ulong"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.UInt64)]
    UInt64,
    /// <summary>32-bit signed integer (<see cref="int"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.Int32)]
    Int32,
    /// <summary>32-bit unsigned integer (<see cref="uint"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.UInt32)]
    UInt32,
    /// <summary>16-bit signed integer (<see cref="short"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.Int16)]
    Int16,
    /// <summary>16-bit unsigned integer (<see cref="ushort"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.UInt16)]
    UInt16,
    /// <summary>8-bit signed integer (<see cref="sbyte"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.Int8)]
    Int8,
    /// <summary>8-bit unsigned integer (<see cref="byte"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.UInt8)]
    UInt8,
    /// <summary>Compact 64-bit datetime (<see cref="DateTime64"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.DateTime64)]
    DateTime64,
    /// <summary>.NET <see cref="System.DateTime"/> (100-ns ticks since 0001-01-01).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.DateTime)]
    DateTime,
    /// <summary>.NET <see cref="System.DateTimeOffset"/>.</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.DateTimeOffset)]
    DateTimeOffset,
    /// <summary>Boolean (<see cref="bool"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.Bool)]
    Bool,
    /// <summary>UTF-16 character (<see cref="char"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.Char)]
    Char,
    /// <summary>String (<see cref="string"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.String)]
    String,

    /// <summary>NA-aware 64-bit float (<see cref="NaFloat{T}"/> over <see cref="double"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaFloat64)]
    NaFloat64,
    /// <summary>NA-aware 32-bit float (<see cref="NaFloat{T}"/> over <see cref="float"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaFloat32)]
    NaFloat32,
    /// <summary>NA-aware 16-bit float (<see cref="NaFloat{T}"/> over <see cref="Half"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaFloat16)]
    NaFloat16,
    /// <summary>NA-aware 64-bit signed integer (<see cref="NaInt{T}"/> over <see cref="long"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaInt64)]
    NaInt64,
    /// <summary>NA-aware 64-bit unsigned integer (<see cref="NaInt{T}"/> over <see cref="ulong"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaUInt64)]
    NaUInt64,
    /// <summary>NA-aware 32-bit signed integer (<see cref="NaInt{T}"/> over <see cref="int"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaInt32)]
    NaInt32,
    /// <summary>NA-aware 32-bit unsigned integer (<see cref="NaInt{T}"/> over <see cref="uint"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaUInt32)]
    NaUInt32,
    /// <summary>NA-aware 16-bit signed integer (<see cref="NaInt{T}"/> over <see cref="short"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaInt16)]
    NaInt16,
    /// <summary>NA-aware 16-bit unsigned integer (<see cref="NaInt{T}"/> over <see cref="ushort"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaUInt16)]
    NaUInt16,
    /// <summary>NA-aware 8-bit signed integer (<see cref="NaInt{T}"/> over <see cref="sbyte"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaInt8)]
    NaInt8,
    /// <summary>NA-aware 8-bit unsigned integer (<see cref="NaInt{T}"/> over <see cref="byte"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaUInt8)]
    NaUInt8,
    /// <summary>NA-aware compact 64-bit datetime (<see cref="NaInt{T}"/> over <see cref="DateTime64"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaDateTime64)]
    NaDateTime64,
    /// <summary>NA-aware <see cref="System.DateTime"/> (<see cref="NaValue{T}"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaDateTime)]
    NaDateTime,
    /// <summary>NA-aware <see cref="System.DateTimeOffset"/> (<see cref="NaValue{T}"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaDateTimeOffset)]
    NaDateTimeOffset,
    /// <summary>NA-aware boolean (<see cref="NaValue{T}"/> over <see cref="bool"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaBool)]
    NaBool,
    /// <summary>NA-aware UTF-16 character (<see cref="NaValue{T}"/> over <see cref="char"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaChar)]
    NaChar,
    /// <summary>NA-aware string (<see cref="NaValue{T}"/> over <see cref="string"/>).</summary>
    [JsonStringEnumMemberName(VectorDataTypeLabel.NaString)]
    NaString,
}
