// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Numerics;

public static class VectorDataTypeHelper
{
    private static readonly FrozenDictionary<SeriesDataType, string> s_labelLookup = new Dictionary<SeriesDataType, string>()
    {
        [SeriesDataType.Bool] = SeriesDataTypeLabels.Bool,
        [SeriesDataType.Char] = SeriesDataTypeLabels.Char,
        [SeriesDataType.DateTime] = SeriesDataTypeLabels.DateTime,
        [SeriesDataType.DateTime64] = SeriesDataTypeLabels.DateTime64,
        [SeriesDataType.DateTimeOffset] = SeriesDataTypeLabels.DateTimeOffset,
        [SeriesDataType.Float32] = SeriesDataTypeLabels.Float32,
        [SeriesDataType.Float64] = SeriesDataTypeLabels.Float64,
        [SeriesDataType.Uuid] = SeriesDataTypeLabels.Uuid,
        [SeriesDataType.Int128] = SeriesDataTypeLabels.Int128,
        [SeriesDataType.Int16] = SeriesDataTypeLabels.Int16,
        [SeriesDataType.Int32] = SeriesDataTypeLabels.Int32,
        [SeriesDataType.Int64] = SeriesDataTypeLabels.Int64,
        [SeriesDataType.Int8] = SeriesDataTypeLabels.Int8,
        [SeriesDataType.String] = SeriesDataTypeLabels.String,
        [SeriesDataType.UInt128] = SeriesDataTypeLabels.UInt128,
        [SeriesDataType.UInt32] = SeriesDataTypeLabels.UInt32,
        [SeriesDataType.UInt64] = SeriesDataTypeLabels.UInt64,
        [SeriesDataType.UInt8] = SeriesDataTypeLabels.UInt8,

    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<string, SeriesDataType> s_vectorDataTypeLookup =
        s_labelLookup
        .ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
        .ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

    private static readonly FrozenDictionary<SeriesDataType, Type> s_typeLookup = new Dictionary<SeriesDataType, Type>()
    {
        [SeriesDataType.Bool] = typeof(bool),
        [SeriesDataType.Char] = typeof(char),
        [SeriesDataType.DateTime] = typeof(DateTime),
        [SeriesDataType.DateTime64] = typeof(DateTime64),
        [SeriesDataType.DateTimeOffset] = typeof(DateTimeOffset),
        [SeriesDataType.Float32] = typeof(float),
        [SeriesDataType.Float64] = typeof(double),
        [SeriesDataType.Uuid] = typeof(Guid),
        [SeriesDataType.Int128] = typeof(Int128),
        [SeriesDataType.Int16] = typeof(short),
        [SeriesDataType.Int32] = typeof(int),
        [SeriesDataType.Int64] = typeof(long),
        [SeriesDataType.Int8] = typeof(sbyte),
        [SeriesDataType.String] = typeof(string),
        [SeriesDataType.UInt128] = typeof(UInt128),
        [SeriesDataType.UInt32] = typeof(uint),
        [SeriesDataType.UInt64] = typeof(ulong),
        [SeriesDataType.UInt8] = typeof(byte),

    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<Type, SeriesDataType> s_vectorTypeLookup =
        s_typeLookup
        .ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
        .ToFrozenDictionary();

    private static readonly FrozenSet<SeriesDataType> s_numericTypes = new SeriesDataType[]
    {
        SeriesDataType.DateTime64,
        SeriesDataType.Float32,
        SeriesDataType.Float64,
        SeriesDataType.Uuid,
        SeriesDataType.Int128,
        SeriesDataType.Int16,
        SeriesDataType.Int32,
        SeriesDataType.Int64,
        SeriesDataType.Int8,
        SeriesDataType.UInt128,
        SeriesDataType.UInt32,
        SeriesDataType.UInt64,
        SeriesDataType.UInt8,

    }.ToFrozenSet();

    public static string GetLabel(SeriesDataType dataType)
    {
        return s_labelLookup[dataType];
    }

    public static SeriesDataType GetVectorDataType(string label)
    {
        return s_vectorDataTypeLookup[label];
    }

    public static SeriesDataType GetVectorDataType<T>()
    {
        return GetVectorDataType(typeof(T));
    }

    public static SeriesDataType GetVectorDataType(Type type)
    {
        if (TryGetVectorDataType(type, out var vectorDataType))
        {
            return vectorDataType;
        }

        throw new ArgumentException($"Type {type} is not a supported data type.");
    }

    public static Type GetItemType(SeriesDataType vectorDataType)
    {
        if (TryGetItemType(vectorDataType, out var type))
        {
            return type;
        }

        throw new ArgumentException($"VectorDataType {vectorDataType} is not a supported data type.");
    }

    public static bool TryGetItemType(SeriesDataType vectorDataType, [NotNullWhen(true)] out Type? type)
    {
        return s_typeLookup.TryGetValue(vectorDataType, out type);
    }

    public static bool TryGetVectorDataType(Type type, out SeriesDataType vectorDataType)
    {
        return s_vectorTypeLookup.TryGetValue(type, out vectorDataType);
    }

    public static bool IsNumericType(SeriesDataType vectorDataType)
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
