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

    // Pins the legacy hash output so the id stays stable across platforms AND across
    // future refactors. Downstream systems persist these ids, so any change that shifts
    // the output (byte order, field ordering, separators, formatting) must be caught here.
    [Theory]
    [InlineData("run", "VERB", "VB", 775513550752ul)]
    [InlineData("run", "NOUN", "NN", 529049257577ul)]
    [InlineData("run", "VERB", null, 334926270592ul)]
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

    // Legacy hash layout concatenates fields with no separator, so tuples whose
    // concatenations align produce identical ids — a known limitation. Preserved
    // intentionally: adding separators (as #446 did) would break ids persisted by
    // prior builds. This test locks the legacy behaviour so any reintroduction of
    // separators fails here.
    [Fact]
    public void FromWordMeaning_tuples_with_aligned_concat_produce_identical_ids()
    {
        // "ab" + "cd" vs "abc" + "d" — same concatenated bytes without a separator.
        var a = WordMeaningId.FromWordMeaning("ab".AsSpan(), (AsciiString)"cd", null);
        var b = WordMeaningId.FromWordMeaning("abc".AsSpan(), (AsciiString)"d", null);
        Assert.Equal(a, b);
    }
}
