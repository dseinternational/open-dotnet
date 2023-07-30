// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5394:Do not use insecure randomness",
    Justification = "Not security sensitive")]
public static class IdGenerator
{
    private static int s_int32Id = Random.Shared.Next(10_000_000);
    private static long s_int64Id = Random.Shared.Next(1_000_000_000);

    public static int GetInt32Id(int minimum = 1)
    {
        var nextId = Interlocked.Increment(ref s_int32Id);

        if (nextId < minimum)
        {
            return nextId + minimum;
        }

        return nextId;
    }

    public static long GetInt64Id(long minimum = 1)
    {
        var nextId = Interlocked.Increment(ref s_int64Id);
        if (nextId < minimum)
        {
            return nextId + minimum;
        }

        return nextId;
    }

    public static string GetRandomHexString(int lengthInBytes = 16)
    {
        var buffer = new byte[lengthInBytes];
        Random.Shared.NextBytes(buffer);
        return Convert.ToHexString(buffer).ToLowerInvariant();
    }
}
