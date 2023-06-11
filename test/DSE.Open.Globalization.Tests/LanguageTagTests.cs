// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Text.Json;
using DSE.Open.Collections.Generic;
using DSE.Open.Text.Json;

namespace DSE.Open.Globalization.Tests;

public class LanguageTagTests
{
    [Theory]
    [MemberData(nameof(ValidLanguageTags))]
    public void TryFromValue_accepts_valid_language_tags(string tag)
    {
        Assert.True(AsciiString.TryParse(tag, default, out var asciiTag));
        Assert.True(LanguageTag.TryFromValue(asciiTag, out _));
    }

    [Theory]
    [MemberData(nameof(ValidLanguageTags))]
    public void Parse_succeeds_with_valid_language_tags(string tag)
    {
        var langTag = LanguageTag.Parse(tag, default);
        Assert.Equal(tag, langTag.ToString());
    }

    [Theory]
    [MemberData(nameof(ValidLanguageTags))]
    public void Serialize_to_json_string(string tag)
    {
        var json = JsonSerializer.Serialize(LanguageTag.Parse(tag, default), JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal($"\"{tag}\"", json);
    }

    [Theory]
    [MemberData(nameof(ValidLanguageTags))]
    public void Deserialize_from_json_string(string tag)
    {
        var langTag = JsonSerializer.Deserialize<LanguageTag>($"\"{tag}\"", JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(LanguageTag.Parse(tag, default), langTag);
    }

    [Theory]
    [InlineData("en-GB", "en")]
    [InlineData("en", "en")]
    [InlineData("zh-CN", "zh")]
    [InlineData("zh", "zh")]
    [InlineData("uz-Latn-UZ", "uz")]
    public void GetLanguagePart(string code, string part)
        => Assert.Equal(part, LanguageTag.Parse(code, default).GetLanguagePart().ToString());

    private static readonly string[] s_validLanguageCodes =
    {
            "af", "af-ZA", "sq", "sq-AL", "gsw", "gsw-FR", "am",
            "am-ET", "ar", "ar-DZ", "ar-BH", "ar-EG", "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA", "ar-OM",
            "ar-QA", "ar-SA", "ar-SY", "ar-TN", "ar-AE", "ar-YE", "hy", "hy-AM", "as", "as-IN", "az", "az-Cyrl",
            "az-Cyrl-AZ", "az-Latn", "az-Latn-AZ", "ba", "ba-RU", "eu", "eu-ES", "be", "be-BY", "bn", "bn-BD", "bn-IN",
            "bs", "bs-Cyrl", "bs-Cyrl-BA", "bs-Latn", "bs-Latn-BA", "br", "br-FR", "bg", "bg-BG", "ca", "ca-ES", "zh",
            "zh-Hans", "zh-CN", "zh-SG", "zh-Hant", "zh-HK", "zh-MO", "zh-TW", "co", "co-FR", "hr", "hr-HR", "hr-BA",
            "cs", "cs-CZ", "da", "da-DK", "prs", "prs-AF", "dv", "dv-MV", "nl", "nl-BE", "nl-NL", "en", "en-AU", "en-BZ",
            "en-CA", "en-029", "en-IN", "en-IE", "en-JM", "en-MY", "en-NZ", "en-PH", "en-SG", "en-ZA", "en-TT", "en-GB",
            "en-US", "en-ZW", "et", "et-EE", "fo", "fo-FO", "fil", "fil-PH", "fi", "fi-FI", "fr", "fr-BE", "fr-CA",
            "fr-FR", "fr-LU", "fr-MC", "fr-CH", "fy", "fy-NL", "gl", "gl-ES", "ka", "ka-GE", "de", "de-AT", "de-DE",
            "de-LI", "de-LU", "de-CH", "el", "el-GR", "kl", "kl-GL", "gu", "gu-IN", "ha", "ha-Latn", "ha-Latn-NG", "he",
            "he-IL", "hi", "hi-IN", "hu", "hu-HU", "is", "is-IS", "ig", "ig-NG", "id", "id-ID", "iu", "iu-Latn",
            "iu-Latn-CA", "iu-Cans", "iu-Cans-CA", "ga", "ga-IE", "xh", "xh-ZA", "zu", "zu-ZA", "it", "it-IT", "it-CH",
            "ja", "ja-JP", "kn", "kn-IN", "kk", "kk-KZ", "km", "km-KH", "qut", "qut-GT", "rw", "rw-RW", "sw", "sw-KE",
            "kok", "kok-IN", "ko", "ko-KR", "ky", "ky-KG", "lo", "lo-LA", "lv", "lv-LV", "lt", "lt-LT", "dsb", "dsb-DE",
            "lb", "lb-LU", "mk-MK", "mk", "ms", "ms-BN", "ms-MY", "ml", "ml-IN", "mt", "mt-MT", "mi", "mi-NZ", "arn",
            "arn-CL", "mr", "mr-IN", "moh", "moh-CA", "mn", "mn-Cyrl", "mn-MN", "mn-Mong", "mn-Mong-CN", "ne", "ne-NP",
            "no", "nb", "nn", "nb-NO", "nn-NO", "oc", "oc-FR", "or", "or-IN", "ps", "ps-AF", "fa", "fa-IR", "pl",
            "pl-PL", "pt", "pt-BR", "pt-PT", "pa", "pa-IN", "quz", "quz-BO", "quz-EC", "quz-PE", "ro", "ro-RO", "rm",
            "rm-CH", "ru", "ru-RU", "smn", "smj", "se", "sms", "sma", "smn-FI", "smj-NO", "smj-SE", "se-FI", "se-NO",
            "se-SE", "sms-FI", "sma-NO", "sma-SE", "sa", "sa-IN", "gd", "gd-GB", "sr", "sr-Cyrl", "sr-Cyrl-BA",
            "sr-Cyrl-ME", "sr-Cyrl-CS", "sr-Cyrl-RS", "sr-Latn", "sr-Latn-BA", "sr-Latn-ME", "sr-Latn-CS", "sr-Latn-RS",
            "nso", "nso-ZA", "tn", "tn-ZA", "si", "si-LK", "sk", "sk-SK", "sl", "sl-SI", "es", "es-AR", "es-BO", "es-CL",
            "es-CO", "es-CR", "es-DO", "es-EC", "es-SV", "es-GT", "es-HN", "es-MX", "es-NI", "es-PA", "es-PY", "es-PE",
            "es-PR", "es-ES", "es-US", "es-UY", "es-VE", "sv", "sv-FI", "sv-SE", "syr", "syr-SY", "tg", "tg-Cyrl",
            "tg-Cyrl-TJ", "tzm", "tzm-Latn", "tzm-Latn-DZ", "ta", "ta-IN", "tt", "tt-RU", "te", "te-IN", "th", "th-TH",
            "bo", "bo-CN", "tr", "tr-TR", "tk", "tk-TM", "uk", "uk-UA", "hsb", "hsb-DE", "ur", "ur-PK", "ug", "ug-CN",
            "uz-Cyrl", "uz-Cyrl-UZ", "uz", "uz-Latn", "uz-Latn-UZ", "vi", "vi-VN", "cy", "cy-GB", "wo", "wo-SN", "sah",
            "sah-RU", "ii", "ii-CN", "yo", "yo-NG", "az-Arab", "az-Cyrl", "az-Latn", "sr-Cyrl", "sr-Latn",
            "uz-Cyrl", "uz-Latn", "zh-Hans", "zh-Hant" //, "zh-Hans_HK"
        };

    private static readonly string[] s_2AlphaCodes =
    {
        "af", "am", "ar", "as", "az", "ba", "be", "bg", "bn", "bo", "br", "bs", "ca", "co", "cs", "cy", "da",
        "de", "dv", "el", "en", "es", "et", "eu", "fa", "ff", "fi", "fo", "fr", "fy", "ga", "gd", "gl",
        "gn", "gu", "ha", "he", "hi", "hr", "hu", "hy", "id", "ig", "ii", "is", "it", "iu", "ja", "jv", "ka",
        "kk", "kl", "km", "kn", "ko", "ku", "ky", "lb", "lo", "lt", "lv", "mg", "mi", "mk", "ml", "mn", "mr",
        "ms", "mt", "my", "nb", "ne", "nl", "nn", "oc", "om", "or", "pa", "pl", "prs", "ps", "pt", "rm", "ro",
        "ru", "rw", "sa", "sd", "se", "si", "sk", "sl", "sn", "so", "sq", "sr", "st", "sv", "sw", "ta",
        "te", "tg", "th", "ti", "tk", "tn", "tr", "ts", "tt", "ug", "uk", "ur", "uz", "vi", "wo", "xh", "yo",
        "zh", "zu"
    };

    public static TheoryData<string> ValidLanguageTags
    {
        get
        {
            var result = new TheoryData<string>();
            foreach (var tag in s_2AlphaCodes)
            {
                result.Add(tag);
            }

            foreach (var tag in s_validLanguageCodes)
            {
                result.Add(tag);
            }

            return result;
        }
    }

    public static TheoryData<string> InvalidLanguageTags { get; } = new TheoryData<string>
    {
        "de-DE-1901-1901",
    };

    // ---------------------------

    [Fact]
    public void Parse_Valid_Codes()
        => s_validLanguageCodes.ForEach(c => Assert.True(LanguageTag.Parse(c).ToString() == c, "Could not parse language code: " + c));

    [Fact]
    public void TryParse_Valid_Codes()
        => s_validLanguageCodes.ForEach(c => Assert.True(LanguageTag.TryParse(c, out _), "Could not parse language code: " + c));

    [Fact]
    public void Parse_Valid_Alpha2Codes()
        => s_2AlphaCodes.ForEach(c => Assert.True(LanguageTag.Parse(c).ToString() == c, "Could not parse language code: " + c));

    [Fact]
    public void TryParse_Valid_Alpha2Codes()
        => s_2AlphaCodes.ForEach(c => Assert.True(LanguageTag.TryParse(c, out _), "Could not parse language code: " + c));

    [Theory]
    [MemberData(nameof(ValidLanguageTags))]
    public void Equal_Equivalent_Languages(string tag)
        => Assert.Equal(LanguageTag.Parse(tag), LanguageTag.Parse(tag));

    [Theory]
    [MemberData(nameof(ValidLanguageTags))]
    public void Equal_Equivalent_Languages_case_different(string tag)
        => Assert.Equal(LanguageTag.Parse(tag), LanguageTag.Parse(tag.ToUpperInvariant()));

    [Fact]
    public void Equal_NonEquivalent_Languages()
    {
        for (var i = 1; i < s_validLanguageCodes.Length; i++)
        {
            Assert.NotEqual(s_validLanguageCodes[i - 1], s_validLanguageCodes[i]);
        }
    }

    [Theory]
    [MemberData(nameof(ValidLanguageTags))]
    public void GetHashCode_Equivalent_Languages(string tag)
        => Assert.Equal(LanguageTag.Parse(tag).GetHashCode(), LanguageTag.Parse(tag).GetHashCode());

    [Theory]
    [MemberData(nameof(ValidLanguageTags))]
    public void GetHashCode_Equivalent_Languages_case_different(string tag)
        => Assert.Equal(LanguageTag.Parse(tag).GetHashCode(), LanguageTag.Parse(tag.ToUpperInvariant()).GetHashCode());

    [Fact]
    public void ToString_as_initialised()
        => s_validLanguageCodes.ForEach(s => Assert.Equal(s, LanguageTag.Parse(s).ToString()));

    [Fact]
    public void Parse_WithNullString_ShouldThrowArgumentNull()
    {
        // Act
        static void Parse()
        {
            _ = LanguageTag.Parse(null!);
        }

        // Assert
        _ = Assert.Throws<ArgumentNullException>(Parse);
    }

    [Fact]
    public void Parse_WithInvalidCode_ShouldThrowException()
    {
        // Arrange
        const string code = "invalid1";

        // Act
        static void Parse()
        {
            _ = LanguageTag.Parse(code, null);
        }

        // Assert
        _ = Assert.Throws<ArgumentOutOfRangeException>(Parse);
    }

    [Fact]
    public void TryParse_WithInvalidCode_ShouldReturnFalse()
    {
        // Arrange
        const string code = "invalid1";

        // Act
        var success = LanguageTag.TryParse(code, null, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal((LanguageTag)default, result);
    }

    [Fact]
    public void TryParse_WithEmptySpan_ShouldReturnFalseAndDefaultResult()
    {
        // Act
        var success = LanguageTag.TryParse(Span<char>.Empty, null, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal((LanguageTag)default, result);
    }

    [Fact]
    public void TryParse_WithNullString_ShouldReturnFalseAndDefaultResult()
    {
        // Act
        var success = LanguageTag.TryParse(null, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal((LanguageTag)default, result);
    }

    [Fact]
    public void TryFormat_WithInvalidBuffer_ShouldReturnFalse()
    {
        // Arrange
        var countryCode = LanguageTag.EnglishUk;
        Span<char> destination = stackalloc char[1];

        // Act
        var result = countryCode.TryFormat(destination, out var charsWritten, null, null);

        // Assert
        Assert.False(result);
        Assert.Equal(0, charsWritten);
    }

    [Fact]
    public void TryFormat_WithValidBuffer_ShouldFormatCorrectly()
    {
        // Arrange.
        var code = LanguageTag.EnglishUk;
        Span<char> buffer = stackalloc char[5];

        // Act
        var success = code.TryFormat(buffer, out var charsWritten, default, default);

        // Assert
        Assert.True(success);
        Assert.Equal(5, charsWritten);
        Assert.Equal("en-GB", buffer.ToString());
    }

    // https://www.rfc-editor.org/rfc/rfc5646.html#appendix-A
    [Theory]
    [InlineData("zh-cmn-Hans-CN")]
    [InlineData("zh-Hans-CN")]
    [InlineData("sl-rozaj-biske")]
    [InlineData("de-CH-1901")]
    [InlineData("sl-IT-nedis")]
    [InlineData("de-CH-x-phonebk")]
    [InlineData("az-Arab-x-AZE-derbend")]
    [InlineData("qaa-Qaaa-QM-x-southern")]
    public void ToStringFormatted_WithCodeWithManyParts_ShouldReturnCorrect(string expected)
    {
        // Arrange
        var code = LanguageTag.Parse(expected);

        // Act
        var result = code.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FromCultureInfo_WithValid_ShouldReturnCorrectCode()
    {
        // Arrange
        var ci = new CultureInfo("en-IE");

        // Act
        var code = LanguageTag.FromCultureInfo(ci);

        // Assert
        Assert.Equal(LanguageTag.EnglishIreland, code);
    }

    [Fact]
    public void Equal_operator_when_case_different()
    {
        var t1 = LanguageTag.Parse("en-GB");
        var t2 = LanguageTag.Parse("en-gb");
        Assert.True(t1 == t2);
    }
}
