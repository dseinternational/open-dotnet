// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

public static class VectorDataTypeHelper
{
    private static readonly FrozenDictionary<VectorDataType, string> s_labelLookup = new Dictionary<VectorDataType, string>()
    {
        [VectorDataType.Bool] = VectorDataTypeLabels.Bool,
        [VectorDataType.Char] = VectorDataTypeLabels.Char,
        [VectorDataType.DateTime] = VectorDataTypeLabels.DateTime,
        [VectorDataType.DateTime64] = VectorDataTypeLabels.DateTime64,
        [VectorDataType.DateTimeOffset] = VectorDataTypeLabels.DateTimeOffset,
        [VectorDataType.Float32] = VectorDataTypeLabels.Float32,
        [VectorDataType.Float64] = VectorDataTypeLabels.Float64,
        [VectorDataType.Uuid] = VectorDataTypeLabels.Uuid,
        [VectorDataType.Int128] = VectorDataTypeLabels.Int128,
        [VectorDataType.Int16] = VectorDataTypeLabels.Int16,
        [VectorDataType.Int32] = VectorDataTypeLabels.Int32,
        [VectorDataType.Int64] = VectorDataTypeLabels.Int64,
        [VectorDataType.Int8] = VectorDataTypeLabels.Int8,
        [VectorDataType.String] = VectorDataTypeLabels.String,
        [VectorDataType.UInt128] = VectorDataTypeLabels.UInt128,
        [VectorDataType.UInt32] = VectorDataTypeLabels.UInt32,
        [VectorDataType.UInt64] = VectorDataTypeLabels.UInt64,
        [VectorDataType.UInt8] = VectorDataTypeLabels.UInt8,

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
        [VectorDataType.Float32] = typeof(float),
        [VectorDataType.Float64] = typeof(double),
        [VectorDataType.Uuid] = typeof(Guid),
        [VectorDataType.Int128] = typeof(Int128),
        [VectorDataType.Int16] = typeof(short),
        [VectorDataType.Int32] = typeof(int),
        [VectorDataType.Int64] = typeof(long),
        [VectorDataType.Int8] = typeof(sbyte),
        [VectorDataType.String] = typeof(string),
        [VectorDataType.UInt128] = typeof(UInt128),
        [VectorDataType.UInt32] = typeof(uint),
        [VectorDataType.UInt64] = typeof(ulong),
        [VectorDataType.UInt8] = typeof(byte),

        [VectorDataType.NullableBool] = typeof(NaValue<bool>),
        [VectorDataType.NullableChar] = typeof(NaValue<char>),
        [VectorDataType.NullableDateTime] = typeof(NaValue<DateTime>),
        [VectorDataType.NullableDateTime64] = typeof(NaValue<DateTime64>),
        [VectorDataType.NullableDateTimeOffset] = typeof(NaValue<DateTimeOffset>),
        //[VectorDataType.NullableFloat32] = typeof(NaInteger<float>),
        //[VectorDataType.NullableFloat64] = typeof(NaInteger<double>),
        [VectorDataType.NullableUuid] = typeof(NaValue<Guid>),
        [VectorDataType.NullableInt128] = typeof(NaInteger<Int128>),
        [VectorDataType.NullableInt16] = typeof(NaInteger<short>),
        [VectorDataType.NullableInt32] = typeof(NaInteger<int>),
        [VectorDataType.NullableInt64] = typeof(NaInteger<long>),
        [VectorDataType.NullableInt8] = typeof(NaInteger<sbyte>),
        [VectorDataType.NullableString] = typeof(NaValue<string>),
        [VectorDataType.NullableUInt128] = typeof(NaInteger<UInt128>),
        [VectorDataType.NullableUInt32] = typeof(NaInteger<uint>),
        [VectorDataType.NullableUInt64] = typeof(NaInteger<ulong>),
        [VectorDataType.NullableUInt8] = typeof(NaInteger<byte>),

    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<Type, VectorDataType> s_vectorTypeLookup =
        s_typeLookup
        .ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
        .ToFrozenDictionary();

    private static readonly FrozenSet<VectorDataType> s_numericTypes = new VectorDataType[]
    {
        VectorDataType.DateTime64,
        VectorDataType.Float32,
        VectorDataType.Float64,
        VectorDataType.Uuid,
        VectorDataType.Int128,
        VectorDataType.Int16,
        VectorDataType.Int32,
        VectorDataType.Int64,
        VectorDataType.Int8,
        VectorDataType.UInt128,
        VectorDataType.UInt32,
        VectorDataType.UInt64,
        VectorDataType.UInt8,

        VectorDataType.NullableDateTime64,
        VectorDataType.NullableFloat32,
        VectorDataType.NullableFloat64,
        VectorDataType.NullableUuid,
        VectorDataType.NullableInt128,
        VectorDataType.NullableInt16,
        VectorDataType.NullableInt32,
        VectorDataType.NullableInt64,
        VectorDataType.NullableInt8,
        VectorDataType.NullableUInt128,
        VectorDataType.NullableUInt32,
        VectorDataType.NullableUInt64,
        VectorDataType.NullableUInt8,

    }.ToFrozenSet();

    public static string GetLabel(VectorDataType dataType)
    {
        return s_labelLookup[dataType];
    }

    public static VectorDataType GetVectorDataType(string label)
    {
        return s_vectorDataTypeLookup[label];
    }

    public static VectorDataType GetVectorDataType<T>()
    {
        return GetVectorDataType(typeof(T));
    }

    public static VectorDataType GetVectorDataType(Type type)
    {
        if (TryGetVectorDataType(type, out var vectorDataType))
        {
            return vectorDataType;
        }

        throw new ArgumentException($"Type {type} is not a supported data type.");
    }

    public static Type GetItemType(VectorDataType vectorDataType)
    {
        if (TryGetItemType(vectorDataType, out var type))
        {
            return type;
        }

        throw new ArgumentException($"VectorDataType {vectorDataType} is not a supported data type.");
    }

    public static bool TryGetItemType(VectorDataType vectorDataType, [NotNullWhen(true)] out Type? type)
    {
        return s_typeLookup.TryGetValue(vectorDataType, out type);
    }

    public static bool TryGetVectorDataType(Type type, out VectorDataType vectorDataType)
    {
        return s_vectorTypeLookup.TryGetValue(type, out vectorDataType);
    }

    public static bool IsNumericType(VectorDataType vectorDataType)
    {
        return s_numericTypes.Contains(vectorDataType);
    }

    public static bool IsNumericType(Type type)
    {
        return IsNumericType(GetVectorDataType(type));
    }

    public static bool IsNumericType<T>()
    {
        return IsNumericType(typeof(T));
    }
}
