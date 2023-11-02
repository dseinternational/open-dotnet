// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Globalization;
using DSE.Open.Text.Json;

namespace DSE.Open.Language.Abstractions.Tests;

public class SignMeaningTests
{
    [Fact]
    public void FormatParse()
    {
        foreach (var m in s_meanings)
        {
            var str = m.ToString();
            var parsed = SignMeaning.Parse(str, CultureInfo.InvariantCulture);

            Assert.NotNull(parsed);
            Assert.Equal(m, parsed);
        }
    }

    [Fact]
    public void SerializeDeserialize()
    {
        foreach (var m in s_meanings)
        {
            var json = JsonSerializer.Serialize(m, JsonSharedOptions.RelaxedJsonEscaping);
            var deserialized = JsonSerializer.Deserialize<SignMeaning>(json, JsonSharedOptions.RelaxedJsonEscaping);

            Assert.NotNull(deserialized);
            Assert.Equal(m, deserialized);
        }
    }

    [Fact]
    public void GeneratesKey()
    {
        foreach (var m in s_meanings)
        {
            var key = m.Key;
            var expected = $"{m.Sign}|{m.Language}|{m.PosTag}|{m.PosDetailedTag}";
            Assert.Equal(expected, key);
        }
    }

    private static readonly SignMeaning[] s_meanings =
    [
        new SignMeaning
        {
            Sign = new Sign { Modality = SignModality.Spoken, Word = new Word("shoes") },
            Language = LanguageTag.EnglishUk,
            PosTag = UniversalPosTag.Noun,
            PosDetailedTag = TreebankPosTag.NounProperPlural,
        },
        new SignMeaning
        {
            Sign = new Sign { Modality = SignModality.Spoken, Word = new Word("elephant") },
            Language = LanguageTag.EnglishUk,
            PosTag = UniversalPosTag.Noun,
            PosDetailedTag = TreebankPosTag.NounProperSingular,
        },
        new SignMeaning
        {
            Sign = new Sign { Modality = SignModality.Spoken, Word = new Word("butterfly") },
            Language = LanguageTag.EnglishUs,
            PosTag = UniversalPosTag.Noun,
            PosDetailedTag = TreebankPosTag.NounProperSingular,
        },
    ];
}
