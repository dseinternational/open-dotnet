// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// Conversion extensions over generic-math <see cref="INumber{T}"/> values that
/// surface the three standard <c>Create*</c> conversion modes (checked,
/// saturating, truncating) for the most common integer targets.
/// </summary>
public static class NumberExtensions
{
    /// <summary>Converts <paramref name="value"/> to <see cref="int"/> with overflow checking.</summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is outside <see cref="int"/>'s range.</exception>
    public static int ToInt32Checked<T>(this T value)
        where T : struct, INumber<T>
    {
        return int.CreateChecked(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="int"/>, saturating at <see cref="int.MinValue"/>/<see cref="int.MaxValue"/> on overflow.</summary>
    public static int ToInt32Saturating<T>(this T value)
        where T : struct, INumber<T>
    {
        return int.CreateSaturating(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="int"/>, truncating bits on overflow.</summary>
    public static int ToInt32Truncating<T>(this T value)
        where T : struct, INumber<T>
    {
        return int.CreateTruncating(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="uint"/> with overflow checking.</summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is outside <see cref="uint"/>'s range.</exception>
    public static uint ToUInt32Checked<T>(this T value)
        where T : struct, INumber<T>
    {
        return uint.CreateChecked(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="uint"/>, saturating at <c>0</c>/<see cref="uint.MaxValue"/> on overflow.</summary>
    public static uint ToUInt32Saturating<T>(this T value)
        where T : struct, INumber<T>
    {
        return uint.CreateSaturating(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="uint"/>, truncating bits on overflow.</summary>
    public static uint ToUInt32Truncating<T>(this T value)
        where T : struct, INumber<T>
    {
        return uint.CreateTruncating(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="long"/> with overflow checking.</summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is outside <see cref="long"/>'s range.</exception>
    public static long ToInt64Checked<T>(this T value)
        where T : struct, INumber<T>
    {
        return long.CreateChecked(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="long"/>, saturating at <see cref="long.MinValue"/>/<see cref="long.MaxValue"/> on overflow.</summary>
    public static long ToInt64Saturating<T>(this T value)
        where T : struct, INumber<T>
    {
        return long.CreateSaturating(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="long"/>, truncating bits on overflow.</summary>
    public static long ToInt64Truncating<T>(this T value)
        where T : struct, INumber<T>
    {
        return long.CreateTruncating(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="ulong"/> with overflow checking.</summary>
    /// <exception cref="OverflowException"><paramref name="value"/> is outside <see cref="ulong"/>'s range.</exception>
    public static ulong ToUInt64Checked<T>(this T value)
        where T : struct, INumber<T>
    {
        return ulong.CreateChecked(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="ulong"/>, saturating at <c>0</c>/<see cref="ulong.MaxValue"/> on overflow.</summary>
    public static ulong ToUInt64Saturating<T>(this T value)
        where T : struct, INumber<T>
    {
        return ulong.CreateSaturating(value);
    }

    /// <summary>Converts <paramref name="value"/> to <see cref="ulong"/>, truncating bits on overflow.</summary>
    public static ulong ToUInt64Truncating<T>(this T value)
        where T : struct, INumber<T>
    {
        return ulong.CreateTruncating(value);
    }
}
