// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return IsKnownFloatingPointIeee754Type(type) || type == typeof(decimal);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointType<T>()
    {
        return IsKnownFloatingPointType(typeof(T));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointIeee754Type(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type == typeof(float) || type == typeof(double) || type == typeof(Half);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKnownFloatingPointIeee754Type<T>()
    {
        return IsKnownFloatingPointIeee754Type(typeof(T));
    }
}
