// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class FixedHashCode
{
    /// <summary>
    /// Gets a deterministic hash of the a span of characters. Unlike the hash functions
    /// built into .NET, there is no randomised element and therefore the value will
    /// remain fixed between platforms, processes, etc.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int GetFixedHashCode(this ReadOnlySpan<char> value)
    {
        // Source:
        // https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/

        var hash1 = (5381 << 16) + 5381;
        var hash2 = hash1;

        for (var i = 0; i < value.Length; i += 2)
        {
            hash1 = (hash1 << 5) + hash1 ^ value[i];

            if (i == value.Length - 1)
            {
                break;
            }

            hash2 = (hash2 << 5) + hash2 ^ value[i + 1];
        }

        return hash1 + hash2 * 1566083941;
    }
}
