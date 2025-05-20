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
    /// <summary>
    /// The label for the <see cref="VectorDataType.Bool"/> data type.
    /// </summary>
    public const string Bool = "bool";

    /// <summary>
    /// The label for the <see cref="VectorDataType.Char"/> data type.
    /// </summary>
    public const string Char = "char";
    public const string String = "string";
    public const string DateTime = "datetime";
    public const string DateTimeOffset = "datetimeoffset";
    public const string UInt8 = "uint8";
    public const string DateTime64 = "datetime64";
    public const string Float64 = "float64";
    public const string Float32 = "float32";
    public const string Float16 = "float16";
    public const string Int32 = "int32";
    public const string Int64 = "int64";
    public const string Int8 = "int8";
    public const string Int16 = "int16";
    public const string UInt32 = "uint32";
    public const string UInt64 = "uint64";
    public const string UInt16 = "uint16";

    /// <summary>
    /// The label for the <see cref="VectorDataType.NaBool"/> data type.
    /// </summary>
    public const string NaBool = "na_bool";

    /// <summary>
    /// The label for the <see cref="VectorDataType.NaChar"/> data type.
    /// </summary>
    public const string NaChar = "na_char";

    /// <summary>
    /// The label for the <see cref="VectorDataType.NaString"/> data type.
    /// </summary>
    public const string NaString = "na_string";

    /// <summary>
    /// The label for the <see cref="VectorDataType.NaDateTime"/> data type.
    /// </summary>
    public const string NaDateTime = "na_datetime";
    public const string NaDateTimeOffset = "na_datetimeoffset";
    public const string NaUInt8 = "na_uint8";
    public const string NaDateTime64 = "na_datetime64";
    public const string NaFloat64 = "na_float64";
    public const string NaFloat32 = "na_float32";
    public const string NaFloat16 = "na_float16";
    public const string NaInt32 = "na_int32";
    public const string NaInt64 = "na_int64";
    public const string NaInt8 = "na_int8";
    public const string NaInt16 = "na_int16";
    public const string NaUInt32 = "na_uint32";
    public const string NaUInt64 = "na_uint64";
    public const string NaUInt16 = "na_uint16";
}
