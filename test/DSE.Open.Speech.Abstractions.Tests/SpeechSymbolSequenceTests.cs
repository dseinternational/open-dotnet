// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Speech.Abstractions.Tests;

public class SpeechSymbolSequenceTests
{
    [Fact]
    public void CanCastMemoryToChar()
    {
        var transcription = SpeechSymbolSequence.Parse("dɹˈɪŋkɪŋ", CultureInfo.InvariantCulture);
        var span = transcription.AsSpan();
        var chars = MemoryMarshal.Cast<SpeechSymbol, char>(span);
        Assert.Equal("dɹˈɪŋkɪŋ", new string(chars));
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

    public static TheoryData<string> WordTranscriptions =>
        new(TranscriptionData.Transcriptions.ToArray());

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
