// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

public partial class Vector
{
    private static readonly FrozenDictionary<VectorDataType, string> s_labelLookup = new Dictionary<VectorDataType, string>()
    {
        [VectorDataType.Bool] = VectorDataTypeLabel.Bool,
        [VectorDataType.Char] = VectorDataTypeLabel.Char,
        [VectorDataType.DateTime] = VectorDataTypeLabel.DateTime,
        [VectorDataType.DateTime64] = VectorDataTypeLabel.DateTime64,
        [VectorDataType.DateTimeOffset] = VectorDataTypeLabel.DateTimeOffset,
        [VectorDataType.Float16] = VectorDataTypeLabel.Float16,
        [VectorDataType.Float32] = VectorDataTypeLabel.Float32,
        [VectorDataType.Float64] = VectorDataTypeLabel.Float64,
        [VectorDataType.Int16] = VectorDataTypeLabel.Int16,
        [VectorDataType.Int32] = VectorDataTypeLabel.Int32,
        [VectorDataType.Int64] = VectorDataTypeLabel.Int64,
        [VectorDataType.Int8] = VectorDataTypeLabel.Int8,
        [VectorDataType.String] = VectorDataTypeLabel.String,
        [VectorDataType.UInt32] = VectorDataTypeLabel.UInt32,
        [VectorDataType.UInt64] = VectorDataTypeLabel.UInt64,
        [VectorDataType.UInt8] = VectorDataTypeLabel.UInt8,

        [VectorDataType.NaBool] = VectorDataTypeLabel.NaBool,
        [VectorDataType.NaChar] = VectorDataTypeLabel.NaChar,
        [VectorDataType.NaDateTime] = VectorDataTypeLabel.NaDateTime,
        [VectorDataType.NaDateTime64] = VectorDataTypeLabel.NaDateTime64,
        [VectorDataType.NaDateTimeOffset] = VectorDataTypeLabel.NaDateTimeOffset,
        [VectorDataType.NaFloat16] = VectorDataTypeLabel.NaFloat16,
        [VectorDataType.NaFloat32] = VectorDataTypeLabel.NaFloat32,
        [VectorDataType.NaFloat64] = VectorDataTypeLabel.NaFloat64,
        [VectorDataType.NaInt16] = VectorDataTypeLabel.NaInt16,
        [VectorDataType.NaInt32] = VectorDataTypeLabel.NaInt32,
        [VectorDataType.NaInt64] = VectorDataTypeLabel.NaInt64,
        [VectorDataType.NaInt8] = VectorDataTypeLabel.NaInt8,
        [VectorDataType.NaString] = VectorDataTypeLabel.NaString,
        [VectorDataType.NaUInt32] = VectorDataTypeLabel.NaUInt32,
        [VectorDataType.NaUInt64] = VectorDataTypeLabel.NaUInt64,
        [VectorDataType.NaUInt8] = VectorDataTypeLabel.NaUInt8,

    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<string, VectorDataType> s_vectorDataTypeLookup =
        s_labelLookup
        .ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
        .ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

    private static readonly FrozenDictionary<VectorDataType, Type> s_typeLookup = new Dictionary<VectorDataType, Type>()
    {
        [VectorDataType.Bool] = typeof(bool),
        [VectorDataType.Char] = typeof(char),
        [VectorDataType.DateTime] = typeof(DateTime),
        [VectorDataType.DateTime64] = typeof(DateTime64),
        [VectorDataType.DateTimeOffset] = typeof(DateTimeOffset),
        [VectorDataType.Float16] = typeof(Half),
        [VectorDataType.Float32] = typeof(float),
        [VectorDataType.Float64] = typeof(double),
        [VectorDataType.Int16] = typeof(short),
        [VectorDataType.Int32] = typeof(int),
        [VectorDataType.Int64] = typeof(long),
        [VectorDataType.Int8] = typeof(sbyte),
        [VectorDataType.String] = typeof(string),
        [VectorDataType.UInt32] = typeof(uint),
        [VectorDataType.UInt64] = typeof(ulong),
        [VectorDataType.UInt16] = typeof(ushort),
        [VectorDataType.UInt8] = typeof(byte),

        [VectorDataType.NaBool] = typeof(NaValue<bool>),
        [VectorDataType.NaChar] = typeof(NaValue<char>),
        [VectorDataType.NaDateTime] = typeof(NaValue<DateTime>),
        [VectorDataType.NaDateTime64] = typeof(NaInt<DateTime64>),
        [VectorDataType.NaDateTimeOffset] = typeof(NaValue<DateTimeOffset>),
        [VectorDataType.NaFloat16] = typeof(NaFloat<Half>),
        [VectorDataType.NaFloat32] = typeof(NaFloat<float>),
        [VectorDataType.NaFloat64] = typeof(NaFloat<double>),
        [VectorDataType.NaInt16] = typeof(NaInt<short>),
        [VectorDataType.NaInt32] = typeof(NaInt<int>),
        [VectorDataType.NaInt64] = typeof(NaInt<long>),
        [VectorDataType.NaInt8] = typeof(NaInt<sbyte>),
        [VectorDataType.NaString] = typeof(NaValue<string>),
        [VectorDataType.NaUInt32] = typeof(NaInt<uint>),
        [VectorDataType.NaUInt64] = typeof(NaInt<ulong>),
        [VectorDataType.NaUInt16] = typeof(NaInt<ushort>),
        [VectorDataType.NaUInt8] = typeof(NaInt<byte>),

    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<Type, VectorDataType> s_vectorTypeLookup =
        s_typeLookup
        .ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
        .ToFrozenDictionary();

    private static readonly FrozenSet<VectorDataType> s_numericTypes = new VectorDataType[]
    {
        VectorDataType.DateTime64,
        VectorDataType.Float16,
        VectorDataType.Float32,
        VectorDataType.Float64,
        VectorDataType.Int16,
        VectorDataType.Int32,
        VectorDataType.Int64,
        VectorDataType.Int8,
        VectorDataType.UInt32,
        VectorDataType.UInt64,
        VectorDataType.UInt8,
        VectorDataType.NaDateTime64,
        VectorDataType.NaFloat16,
        VectorDataType.NaFloat32,
        VectorDataType.NaFloat64,
        VectorDataType.NaInt16,
        VectorDataType.NaInt32,
        VectorDataType.NaInt64,
        VectorDataType.NaInt8,
        VectorDataType.NaUInt32,
        VectorDataType.NaUInt64,
        VectorDataType.NaUInt8,

    }.ToFrozenSet();

    public static string GetDataTypeLabel(VectorDataType dataType)
    {
        return s_labelLookup[dataType];
    }

    public static VectorDataType GetDataType(string label)
    {
        return s_vectorDataTypeLookup[label];
    }

    public static VectorDataType GetDataType<T>()
    {
        return GetDataType(typeof(T));
    }

    public static VectorDataType GetDataType(Type type)
    {
        if (TryGetDataType(type, out var vectorDataType))
        {
            return vectorDataType;
        }

        throw new ArgumentException($"Type {type} is not a supported data type.");
    }

    public static Type GetType(VectorDataType vectorDataType)
    {
        if (TryGetType(vectorDataType, out var type))
        {
            return type;
        }

        throw new ArgumentException($"VectorDataType {vectorDataType} is not a supported data type.");
    }

    public static bool TryGetType(VectorDataType vectorDataType, [NotNullWhen(true)] out Type? type)
    {
        return s_typeLookup.TryGetValue(vectorDataType, out type);
    }

    public static bool TryGetDataType(Type type, out VectorDataType vectorDataType)
    {
        return s_vectorTypeLookup.TryGetValue(type, out vectorDataType);
    }

    public static bool IsNumericType(VectorDataType vectorDataType)
    {
        return s_numericTypes.Contains(vectorDataType);
    }

    public static bool IsNumericType(Type type)
    {
        return IsNumericType(GetDataType(type));
    }

    public static bool IsNumericType<T>()
    {
        return IsNumericType(typeof(T));
    }
}
