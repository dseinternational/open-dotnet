// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Speech.Abstractions.Tests.Serialization;

public class JsonStringTranscriptionConverterTests
{
    [Theory]
    [MemberData(nameof(WordTranscriptions))]
    public void SerializeDeserialize(string transcription)
    {
        var json = $"\"{transcription}\"";
        var deserialized = JsonSerializer.Deserialize<SpeechTranscription>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.NotEqual(default, deserialized);

        var serialized = JsonSerializer.Serialize(deserialized, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal(json, serialized);

        var original = SpeechTranscription.Parse(transcription, CultureInfo.InvariantCulture);
        Assert.Equal(original, deserialized);
    }

    public static TheoryData<string> WordTranscriptions =>
        new(TranscriptionData.Transcriptions.Skip(800).Take(500).Select(t => $"[{t}]").ToArray());
}
