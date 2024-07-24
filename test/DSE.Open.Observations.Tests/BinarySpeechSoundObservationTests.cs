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

    [Fact]
    public void MeasurementIdEqualForSameMeasureAndSound()
    {
        var obs1 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.VoicedPostalveolarAffricate, true);
        var obs2 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.VoicedPostalveolarAffricate, false);
        Assert.Equal(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.VoicedPostalveolarAffricate, true);
        var obs2 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.CloseBackRoundedVowel, false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.CloseBackRoundedVowel, true);
        var obs2 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure2, SpeechSound.CloseBackRoundedVowel, false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementHashCodeEqualForSameMeasureAndSound()
    {
        var obs1 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.VoicedPostalveolarAffricate, true);
        var obs2 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.VoicedPostalveolarAffricate, false);
        Assert.Equal(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.VoicedPostalveolarAffricate, true);
        var obs2 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.CloseBackRoundedVowel, false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.CloseBackRoundedVowel, true);
        var obs2 = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure2, SpeechSound.CloseBackRoundedVowel, false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }
}
