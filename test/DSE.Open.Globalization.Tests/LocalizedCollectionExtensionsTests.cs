// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Globalization.Tests;

public class LocalizedCollectionExtensionsTests
{
    [Fact]
    public void GetOrFallback_collection_gets_match()
    {
        var collection = new[]
        {
            new LocalizedFake { LanguageTag = LanguageTag.EnglishAustralia }
        };

        var result = collection.GetLocalizedOrFallback(LanguageTag.EnglishAustralia);

        Assert.Same(collection[0], result);
    }

    [Fact]
    public void GetOrFallback_collection_gets_match_2()
    {
        var collection = new[]
        {
            new LocalizedFake { LanguageTag = LanguageTag.EnglishAustralia },
            new LocalizedFake { LanguageTag = LanguageTag.EnglishUs },
            new LocalizedFake { LanguageTag = LanguageTag.ParseInvariant("fr-FR") },
        };

        var result = collection.GetLocalizedOrFallback(LanguageTag.EnglishAustralia);

        Assert.Same(collection[0], result);
    }

    [Fact]
    public void GetOrFallback_collection_gets_fallback()
    {
        var collection = new[]
        {
            new LocalizedFake { LanguageTag = LanguageTag.EnglishUs },
            new LocalizedFake { LanguageTag = LanguageTag.ParseInvariant("fr-FR") },
        };

        var result = collection.GetLocalizedOrFallback(LanguageTag.EnglishAustralia);

        Assert.Same(collection[0], result);
    }

    [Fact]
    public void GetOrFallback_collection_gets_fallback_2()
    {
        var collection = new[]
        {
            new LocalizedFake { LanguageTag = LanguageTag.ParseInvariant("fr-FR") },
            new LocalizedFake { LanguageTag = LanguageTag.ParseInvariant("en") },
        };

        var result = collection.GetLocalizedOrFallback(LanguageTag.EnglishAustralia);

        Assert.Same(collection[1], result);
    }

    [Fact]
    public void GetOrFallback_dictionary_gets_match()
    {
        var collection = new Dictionary<LanguageTag, string>
        {
            { LanguageTag.EnglishAustralia, "Australia" }
        };

        var result = collection.GetLocalizedOrFallback(LanguageTag.EnglishAustralia);

        Assert.Same(collection.Values.First(), result);
    }

    [Fact]
    public void GetOrFallback_string_dictionary_gets_match()
    {
        var collection = new Dictionary<string, string>
        {
            { LanguageTag.EnglishAustralia.ToString(), "Australia" }
        };

        var result = collection.GetLocalizedOrFallback(LanguageTag.EnglishAustralia);

        Assert.Same(collection.Values.First(), result);
    }

    [Fact]
    public void GetOrFallback_string_dictionary_gets_match_2()
    {
        var collection = new Dictionary<string, string>
        {
            { LanguageTag.EnglishCanada.ToString(), "Canada" },
            { LanguageTag.EnglishAustralia.ToString(), "Australia" },
            { LanguageTag.EnglishUk.ToString(), "UK" },
            { LanguageTag.EnglishNewZealand.ToString(), "New Zealand" },
        };

        var result = collection.GetLocalizedOrFallback(LanguageTag.EnglishAustralia);

        Assert.Equal("Australia", result);
    }

    private sealed class LocalizedFake : ILocalized
    {
        public LanguageTag LanguageTag
        {
            get => Language;
            set => Language = value;
        }

        public LanguageTag Language { get; set; }

        public LanguageTag FormatLanguage { get; set; }
    }
}
