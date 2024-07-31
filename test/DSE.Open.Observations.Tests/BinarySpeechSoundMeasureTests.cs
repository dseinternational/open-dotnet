// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Speech;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public sealed class BinarySpeechSoundMeasureTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new BinarySpeechSoundMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        var json = JsonSerializer.Serialize(measure, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<BinarySpeechSoundMeasure>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.NotNull(deserialized);
        Assert.Equal(measure.Id, deserialized.Id);
        Assert.Equal(measure.Uri, deserialized.Uri);
        Assert.Equal(measure.MeasurementLevel, deserialized.MeasurementLevel);
        Assert.Equal(measure.Name, deserialized.Name);
        Assert.Equal(measure.Statement, deserialized.Statement);
    }

    [Fact]
    public void CanCreateObservation()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new BinarySpeechSoundMeasure(MeasureId.GetRandomId(), uri, "Test measure", "[subject] does something");
        var obs = measure.CreateObservation(SpeechSound.CloseBackRoundedVowel, true, DateTimeOffset.UtcNow);
        Assert.Equal(measure.Id, obs.MeasureId);
        Assert.True(obs.Value);
    }
}
