// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Reflection-based helpers for inspecting <see cref="Type"/> values.
/// </summary>
public static class TypeHelper
{
    /// <summary>
    /// Indicates whether the supplied type is a closed <see cref="Nullable{T}"/> value type
    /// (for example, <c>int?</c>). Reference types are always <see langword="false"/> —
    /// use the nullable-reference-type annotations to reason about those.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    public static bool IsNullableType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    /// Indicates whether the supplied type is one of the built-in numeric primitives —
    /// the signed and unsigned integers, <see cref="float"/>, <see cref="double"/>, or
    /// <see cref="decimal"/>. <see cref="Nullable{T}"/> wrappers return <see langword="false"/>.
    /// </summary>
    /// <param name="type">The type to inspect.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    public static bool IsNumericType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return Type.GetTypeCode(type) switch
        {
            TypeCode.Byte => true,
            TypeCode.SByte => true,
            TypeCode.UInt16 => true,
            TypeCode.Int16 => true,
            TypeCode.UInt32 => true,
            TypeCode.Int32 => true,
            TypeCode.UInt64 => true,
            TypeCode.Int64 => true,
            TypeCode.Single => true,
            TypeCode.Double => true,
            TypeCode.Decimal => true,
            TypeCode.Empty => false,
            TypeCode.Object => false,
            TypeCode.DBNull => false,
            TypeCode.Boolean => false,
            TypeCode.Char => false,
            TypeCode.DateTime => false,
            TypeCode.String => false,
            _ => false,
        };
    }
}
