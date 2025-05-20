// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

/// <summary>
/// Provides labels for the data types supported by <see cref="Series{T}"/>.
/// </summary>
[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "By design")]
public static class VectorDataTypeLabel
{
    public const string Bool = "bool";
    public const string Char = "char";
    public const string String = "string";
    public const string DateTime = "datetime";
    public const string DateTimeOffset = "datetimeoffset";
    public const string Uuid = "uuid";
    public const string UInt8 = "uint8";
    public const string DateTime64 = "datetime64";
    public const string Float64 = "float64";
    public const string Float32 = "float32";
    public const string Int32 = "int32";
    public const string Int64 = "int64";
    public const string Int8 = "int8";
    public const string Int16 = "int16";
    public const string UInt32 = "uint32";
    public const string UInt64 = "uint64";
    public const string UInt16 = "uint16";

    public const string NullableBool = "bool_n";
    public const string NullableChar = "char_n";
    public const string NullableString = "string_n";
    public const string NullableDateTime = "datetime_n";
    public const string NullableDateTimeOffset = "datetimeoffset_n";
    public const string NullableUuid = "uuid_n";
    public const string NullableUInt8 = "uint8_n";
    public const string NullableDateTime64 = "datetime64_n";
    public const string NullableFloat64 = "float64_n";
    public const string NullableFloat32 = "float32_n";
    public const string NullableInt32 = "int32_n";
    public const string NullableInt64 = "int64_n";
    public const string NullableInt8 = "int8_n";
    public const string NullableInt16 = "int16_n";
    public const string NullableUInt32 = "uint32_n";
    public const string NullableUInt64 = "uint64_n";
    public const string NullableUInt16 = "uint16_n";
}
