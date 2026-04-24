// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers.Binary;
using System.Security.Cryptography;

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

        return string.Create(length, validCharacters, static (chars, characters) =>
        {
            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = characters[RandomNumberGenerator.GetInt32(characters.Length)];
            }
        });
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
        if (fromInclusive >= toExclusive)
        {
            throw new ArgumentOutOfRangeException(nameof(toExclusive), "Maximum must be greater than minimum.");
        }

        var range = unchecked((ulong)(toExclusive - fromInclusive));
        var offset = GetUInt64ValueBelow(range);
        return unchecked(fromInclusive + (long)offset);
    }

    public static ulong GetUInt64Value(ulong fromInclusive, ulong toExclusive)
    {
        if (fromInclusive >= toExclusive)
        {
            throw new ArgumentOutOfRangeException(nameof(toExclusive), "Maximum must be greater than minimum.");
        }

        var range = toExclusive - fromInclusive;
        return fromInclusive + GetUInt64ValueBelow(range);
    }

    private static ulong GetUInt64ValueBelow(ulong exclusiveUpperBound)
    {
        ArgumentOutOfRangeException.ThrowIfZero(exclusiveUpperBound);

        var range = exclusiveUpperBound - 1;

        if (range == 0)
        {
            return 0;
        }

        var mask = range;
        mask |= mask >> 1;
        mask |= mask >> 2;
        mask |= mask >> 4;
        mask |= mask >> 8;
        mask |= mask >> 16;
        mask |= mask >> 32;

        Span<byte> randomBytes = stackalloc byte[sizeof(ulong)];
        ulong result;

        do
        {
            RandomNumberGenerator.Fill(randomBytes);
            result = BinaryPrimitives.ReadUInt64LittleEndian(randomBytes) & mask;
        }
        while (result > range);

        return result;
    }

    public static ulong GetJsonSafeUInt64()
    {
        return GetJsonSafeUInt64(0, (ulong)NumberHelper.MaxJsonSafeInteger + 1);
    }

    public static ulong GetJsonSafeUInt64(ulong fromInclusive, ulong toExclusive)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(toExclusive, (ulong)NumberHelper.MaxJsonSafeInteger + 1);
        return GetUInt64Value(fromInclusive, toExclusive);
    }
}
