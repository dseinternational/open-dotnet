// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing;

/// <summary>
/// Produces <see cref="Timestamp"/> values for test data. Both members are thread-safe.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5394:Do not use insecure randomness",
    Justification = "Not security sensitive")]
public static class TimestampGenerator
{
    private static long s_timestamp = Random.Shared.NextInt64();

    /// <summary>
    /// Returns the next <see cref="Timestamp"/> in a monotonically increasing sequence.
    /// Increments atomically; the underlying counter wraps on overflow.
    /// </summary>
    public static Timestamp GetNext()
    {
        var next = Interlocked.Increment(ref s_timestamp);
        return new(BitConverter.GetBytes(next));
    }

    /// <summary>
    /// Returns a random <see cref="Timestamp"/> unrelated to the counter used by
    /// <see cref="GetNext"/>.
    /// </summary>
    public static Timestamp GetRandom()
    {
        return new(BitConverter.GetBytes(Random.Shared.NextInt64()));
    }
}
