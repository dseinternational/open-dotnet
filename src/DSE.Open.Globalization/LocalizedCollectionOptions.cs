// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Globalization;

/// <summary>
/// Provides global options used by the localized collection extensions, including the default
/// fallback language tags applied when an exact match is not found.
/// </summary>
public static class LocalizedCollectionOptions
{
    private static LanguageTag[] s_defaultFallbacks = [
        LanguageTag.EnglishUk,
        LanguageTag.EnglishUs,
        LanguageTag.English
    ];

    /// <summary>
    /// Gets the default fallback language tags, applied in order when a localized collection lookup
    /// does not find an exact or language-part match.
    /// </summary>
    public static IList<LanguageTag> DefaultFallbacks => s_defaultFallbacks;

    /// <summary>
    /// Sets the default fallback language tags returned by <see cref="DefaultFallbacks"/>.
    /// </summary>
    /// <param name="languageTags">The language tags to use as fallbacks. Must contain at least one item.</param>
    public static void SetDefaultFallbacks(ICollection<LanguageTag> languageTags)
    {
        ArgumentNullException.ThrowIfNull(languageTags);
        Guard.HasSizeGreaterThanOrEqualTo(languageTags, 1);

        s_defaultFallbacks = [.. languageTags];
    }
}
