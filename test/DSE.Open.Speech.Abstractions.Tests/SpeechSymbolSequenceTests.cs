// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Speech.Abstractions.Tests;

public class SpeechSymbolSequenceTests
{
    [Theory]
    [MemberData(nameof(WordTranscriptions))]
    public void ParseInvariant(string transcription)
    {
        var sequence = SpeechSymbolSequence.ParseInvariant(transcription);
        Assert.False(sequence.IsEmpty);
    }

    public static TheoryData<string> WordTranscriptions =>
        new(TranscriptionData.Transcriptions.ToArray());
}
