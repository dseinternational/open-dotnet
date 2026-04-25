// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

/// <summary>
/// Provides labels for the data types supported by <see cref="Series{T}"/>.
/// </summary>
/// <remarks>
/// These string constants are the wire-format labels embedded by the JSON
/// serializers (e.g. <see cref="DSE.Open.Numerics.Serialization.VectorJsonWriter"/>)
/// and parsed back via <see cref="Vector.GetDataType(string)"/>. They form the
/// stable public contract for cross-version vector serialization.
/// </remarks>
[SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "By design")]
public static class VectorDataTypeLabel
{
    /// <summary>The label for the <see cref="VectorDataType.Bool"/> data type.</summary>
    public const string Bool = "bool";
    /// <summary>The label for the <see cref="VectorDataType.Char"/> data type.</summary>
    public const string Char = "char";
    /// <summary>The label for the <see cref="VectorDataType.String"/> data type.</summary>
    public const string String = "string";
    /// <summary>The label for the <see cref="VectorDataType.DateTime"/> data type.</summary>
    public const string DateTime = "datetime";
    /// <summary>The label for the <see cref="VectorDataType.DateTimeOffset"/> data type.</summary>
    public const string DateTimeOffset = "datetimeoffset";
    /// <summary>The label for the <see cref="VectorDataType.UInt8"/> data type.</summary>
    public const string UInt8 = "uint8";
    /// <summary>The label for the <see cref="VectorDataType.DateTime64"/> data type.</summary>
    public const string DateTime64 = "datetime64";
    /// <summary>The label for the <see cref="VectorDataType.Float64"/> data type.</summary>
    public const string Float64 = "float64";
    /// <summary>The label for the <see cref="VectorDataType.Float32"/> data type.</summary>
    public const string Float32 = "float32";
    /// <summary>The label for the <see cref="VectorDataType.Float16"/> data type.</summary>
    public const string Float16 = "float16";
    /// <summary>The label for the <see cref="VectorDataType.Int32"/> data type.</summary>
    public const string Int32 = "int32";
    /// <summary>The label for the <see cref="VectorDataType.Int64"/> data type.</summary>
    public const string Int64 = "int64";
    /// <summary>The label for the <see cref="VectorDataType.Int8"/> data type.</summary>
    public const string Int8 = "int8";
    /// <summary>The label for the <see cref="VectorDataType.Int16"/> data type.</summary>
    public const string Int16 = "int16";
    /// <summary>The label for the <see cref="VectorDataType.UInt32"/> data type.</summary>
    public const string UInt32 = "uint32";
    /// <summary>The label for the <see cref="VectorDataType.UInt64"/> data type.</summary>
    public const string UInt64 = "uint64";
    /// <summary>The label for the <see cref="VectorDataType.UInt16"/> data type.</summary>
    public const string UInt16 = "uint16";

    /// <summary>The label for the <see cref="VectorDataType.NaBool"/> data type.</summary>
    public const string NaBool = "na_bool";
    /// <summary>The label for the <see cref="VectorDataType.NaChar"/> data type.</summary>
    public const string NaChar = "na_char";
    /// <summary>The label for the <see cref="VectorDataType.NaString"/> data type.</summary>
    public const string NaString = "na_string";
    /// <summary>The label for the <see cref="VectorDataType.NaDateTime"/> data type.</summary>
    public const string NaDateTime = "na_datetime";
    /// <summary>The label for the <see cref="VectorDataType.NaDateTimeOffset"/> data type.</summary>
    public const string NaDateTimeOffset = "na_datetimeoffset";
    /// <summary>The label for the <see cref="VectorDataType.NaUInt8"/> data type.</summary>
    public const string NaUInt8 = "na_uint8";
    /// <summary>The label for the <see cref="VectorDataType.NaDateTime64"/> data type.</summary>
    public const string NaDateTime64 = "na_datetime64";
    /// <summary>The label for the <see cref="VectorDataType.NaFloat64"/> data type.</summary>
    public const string NaFloat64 = "na_float64";
    /// <summary>The label for the <see cref="VectorDataType.NaFloat32"/> data type.</summary>
    public const string NaFloat32 = "na_float32";
    /// <summary>The label for the <see cref="VectorDataType.NaFloat16"/> data type.</summary>
    public const string NaFloat16 = "na_float16";
    /// <summary>The label for the <see cref="VectorDataType.NaInt32"/> data type.</summary>
    public const string NaInt32 = "na_int32";
    /// <summary>The label for the <see cref="VectorDataType.NaInt64"/> data type.</summary>
    public const string NaInt64 = "na_int64";
    /// <summary>The label for the <see cref="VectorDataType.NaInt8"/> data type.</summary>
    public const string NaInt8 = "na_int8";
    /// <summary>The label for the <see cref="VectorDataType.NaInt16"/> data type.</summary>
    public const string NaInt16 = "na_int16";
    /// <summary>The label for the <see cref="VectorDataType.NaUInt32"/> data type.</summary>
    public const string NaUInt32 = "na_uint32";
    /// <summary>The label for the <see cref="VectorDataType.NaUInt64"/> data type.</summary>
    public const string NaUInt64 = "na_uint64";
    /// <summary>The label for the <see cref="VectorDataType.NaUInt16"/> data type.</summary>
    public const string NaUInt16 = "na_uint16";
}
