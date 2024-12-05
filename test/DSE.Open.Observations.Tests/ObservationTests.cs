// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Speech;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class ObservationTests
{
    [Fact]
    public void SerializeDeserializeBinary()
    {
        var observation = Observation.Create(TestMeasures.BinaryMeasure, true);
        var json = JsonSerializer.Serialize(observation);
        var deserialized = JsonSerializer.Deserialize<Observation<Binary>>(json);
        Assert.Equal(observation, deserialized);
    }

    [Fact]
    public void JsonRoundtrip()
    {
        Observation[] observations =
        [
            Observation.Create(TestMeasures.BinaryMeasure, true),
            Observation.Create(TestMeasures.CountMeasure, (Count)42),
            Observation.Create(TestMeasures.AmountMeasure, (Amount)42.0m),
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true),
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, false),
        ];

        foreach (var observation in observations)
        {
            AssertJson.Roundtrip(observation, JsonContext.Default.Observation);
        }
    }
}
