// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Globalization;

public static class LocalizedCollectionOptions
{
    private static LanguageTag[] s_defaultFallbacks = [
        LanguageTag.EnglishUk,
        LanguageTag.EnglishUs,
        LanguageTag.English
    ];

    public static IList<LanguageTag> DefaultFallbacks => s_defaultFallbacks;

    public static void SetDefaultFallbacks(ICollection<LanguageTag> languageTags)
    {
        ArgumentNullException.ThrowIfNull(languageTags);
        Guard.HasSizeGreaterThanOrEqualTo(languageTags, 1);

        s_defaultFallbacks = [.. languageTags];
    }
}
