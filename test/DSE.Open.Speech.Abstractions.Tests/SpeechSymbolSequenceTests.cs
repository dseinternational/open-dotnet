// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Speech.Abstractions.Tests;

public class SpeechSymbolSequenceTests
{
    [Theory]
    [InlineData("ˈeəɹəplˌeɪn", false, "eə")]
    [InlineData("lˈɒlɪpˌɒp", true, "l")]
    [InlineData("dɹˈɪŋkɪŋ", true, "d")]
    [InlineData("mˈɒp", true, "m")]
    [InlineData("təmˈɑːtəʊ", true, "t")]
    [InlineData("ɹˈæbɪt", true, "ɹ")]
    [InlineData("ˌæftɜ˞ˈnun", true, "ˌæ")]
    [InlineData("ˈɑːnsɐ", true, "ˈɑː")]
    [InlineData("ˌæftɜ˞ˈnun", false, "æ")]
    [InlineData("ˈɑːnsɐ", false, "ɑː")]
    [InlineData("flˈæɡ", false, "f")]
    [InlineData("ˈdɹɪŋkɪŋ", false, "d")]
    [InlineData("ˈdɹɪŋkɪŋ", true, "ˈd")]
    [InlineData("ɡlˈɑːs", true, "ɡ")]
    [InlineData("θɹˈəʊɪŋ", true, "θ")]
    [InlineData("tʃˈiːz", true, "tʃ")]
    [InlineData("ʃˈɒpɪŋ", true, "ʃ")]
    [InlineData("ɜ˞ˈaʊnd", true, "ɜ˞")]
    [InlineData("ˈɬædɜ˞", false, "ɬ")]
    [InlineData("ˈɬædɜ˞", true, "ˈɬ")]
    public void GetInitialSound(string sequence, bool includeInitialStressMarkers, string expected)
    {
        var transcription = SpeechSymbolSequence.ParseInvariant(sequence);
        var expectedSound = SpeechSound.ParseInvariant(expected);
        var initialSound = transcription.GetInitialSound(includeInitialStressMarkers);
        Assert.Equal(expectedSound, initialSound);
    }

    [Fact]
    public void CanCastMemoryToChar()
    {
        var transcription = SpeechSymbolSequence.Parse("dɹˈɪŋkɪŋ", CultureInfo.InvariantCulture);
        var span = transcription.AsSpan();
        var chars = MemoryMarshal.Cast<SpeechSymbol, char>(span);
        Assert.Equal("dɹˈɪŋkɪŋ", new(chars));
    }

    [Theory]
    [InlineData("lˈɒlɪpˌɒp", "lˈɒlɪpˌɒp")]
    [InlineData("lɒlɪpˌɒp", "lˈɒlɪpˌɒp")]
    [InlineData("lˈɒlɪpˌɒp", "lɒlɪpˌɒp")]
    [InlineData("lˈɒlɪpˌɒp", "lɒlɪpɒp")]
    [InlineData("lɒlɪpɒp", "lˈɒlɪpˌɒp")]
    public void EqualsConsonantsAndVowels(string t1, string t2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = t2.AsSpan();
        Assert.True(transcription1.Equals(transcription2, SpeechSymbolSequenceComparison.ConsonantsAndVowels));
    }

    [Theory]
    [InlineData("lˈɒlɪpˌp", "lˈɒlɪpˌɒp")]
    [InlineData("lɒlɪpˌɒp", "lˈɒɪpˌɒp")]
    [InlineData("lˈɒlɪpˌɒp", "lɒlpˌɒp")]
    [InlineData("lˈɒlɪˌɒp", "lɒlɪpɒp")]
    [InlineData("lɒlɪpɒp", "lˈɒpˌɒp")]
    public void NotEqualsConsonantsAndVowels(string t1, string t2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = t2.AsSpan();
        Assert.False(transcription1.Equals(transcription2, SpeechSymbolSequenceComparison.ConsonantsAndVowels));
    }

    [Theory]
    [InlineData("hˈɛlɪkˌɒptɐ", "h", 0)]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɒptɐ", 7)]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒptɐ", 0)]
    [InlineData("hˈɛlɪkˌɒptɐ", "ˌ", 6)]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɐ", 10)]
    public void IndexOf(string t1, string t2, int expected)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = SpeechSymbolSequence.Parse(t2, CultureInfo.InvariantCulture);
        Assert.Equal(expected, transcription1.IndexOf(transcription2));
    }

    [Theory]
    [InlineData("hˈɛlɪkˌɒptɐ", "h", 0)]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɒptɐ", 7)]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒptɐ", 0)]
    [InlineData("hˈɛlɪkˌɒptɐ", "ˌ", 6)]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɐ", 10)]
    public void IndexOfCharSpan(string t1, string t2, int expected)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = t2.AsSpan();
        Assert.Equal(expected, transcription1.IndexOf(transcription2));
    }

    [Theory]
    [InlineData("hˈɛlɪkˌɒptɐ", "h")]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɒptɐ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒptɐ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "ˌ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɐ")]
    public void Contains(string t1, string t2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = SpeechSymbolSequence.Parse(t2, CultureInfo.InvariantCulture);
        Assert.True(transcription1.Contains(transcription2));
    }

    [Theory]
    [InlineData("hˈɛlɪkˌɒptɐ", "h")]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɒptɐ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒptɐ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "ˌ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɐ")]
    public void ContainsExact(string t1, string t2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = t2.AsSpan();
        Assert.True(transcription1.Contains(transcription2, SpeechSymbolSequenceComparison.Exact));
    }

    [Theory]
    [InlineData("hˈɛlɪkˌɒptɐ", "h")]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɒptɐ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hɛlɪkɒptɐ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "ɐ")]
    public void ContainsConsonantsAndVowels(string t1, string t2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = SpeechSymbolSequence.Parse(t2, CultureInfo.InvariantCulture);
        Assert.True(transcription1.Contains(transcription2, SpeechSymbolSequenceComparison.ConsonantsAndVowels));
    }

    [Theory]
    [InlineData("hˈɛlɪkˌɒptɐ", "h")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪk")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒptɐ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈ")]
    public void StartsWith(string t1, string t2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = SpeechSymbolSequence.Parse(t2, CultureInfo.InvariantCulture);
        Assert.True(transcription1.StartsWith(transcription2));
    }

    [Theory]
    [InlineData("hˈɛlɪkˌɒptɐ", "h")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪk")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒptɐ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈ")]
    public void StartsWithExact(string t1, string t2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = t2.AsSpan();
        Assert.True(transcription1.StartsWith(transcription2, SpeechSymbolSequenceComparison.Exact));
    }

    [Theory]
    [InlineData("hˈɛlɪkˌɒptɐ", "h")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hɛlɪk")]
    [InlineData("hɛlɪkɒptɐ", "hˈɛlɪk")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈ")]
    public void StartsWithConsonantsAndVowels(string t1, string t2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = SpeechSymbolSequence.Parse(t2, CultureInfo.InvariantCulture);
        Assert.True(transcription1.StartsWith(transcription2, SpeechSymbolSequenceComparison.ConsonantsAndVowels));
    }

    [Theory]
    [InlineData("hˈɛlɪkˌɒptɐ", "h")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hɛlɪk")]
    [InlineData("hɛlɪkɒptɐ", "hˈɛlɪkˌɒptɐ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈɛlɪkˌɒ")]
    [InlineData("hˈɛlɪkˌɒptɐ", "hˈ")]
    public void StartsWithConsonantsAndVowelsCharSpan(string t1, string t2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(t1, CultureInfo.InvariantCulture);
        var transcription2 = t2.AsSpan();
        Assert.True(transcription1.StartsWith(transcription2, SpeechSymbolSequenceComparison.ConsonantsAndVowels));
    }

    [Theory]
    [MemberData(nameof(WordTranscriptions))]
    public void ParseInvariant(string transcription)
    {
        var sequence = SpeechSymbolSequence.ParseInvariant(transcription);
        Assert.False(sequence.IsEmpty);
    }

    [Theory]
    [MemberData(nameof(WordTranscriptionPairs))]
    public void Equals_returns_true_when_equal(string value1, string value2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(value1, CultureInfo.InvariantCulture);
        var transcription2 = SpeechSymbolSequence.Parse(value2, CultureInfo.InvariantCulture);
        Assert.True(transcription1.Equals(transcription2));
    }

    [Theory]
    [MemberData(nameof(WordTranscriptionPairs))]
    public void Equals_char_sequence_returns_true_when_equal(string value1, string value2)
    {
        var transcription1 = SpeechSymbolSequence.Parse(value1, CultureInfo.InvariantCulture);
        var transcription2 = value2.AsSpan();
        Assert.True(transcription1.Equals(transcription2));
    }

    [Theory]
    [MemberData(nameof(TwentyWordTranscriptions))]
    public void TryFormat_Parse_Roundtrip(string transcription)
    {
        // Arrange
        Span<char> buffer = stackalloc char[128];
        var sequence = SpeechSymbolSequence.Parse(transcription, CultureInfo.InvariantCulture);

        // Act & Assert
        var formatted = sequence.TryFormat(buffer, out var charsWritten);
        Assert.True(formatted);

        var parsed = SpeechSymbolSequence.Parse(buffer[..charsWritten], CultureInfo.InvariantCulture);
        Assert.Equal(sequence, parsed);
    }

    [Fact]
    public void GetRepeatableHashCode_ReturnsExpectedValue()
    {
        var value = SpeechSymbolSequence.Parse("hɛlɪkɒptɐ", CultureInfo.InvariantCulture);
        Assert.Equal(3774544766252701229u, value.GetRepeatableHashCode());
    }

    public static TheoryData<string> TwentyWordTranscriptions => new((string[])[.. TranscriptionData.Transcriptions.RandomSelection(20)]);

    public static TheoryData<string> WordTranscriptions => new((string[])[.. TranscriptionData.Transcriptions]);

    public static TheoryData<string, string> WordTranscriptionPairs
    {
        get
        {
            var data = new TheoryData<string, string>();
            TranscriptionData.Transcriptions.Skip(200).Take(200).ForEach(t => data.Add(t, t));
            return data;
        }
    }
}
