// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Security.Cryptography;

namespace DSE.Open;

public static class RandomNumberHelper
{
    public static ulong GetJsonSafeInteger()
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        RandomNumberGenerator.Fill(bytes);
        return (ulong)(BitConverter.ToUInt64(bytes) / (double)ulong.MaxValue * NumberHelper.MaxJsonSafeInteger);
    }
}
