// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers.Binary;
using System.Security.Cryptography;

namespace DSE.Open.Language;

/// <summary>
/// Bounds for the numeric identifiers used by language entities such as
/// <see cref="WordId"/>, <see cref="WordMeaningId"/>, <see cref="SentenceId"/> and
/// <see cref="SentenceMeaningId"/>.
/// </summary>
public static class LanguageIds
{
    /// <summary>The smallest valid identifier value.</summary>
    public const ulong MinIdValue = 100000000001;

    /// <summary>The largest valid identifier value.</summary>
    public const ulong MaxIdValue = 999999999999;

    /// <summary>The numeric span between the valid identifier bounds, equal to <see cref="MaxIdValue"/> minus <see cref="MinIdValue"/>.</summary>
    public const ulong MaxRange = MaxIdValue - MinIdValue;

    internal static ulong GetRandomIdValue()
    {
        const ulong range = MaxRange + 1;
        var limit = ulong.MaxValue - (ulong.MaxValue % range);

        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        ulong value;

        do
        {
            RandomNumberGenerator.Fill(bytes);
            value = BinaryPrimitives.ReadUInt64LittleEndian(bytes);
        }
        while (value >= limit);

        return MinIdValue + (value % range);
    }
}
