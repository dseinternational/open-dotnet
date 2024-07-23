// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Speech;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public sealed class BinarySpeechSoundObservationTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var obs = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.VoicedPostalveolarAffricate, true);
        var json = JsonSerializer.Serialize(obs, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<BinarySpeechSoundObservation>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.NotNull(deserialized);
        Assert.Equal(obs.Id, deserialized.Id);
        Assert.Equal(obs.MeasureId, deserialized.MeasureId);
        Assert.Equal(obs.SpeechSound, deserialized.SpeechSound);
        Assert.Equal(obs.Time.ToUnixTimeMilliseconds(), deserialized.Time.ToUnixTimeMilliseconds());
    }
}
