// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class TypeHelper
{
    public static bool IsNullableType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

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
