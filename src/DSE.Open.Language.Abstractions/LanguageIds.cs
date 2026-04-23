// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Language;

/// <summary>
/// Defines the numeric range used by language identifier value types
/// (<see cref="SentenceId"/>, <see cref="SentenceMeaningId"/>,
/// <see cref="WordId"/> and <see cref="WordMeaningId"/>).
/// </summary>
/// <remarks>
/// Identifiers are 12-digit positive integers, allowing values to be
/// represented by the same number of characters regardless of magnitude.
/// </remarks>
public static class LanguageIds
{
    /// <summary>
    /// The smallest valid identifier value (<c>100000000001</c>).
    /// </summary>
    public const ulong MinIdValue = 100000000001;

    /// <summary>
    /// The largest valid identifier value (<c>999999999999</c>).
    /// </summary>
    public const ulong MaxIdValue = 999999999999;

    /// <summary>
    /// The size of the inclusive range of valid values, expressed as
    /// <see cref="MaxIdValue"/> minus <see cref="MinIdValue"/>.
    /// </summary>
    public const ulong MaxRange = MaxIdValue - MinIdValue;
}
