// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Globalization;

namespace DSE.Open.Language;

public sealed class SentenceIdTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var value = (SentenceId)258089004501;
        var json = JsonSerializer.Serialize(value);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Fact]
    public void GetRandomId_ReturnsValidValue()
    {
        for (var i = 0; i < 1000; i++)
        {
            var id = SentenceId.GetRandomId();
            Assert.True(SentenceId.IsValidValue((long)id));
        }
    }

    [Fact]
    public void ToInt64_round_trips_via_FromInt64()
    {
        var id = SentenceId.FromUInt64(258089004501ul);
        var i = id.ToInt64();
        Assert.Equal(id, SentenceId.FromInt64(i));
    }

    [Fact]
    public void ToUInt64_round_trips_via_FromUInt64()
    {
        var id = SentenceId.FromUInt64(258089004501ul);
        var u = id.ToUInt64();
        Assert.Equal(id, SentenceId.FromUInt64(u));
    }

    // Pins a specific hash output so any future change to the serialization
    // format is caught on every platform. Regression guard for #354 item 4.
    [Theory]
    [InlineData(100000000001ul, "en-GB", "The cat sat on the mat.", 360182216922ul)]
    [InlineData(500000000042ul, "es-ES", "El gato se sentó en la alfombra.", 820487636515ul)]
    public void FromSentence_produces_platform_pinned_id(
        ulong meaningIdValue,
        string languageTag,
        string sentence,
        ulong expectedId)
    {
        var meaning = (SentenceMeaningId)meaningIdValue;
        var language = LanguageTag.FromString(languageTag);

        var actual = SentenceId.FromSentence(meaning, language, sentence.AsSpan());

        Assert.Equal(expectedId, actual.ToUInt64());
    }
}
