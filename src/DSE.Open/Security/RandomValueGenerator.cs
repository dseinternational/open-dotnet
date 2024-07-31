// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace DSE.Open.Security;

public static class RandomValueGenerator
{
    private const string DefaultStringValueCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";

    public static string GetStringValue(int? length = null)
    {
        return GetStringValue(length ?? 16, DefaultStringValueCharacters);
    }

    public static string GetStringValue(int length, string validCharacters)
    {
        Guard.IsGreaterThan(length, 0);
        ArgumentException.ThrowIfNullOrWhiteSpace(validCharacters);
        Guard.IsGreaterThan(validCharacters.Length, 8);

        var data = RandomNumberGenerator.GetBytes(length * 2);
        var sb = new StringBuilder(length);
        for (var i = 0; i < data.Length; i += 2)
        {
            var f = BitConverter.ToUInt16(data, i);
            var r = (double)f / ushort.MaxValue;
            var c = (int)(r * (validCharacters.Length - 1));
            _ = sb.Append(validCharacters[c]);
        }

        return sb.ToString();
    }

    public static int GetInt32Value()
    {
        return RandomNumberGenerator.GetInt32(int.MaxValue);
    }

    public static int GetInt32Value(int minimum, int maximum)
    {
        return minimum >= maximum
            ? throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be smaller than maximum.")
            : RandomNumberGenerator.GetInt32(minimum, maximum);
    }

    public static int GetPositiveInt32Value()
    {
        return GetInt32Value(1, int.MaxValue);
    }

    public static long GetInt64Value()
    {
        return GetInt64Value(long.MinValue, long.MaxValue);
    }

    public static long GetPositiveInt64Value()
    {
        return GetInt64Value(1L, long.MaxValue);
    }

    public static long GetInt64Value(long fromInclusive, long toExclusive)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(toExclusive, fromInclusive);

        var range = toExclusive - fromInclusive - 1;

        if (range == 0)
        {
            return fromInclusive;
        }

        var mask = range;
        mask |= mask >> 1;
        mask |= mask >> 2;
        mask |= mask >> 4;
        mask |= mask >> 8;
        mask |= mask >> 16;
        mask |= mask >> 32;

        long oneInt64 = 0;
        var oneInt64Bytes = MemoryMarshal.AsBytes(new Span<long>(ref oneInt64));
        long result;

        do
        {
            RandomNumberGenerator.Fill(oneInt64Bytes);
            result = mask & oneInt64;
        }
        while (result > range);

        return result + fromInclusive;
    }

    public static ulong GetUInt64Value(ulong fromInclusive, ulong toExclusive)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(toExclusive, fromInclusive);

        var range = toExclusive - fromInclusive - 1;

        if (range == 0)
        {
            return fromInclusive;
        }

        var mask = range;
        mask |= mask >> 1;
        mask |= mask >> 2;
        mask |= mask >> 4;
        mask |= mask >> 8;
        mask |= mask >> 16;
        mask |= mask >> 32;

        ulong oneUInt64 = 0;
        var oneUInt64Bytes = MemoryMarshal.AsBytes(new Span<ulong>(ref oneUInt64));
        ulong result;

        do
        {
            RandomNumberGenerator.Fill(oneUInt64Bytes);
            result = mask & oneUInt64;
        }
        while (result > range);

        return result + fromInclusive;
    }

    public static ulong GetJsonSafeUInt64()
    {
        return GetJsonSafeUInt64(0L, NumberHelper.MaxJsonSafeInteger);
    }

    public static ulong GetJsonSafeUInt64(ulong fromInclusive, ulong toExclusive)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(toExclusive, (ulong)NumberHelper.MaxJsonSafeInteger);
        return GetUInt64Value(fromInclusive, toExclusive);
    }
}
