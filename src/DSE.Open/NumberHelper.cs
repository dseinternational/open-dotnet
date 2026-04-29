// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open;

/// <summary>
/// Helpers for working with numeric types.
/// </summary>
public static partial class NumberHelper
{
    /// <summary>
    /// The largest integer that can be exactly represented by IEEE 754 double precision (2^53 - 1).
    /// </summary>
    // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Number/MAX_SAFE_INTEGER
    public const long MaxJsonSafeInteger = 9007199254740991;

    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="value"/> is less than or equal to
    /// <see cref="MaxJsonSafeInteger"/>.
    /// </summary>
    public static bool IsJsonSafeInteger(ulong value)
    {
        return value <= MaxJsonSafeInteger;
    }

    private static readonly FrozenSet<Type> s_knownNumberTypes = FrozenSet.Create(
    [
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(Int128),
        typeof(UInt128),
        typeof(Half),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(DateTime64),
    ]);

    /// <summary>
    /// Determines if the specified type is known to implement <see cref="INumber{TSelf}"/>.
    /// This is hardcoded to avoid reflection and possible trimming issues.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsKnownNumberType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return s_knownNumberTypes.Contains(type);
    }

    /// <summary>
    /// Determines if the specified type is known to implement <see cref="INumber{TSelf}"/>.
    /// This is hardcoded to avoid reflection and possible trimming issues.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsKnownNumberType<T>()
    {
        return IsKnownNumberType(typeof(T));
    }

    /// <summary>
    /// Determines if <paramref name="type"/> is a known floating-point type (IEEE 754 or <see cref="decimal"/>).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return IsKnownFloatingPointIeee754Type(type) || type == typeof(decimal);
    }

    /// <summary>
    /// Determines if <typeparamref name="T"/> is a known floating-point type (IEEE 754 or <see cref="decimal"/>).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointType<T>()
    {
        return IsKnownFloatingPointType(typeof(T));
    }

    /// <summary>
    /// Determines if <paramref name="type"/> is one of the known IEEE 754 floating-point types
    /// (<see cref="float"/>, <see cref="double"/>, <see cref="Half"/>).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointIeee754Type(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type == typeof(float) || type == typeof(double) || type == typeof(Half);
    }

    /// <summary>
    /// Determines if <typeparamref name="T"/> is one of the known IEEE 754 floating-point types
    /// (<see cref="float"/>, <see cref="double"/>, <see cref="Half"/>).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointIeee754Type<T>()
    {
        return IsKnownFloatingPointIeee754Type(typeof(T));
    }
}
