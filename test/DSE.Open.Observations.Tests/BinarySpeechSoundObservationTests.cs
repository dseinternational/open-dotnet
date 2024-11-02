// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class BinarySpeechSoundObservationTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var obs = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.VoicedPostalveolarAffricate, true);
        AssertJson.Roundtrip(obs);
    }

    [Fact]
    public void JsonRoundtrip_WithContext()
    {
        var obs = BinarySpeechSoundObservation.Create(TestMeasures.BinarySpeechSoundMeasure, SpeechSound.VoicedPostalveolarAffricate, true);
        AssertJson.Roundtrip(obs, JsonContext.Default);
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
