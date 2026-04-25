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
        [VectorDataType.Int8] = VectorDataTypeLabel.Int8,
        [VectorDataType.Int16] = VectorDataTypeLabel.Int16,
        [VectorDataType.Int32] = VectorDataTypeLabel.Int32,
        [VectorDataType.Int64] = VectorDataTypeLabel.Int64,
        [VectorDataType.String] = VectorDataTypeLabel.String,
        [VectorDataType.UInt8] = VectorDataTypeLabel.UInt8,
        [VectorDataType.UInt16] = VectorDataTypeLabel.UInt16,
        [VectorDataType.UInt32] = VectorDataTypeLabel.UInt32,
        [VectorDataType.UInt64] = VectorDataTypeLabel.UInt64,

        [VectorDataType.NaBool] = VectorDataTypeLabel.NaBool,
        [VectorDataType.NaChar] = VectorDataTypeLabel.NaChar,
        [VectorDataType.NaDateTime] = VectorDataTypeLabel.NaDateTime,
        [VectorDataType.NaDateTime64] = VectorDataTypeLabel.NaDateTime64,
        [VectorDataType.NaDateTimeOffset] = VectorDataTypeLabel.NaDateTimeOffset,
        [VectorDataType.NaFloat16] = VectorDataTypeLabel.NaFloat16,
        [VectorDataType.NaFloat32] = VectorDataTypeLabel.NaFloat32,
        [VectorDataType.NaFloat64] = VectorDataTypeLabel.NaFloat64,
        [VectorDataType.NaInt8] = VectorDataTypeLabel.NaInt8,
        [VectorDataType.NaInt16] = VectorDataTypeLabel.NaInt16,
        [VectorDataType.NaInt32] = VectorDataTypeLabel.NaInt32,
        [VectorDataType.NaInt64] = VectorDataTypeLabel.NaInt64,
        [VectorDataType.NaUInt8] = VectorDataTypeLabel.NaUInt8,
        [VectorDataType.NaUInt16] = VectorDataTypeLabel.NaUInt16,
        [VectorDataType.NaUInt32] = VectorDataTypeLabel.NaUInt32,
        [VectorDataType.NaUInt64] = VectorDataTypeLabel.NaUInt64,

        [VectorDataType.NaString] = VectorDataTypeLabel.NaString,
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
        [VectorDataType.Int8] = typeof(sbyte),
        [VectorDataType.Int16] = typeof(short),
        [VectorDataType.Int32] = typeof(int),
        [VectorDataType.Int64] = typeof(long),
        [VectorDataType.String] = typeof(string),
        [VectorDataType.UInt8] = typeof(byte),
        [VectorDataType.UInt16] = typeof(ushort),
        [VectorDataType.UInt32] = typeof(uint),
        [VectorDataType.UInt64] = typeof(ulong),

        [VectorDataType.NaBool] = typeof(NaValue<bool>),
        [VectorDataType.NaChar] = typeof(NaValue<char>),
        [VectorDataType.NaDateTime] = typeof(NaValue<DateTime>),
        [VectorDataType.NaDateTime64] = typeof(NaInt<DateTime64>),
        [VectorDataType.NaDateTimeOffset] = typeof(NaValue<DateTimeOffset>),
        [VectorDataType.NaFloat16] = typeof(NaFloat<Half>),
        [VectorDataType.NaFloat32] = typeof(NaFloat<float>),
        [VectorDataType.NaFloat64] = typeof(NaFloat<double>),
        [VectorDataType.NaInt8] = typeof(NaInt<sbyte>),
        [VectorDataType.NaInt16] = typeof(NaInt<short>),
        [VectorDataType.NaInt32] = typeof(NaInt<int>),
        [VectorDataType.NaInt64] = typeof(NaInt<long>),
        [VectorDataType.NaString] = typeof(NaValue<string>),
        [VectorDataType.NaUInt8] = typeof(NaInt<byte>),
        [VectorDataType.NaUInt16] = typeof(NaInt<ushort>),
        [VectorDataType.NaUInt32] = typeof(NaInt<uint>),
        [VectorDataType.NaUInt64] = typeof(NaInt<ulong>),
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<Type, VectorDataType> s_vectorTypeLookup =
        s_typeLookup
        .ToDictionary(kvp => kvp.Value, kvp => kvp.Key)
        .ToFrozenDictionary();

    private static readonly FrozenSet<VectorDataType> s_numericTypes = new[]
    {
        VectorDataType.DateTime64,
        VectorDataType.Float16,
        VectorDataType.Float32,
        VectorDataType.Float64,
        VectorDataType.Int8,
        VectorDataType.Int16,
        VectorDataType.Int32,
        VectorDataType.Int64,
        VectorDataType.UInt8,
        VectorDataType.UInt16,
        VectorDataType.UInt32,
        VectorDataType.UInt64,
        VectorDataType.NaDateTime64,
        VectorDataType.NaFloat16,
        VectorDataType.NaFloat32,
        VectorDataType.NaFloat64,
        VectorDataType.NaInt8,
        VectorDataType.NaInt16,
        VectorDataType.NaInt32,
        VectorDataType.NaInt64,
        VectorDataType.NaUInt8,
        VectorDataType.NaUInt16,
        VectorDataType.NaUInt32,
        VectorDataType.NaUInt64,

    }.ToFrozenSet();

    /// <summary>
    /// Returns the string label that identifies <paramref name="dataType"/> in
    /// JSON serialization (e.g. <c>"Float64"</c> for <see cref="VectorDataType.Float64"/>).
    /// </summary>
    public static string GetDataTypeLabel(VectorDataType dataType)
    {
        return s_labelLookup[dataType];
    }

    /// <summary>
    /// Returns the <see cref="VectorDataType"/> identified by the JSON
    /// <paramref name="label"/> (case-insensitive ordinal match).
    /// </summary>
    /// <exception cref="KeyNotFoundException">Thrown when <paramref name="label"/>
    /// does not match a known data-type label.</exception>
    public static VectorDataType GetDataType(string label)
    {
        return s_vectorDataTypeLookup[label];
    }

    /// <summary>
    /// Returns the <see cref="VectorDataType"/> for element type
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <exception cref="ArgumentException"><typeparamref name="T"/> is not a
    /// supported element type. See <see cref="TryGetDataType(Type, out VectorDataType)"/>
    /// for a non-throwing variant.</exception>
    public static VectorDataType GetDataType<T>()
    {
        return GetDataType(typeof(T));
    }

    /// <summary>
    /// Returns the <see cref="VectorDataType"/> for runtime element type
    /// <paramref name="type"/>.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="type"/> is not a
    /// supported element type.</exception>
    public static VectorDataType GetDataType(Type type)
    {
        if (TryGetDataType(type, out var vectorDataType))
        {
            return vectorDataType;
        }

        throw new ArgumentException($"Type {type} is not a supported data type.");
    }

    /// <summary>
    /// Returns the runtime element <see cref="Type"/> for the given
    /// <paramref name="vectorDataType"/>.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="vectorDataType"/>
    /// has no corresponding runtime type.</exception>
    public static Type GetType(VectorDataType vectorDataType)
    {
        if (TryGetType(vectorDataType, out var type))
        {
            return type;
        }

        throw new ArgumentException($"VectorDataType {vectorDataType} is not a supported data type.");
    }

    /// <summary>
    /// Attempts to map <paramref name="vectorDataType"/> to its runtime
    /// element <see cref="Type"/>.
    /// </summary>
    /// <param name="vectorDataType">The data-type tag.</param>
    /// <param name="type">When the method returns, the matching <see cref="Type"/>
    /// or <see langword="null"/> if no mapping was found.</param>
    /// <returns><see langword="true"/> when a mapping exists, otherwise <see langword="false"/>.</returns>
    public static bool TryGetType(VectorDataType vectorDataType, [NotNullWhen(true)] out Type? type)
    {
        return s_typeLookup.TryGetValue(vectorDataType, out type);
    }

    /// <summary>
    /// Attempts to map runtime <paramref name="type"/> to its
    /// <see cref="VectorDataType"/>.
    /// </summary>
    /// <param name="type">The runtime element type.</param>
    /// <param name="vectorDataType">When the method returns, the matching
    /// <see cref="VectorDataType"/>; otherwise the default value.</param>
    /// <returns><see langword="true"/> when a mapping exists, otherwise <see langword="false"/>.</returns>
    public static bool TryGetDataType(Type type, out VectorDataType vectorDataType)
    {
        return s_vectorTypeLookup.TryGetValue(type, out vectorDataType);
    }

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="vectorDataType"/>
    /// represents a numeric element type (integer, floating-point, or the
    /// corresponding NA-aware variant).
    /// </summary>
    public static bool IsNumericType(VectorDataType vectorDataType)
    {
        return s_numericTypes.Contains(vectorDataType);
    }

    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="type"/> represents a
    /// numeric element type (integer, floating-point, or the corresponding
    /// NA-aware wrapper).
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="type"/> is not a
    /// supported element type.</exception>
    public static bool IsNumericType(Type type)
    {
        return IsNumericType(GetDataType(type));
    }

    /// <summary>
    /// Returns <see langword="true"/> when <typeparamref name="T"/> represents
    /// a numeric element type.
    /// </summary>
    /// <exception cref="ArgumentException"><typeparamref name="T"/> is not a
    /// supported element type.</exception>
    public static bool IsNumericType<T>()
    {
        return IsNumericType(typeof(T));
    }
}
