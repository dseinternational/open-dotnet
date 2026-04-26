// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

/// <summary>
/// Type-classification helpers used internally by <c>DSE.Open.Numerics</c> to
/// dispatch on numeric / floating-point / NA-aware element types without
/// resorting to reflection (which is incompatible with AOT/trimming).
/// </summary>
public static partial class NumericsNumberHelper
{
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

        typeof(NaInt<byte>),
        typeof(NaInt<sbyte>),
        typeof(NaInt<short>),
        typeof(NaInt<ushort>),
        typeof(NaInt<int>),
        typeof(NaInt<uint>),
        typeof(NaInt<long>),
        typeof(NaInt<ulong>),
        typeof(NaFloat<Half>),
        typeof(NaFloat<float>),
        typeof(NaFloat<double>),
        typeof(NaInt<DateTime64>),
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

    /// <summary>Returns <see langword="true"/> when <paramref name="type"/> is one of the recognised floating-point types (IEEE 754 plus <see cref="decimal"/>).</summary>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return IsKnownFloatingPointIeee754Type(type) || type == typeof(decimal);
    }

    /// <summary>Generic-arg overload of <see cref="IsKnownFloatingPointType(Type)"/>.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointType<T>()
    {
        return IsKnownFloatingPointType(typeof(T));
    }

    /// <summary>Returns <see langword="true"/> when <paramref name="type"/> is <see cref="float"/>, <see cref="double"/>, or <see cref="Half"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointIeee754Type(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type == typeof(float) || type == typeof(double) || type == typeof(Half);
    }

    /// <summary>Generic-arg overload of <see cref="IsKnownFloatingPointIeee754Type(Type)"/>.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointIeee754Type<T>()
    {
        return IsKnownFloatingPointIeee754Type(typeof(T));
    }
}
