// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Testing;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5394:Do not use insecure randomness",
    Justification = "Not security sensitive")]
public static class TimestampGenerator
{
    private static ulong s_timestamp = (ulong)Random.Shared.NextInt64();

    public static Timestamp GetNext()
    {
        if (s_timestamp == ulong.MaxValue)
        {
            s_timestamp = 0;
        }

        return new(BitConverter.GetBytes(s_timestamp++));
    }

    public static Timestamp GetRandom()
    {
        return new(BitConverter.GetBytes((ulong)Random.Shared.NextInt64()));
    }
}
