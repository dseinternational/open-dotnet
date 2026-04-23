// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

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

    /// <summary>The number of representable identifier values, equal to <see cref="MaxIdValue"/> minus <see cref="MinIdValue"/>.</summary>
    public const ulong MaxRange = MaxIdValue - MinIdValue;
}
