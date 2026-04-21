// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using DSE.Open.Globalization;

namespace DSE.Open.Language;

public sealed class WordIdTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var value = (WordId)258089004501;
        var json = JsonSerializer.Serialize(value);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Fact]
    public void GetRandomId_ReturnsValidValue()
    {
        for (var i = 0; i < 1000; i++)
        {
            var id = WordId.GetRandomId();
            Assert.True(WordId.IsValidValue((long)id));
        }
    }

    [Fact]
    public void FromWord_overloads_agree()
    {
        var meaning = (WordMeaningId)123456789012ul;
        var word = new WordText("run");
        var language = LanguageTag.EnglishUk;

        var fromWordText = WordId.FromWord(meaning, word, language);
        var fromChars = WordId.FromWord(meaning, word.Span, language);
        var fromBytes = WordId.FromWord(meaning, Encoding.UTF8.GetBytes("run"), language);

        Assert.Equal(fromWordText, fromChars);
        Assert.Equal(fromWordText, fromBytes);
    }

    // Pins a specific hash output so any future change to the serialization
    // format (including endianness, ordering, or formatting of the meaning id,
    // or separator bytes) is caught on every platform. Regression guard for #348
    // and #354 item 4.
    [Theory]
    [InlineData(100000000001ul, "run", "en-GB", 680816084390ul)]
    [InlineData(500000000042ul, "correr", "es-ES", 704252438813ul)]
    public void FromWord_produces_platform_pinned_id(
        ulong meaningIdValue,
        string word,
        string languageTag,
        ulong expectedId)
    {
        var meaning = (WordMeaningId)meaningIdValue;
        var language = LanguageTag.FromString(languageTag);

        var actual = WordId.FromWord(meaning, word.AsSpan(), language);

        Assert.Equal(expectedId, actual.ToUInt64());
    }

    [Fact]
    public void ToInt64_round_trips_via_FromInt64()
    {
        var id = WordId.FromUInt64(258089004501ul);
        var i = id.ToInt64();
        Assert.Equal(id, WordId.FromInt64(i));
    }

    [Fact]
    public void ToUInt64_round_trips_via_FromUInt64()
    {
        var id = WordId.FromUInt64(258089004501ul);
        var u = id.ToUInt64();
        Assert.Equal(id, WordId.FromUInt64(u));
    }

    [Fact]
    public void FromUInt64_throws_for_out_of_range_value()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => WordId.FromUInt64(1ul));
    }


    [Theory]
    [InlineData(100000000001ul, "run", "en-GB")]
    [InlineData(500000000042ul, "correr", "es-ES")]
    [InlineData(999999999999ul, "{{ child_subject_given_name }}", "en-US")]
    public void FromWord_overloads_agree_for_platform_independent_inputs(
        ulong meaningIdValue,
        string word,
        string languageTag)
    {
        var meaning = (WordMeaningId)meaningIdValue;
        var language = LanguageTag.FromString(languageTag);

        var fromChars = WordId.FromWord(meaning, word.AsSpan(), language);
        var fromBytes = WordId.FromWord(meaning, Encoding.UTF8.GetBytes(word), language);
        var fromWordText = word.Length <= WordText.MaxSerializedCharLength
            ? WordId.FromWord(meaning, new WordText(word), language)
            : fromChars;

        Assert.Equal(fromChars, fromBytes);
        Assert.Equal(fromChars, fromWordText);
        Assert.True(WordId.IsValidValue((long)fromChars));
    }

    // Regression guard for #349: the char overload previously used an
    // unbounded `stackalloc byte[c]`, which would blow the stack for
    // large inputs. With the ArrayPool guard in place, a 64 KB word
    // must complete without a StackOverflowException.
    [Fact]
    public void FromWord_with_long_char_span_does_not_stack_overflow()
    {
        var meaning = (WordMeaningId)100000000042ul;
        var language = LanguageTag.EnglishUk;
        var word = new string('a', 64 * 1024);

        var id = WordId.FromWord(meaning, word.AsSpan(), language);

        Assert.True(WordId.IsValidValue((long)id));
    }

    // Regression guard for #349: same check via the byte overload.
    [Fact]
    public void FromWord_with_long_byte_span_does_not_stack_overflow()
    {
        var meaning = (WordMeaningId)100000000042ul;
        var language = LanguageTag.EnglishUk;
        var wordUtf8 = new byte[64 * 1024];
        Array.Fill(wordUtf8, (byte)'a');

        var id = WordId.FromWord(meaning, wordUtf8, language);

        Assert.True(WordId.IsValidValue((long)id));
    }
}
