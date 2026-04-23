// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing;

/// <summary>
/// Produces monotonically increasing integer identifiers and random hexadecimal strings,
/// suitable for seeding test data. All members are thread-safe.
/// </summary>
/// <remarks>
/// Not intended for production use — the counters are process-local and not persisted.
/// </remarks>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5394:Do not use insecure randomness",
    Justification = "Not security sensitive")]
public static class IdGenerator
{
    private static int s_int32Id = Random.Shared.Next(10_000_000);
    private static long s_int64Id = Random.Shared.Next(1_000_000_000);

    /// <summary>
    /// Returns the next <see cref="int"/> identifier. The value is guaranteed to be at least
    /// <paramref name="minimum"/> (clamped, never duplicated).
    /// </summary>
    /// <param name="minimum">A non-negative lower bound for the returned value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minimum"/> is negative.</exception>
    public static int GetInt32Id(int minimum = 1)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minimum);

        var nextId = Interlocked.Increment(ref s_int32Id);

        if (nextId < minimum)
        {
            return nextId + minimum;
        }

        return nextId;
    }

    /// <summary>
    /// Returns the next <see cref="long"/> identifier. The value is guaranteed to be at least
    /// <paramref name="minimum"/> (clamped, never duplicated).
    /// </summary>
    /// <param name="minimum">A non-negative lower bound for the returned value.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minimum"/> is negative.</exception>
    public static long GetInt64Id(long minimum = 1)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minimum);

        var nextId = Interlocked.Increment(ref s_int64Id);

        if (nextId < minimum)
        {
            return nextId + minimum;
        }

        return nextId;
    }

    /// <summary>
    /// Returns a lowercase hexadecimal string of <c>2 * <paramref name="lengthInBytes"/></c>
    /// characters composed from random bytes.
    /// </summary>
    /// <param name="lengthInBytes">The number of random bytes to generate. Must be non-negative.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="lengthInBytes"/> is negative.</exception>
    public static string GetRandomHexString(int lengthInBytes = 16)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(lengthInBytes);

        var buffer = new byte[lengthInBytes];
        Random.Shared.NextBytes(buffer);
        return Convert.ToHexString(buffer).ToLowerInvariant();
    }
}
