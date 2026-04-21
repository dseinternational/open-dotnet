// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;

namespace DSE.Open.Language;

public sealed class WordMeaningIdTests
{
    [Fact]
    public void SerializesToNumber()
    {
        var value = (WordMeaningId)258089004501;
        var json = JsonSerializer.Serialize(value);
        Assert.Equal(value.ToStringInvariant(), json);
    }

    [Fact]
    public void GetRandomId_ReturnsValidValue()
    {
        for (var i = 0; i < 1000; i++)
        {
            var id = WordMeaningId.GetRandomId();
            Assert.True(WordMeaningId.IsValidValue((long)id));
        }
    }

    [Fact]
    public void ToInt64_round_trips_via_FromInt64()
    {
        var id = WordMeaningId.FromUInt64(258089004501ul);
        var i = id.ToInt64();
        Assert.Equal(id, WordMeaningId.FromInt64(i));
    }

    [Fact]
    public void ToUInt64_round_trips_via_FromUInt64()
    {
        var id = WordMeaningId.FromUInt64(258089004501ul);
        var u = id.ToUInt64();
        Assert.Equal(id, WordMeaningId.FromUInt64(u));
    }

    [Fact]
    public void FromUInt64_throws_for_out_of_range_value()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => WordMeaningId.FromUInt64(1ul));
    }

    // Pins a specific hash output so any future change to the serialization
    // format is caught on every platform. Regression guard for #354 item 4.
    [Theory]
    [InlineData("run", "VERB", "VB", 416664435337ul)]
    [InlineData("run", "NOUN", "NN", 196937913916ul)]
    [InlineData("run", "VERB", null, 747335625621ul)]
    public void FromWordMeaning_produces_platform_pinned_id(
        string label,
        string pos,
        string? altPos,
        ulong expectedId)
    {
        var actual = WordMeaningId.FromWordMeaning(
            label.AsSpan(),
            (AsciiString)pos,
            altPos is null ? null : (AsciiString)altPos);

        Assert.Equal(expectedId, actual.ToUInt64());
    }

    // With 0xFF separator bytes, distinct input tuples whose naive concatenations
    // would align now produce different ids. Regression guard for #354 item 4.
    [Fact]
    public void FromWordMeaning_tuples_with_aligned_concat_produce_distinct_ids()
    {
        // "ab" + "cd" vs "abc" + "d" — same concatenated bytes without a separator.
        var a = WordMeaningId.FromWordMeaning("ab".AsSpan(), (AsciiString)"cd", null);
        var b = WordMeaningId.FromWordMeaning("abc".AsSpan(), (AsciiString)"d", null);
        Assert.NotEqual(a, b);
    }
}
