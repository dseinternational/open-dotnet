// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers.Binary;
using System.Security.Cryptography;

namespace DSE.Open.Security;

/// <summary>
/// Generates cryptographically random values using <see cref="RandomNumberGenerator"/>.
/// </summary>
public static class RandomValueGenerator
{
    private const string DefaultStringValueCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";

    /// <summary>
    /// Generates a random alphanumeric string of the specified length (default 16) using a default
    /// alphabet of upper and lower case ASCII letters and the digits 1-9.
    /// </summary>
    public static string GetStringValue(int? length = null)
    {
        return GetStringValue(length ?? 16, DefaultStringValueCharacters);
    }

    /// <summary>
    /// Generates a random string of the specified length using the supplied set of valid characters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if <paramref name="validCharacters"/> is null, empty,
    /// whitespace, or contains fewer than 9 characters.</exception>
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

    /// <summary>
    /// Returns a random non-negative <see cref="int"/> in the range <c>[0, <see cref="int.MaxValue"/>)</c>.
    /// </summary>
    public static int GetInt32Value()
    {
        return RandomNumberGenerator.GetInt32(int.MaxValue);
    }

    /// <summary>
    /// Returns a random <see cref="int"/> in the range <c>[<paramref name="minimum"/>, <paramref name="maximum"/>)</c>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="minimum"/> is not less than <paramref name="maximum"/>.</exception>
    public static int GetInt32Value(int minimum, int maximum)
    {
        return minimum >= maximum
            ? throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be smaller than maximum.")
            : RandomNumberGenerator.GetInt32(minimum, maximum);
    }

    /// <summary>
    /// Returns a random positive <see cref="int"/> in the range <c>[1, <see cref="int.MaxValue"/>)</c>.
    /// </summary>
    public static int GetPositiveInt32Value()
    {
        return GetInt32Value(1, int.MaxValue);
    }

    /// <summary>
    /// Returns a random <see cref="long"/> in the full range <c>[<see cref="long.MinValue"/>, <see cref="long.MaxValue"/>)</c>.
    /// </summary>
    public static long GetInt64Value()
    {
        return GetInt64Value(long.MinValue, long.MaxValue);
    }

    /// <summary>
    /// Returns a random positive <see cref="long"/> in the range <c>[1, <see cref="long.MaxValue"/>)</c>.
    /// </summary>
    public static long GetPositiveInt64Value()
    {
        return GetInt64Value(1L, long.MaxValue);
    }

    /// <summary>
    /// Returns a random <see cref="long"/> in the range <c>[<paramref name="fromInclusive"/>, <paramref name="toExclusive"/>)</c>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="fromInclusive"/> is not less than <paramref name="toExclusive"/>.</exception>
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

    /// <summary>
    /// Returns a random <see cref="ulong"/> in the range <c>[<paramref name="fromInclusive"/>, <paramref name="toExclusive"/>)</c>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="fromInclusive"/> is not less than <paramref name="toExclusive"/>.</exception>
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

    /// <summary>
    /// Returns a random <see cref="ulong"/> in the range <c>[0, <see cref="NumberHelper.MaxJsonSafeInteger"/>]</c>,
    /// safe to round-trip through JSON without loss of precision.
    /// </summary>
    public static ulong GetJsonSafeUInt64()
    {
        return GetJsonSafeUInt64(0, (ulong)NumberHelper.MaxJsonSafeInteger + 1);
    }

    /// <summary>
    /// Returns a random <see cref="ulong"/> in the range <c>[<paramref name="fromInclusive"/>, <paramref name="toExclusive"/>)</c>,
    /// constrained so that the upper bound does not exceed <see cref="NumberHelper.MaxJsonSafeInteger"/>.
    /// </summary>
    public static ulong GetJsonSafeUInt64(ulong fromInclusive, ulong toExclusive)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(toExclusive, (ulong)NumberHelper.MaxJsonSafeInteger + 1);
        return GetUInt64Value(fromInclusive, toExclusive);
    }
}
