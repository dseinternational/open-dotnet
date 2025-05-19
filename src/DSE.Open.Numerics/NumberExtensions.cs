// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static class NumberExtensions
{
    public static int ToInt32Checked<T>(this T value)
        where T : struct, INumber<T>
    {
        return int.CreateChecked(value);
    }

    public static int ToInt32Saturating<T>(this T value)
        where T : struct, INumber<T>
    {
        return int.CreateSaturating(value);
    }

    public static int ToInt32Truncating<T>(this T value)
        where T : struct, INumber<T>
    {
        return int.CreateTruncating(value);
    }

    public static uint ToUInt32Checked<T>(this T value)
        where T : struct, INumber<T>
    {
        return uint.CreateChecked(value);
    }
    public static uint ToUInt32Saturating<T>(this T value)
        where T : struct, INumber<T>
    {
        return uint.CreateSaturating(value);
    }

    public static uint ToUInt32Truncating<T>(this T value)
        where T : struct, INumber<T>
    {
        return uint.CreateTruncating(value);
    }

    public static long ToInt64Checked<T>(this T value)
        where T : struct, INumber<T>
    {
        return long.CreateChecked(value);
    }

    public static long ToInt64Saturating<T>(this T value)
        where T : struct, INumber<T>
    {
        return long.CreateSaturating(value);
    }

    public static long ToInt64Truncating<T>(this T value)
        where T : struct, INumber<T>
    {
        return long.CreateTruncating(value);
    }

    public static ulong ToUInt64Checked<T>(this T value)
        where T : struct, INumber<T>
    {
        return ulong.CreateChecked(value);
    }
    public static ulong ToUInt64Saturating<T>(this T value)
        where T : struct, INumber<T>
    {
        return ulong.CreateSaturating(value);
    }

    public static ulong ToUInt64Truncating<T>(this T value)
        where T : struct, INumber<T>
    {
        return ulong.CreateTruncating(value);
    }
}
