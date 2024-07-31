// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Language;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public sealed class BinarySentenceObservationTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var obs = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)420048260031uL, true);
        var json = JsonSerializer.Serialize(obs, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<BinarySentenceObservation>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.NotNull(deserialized);
        Assert.Equal(obs.Id, deserialized.Id);
        Assert.Equal(obs.MeasureId, deserialized.MeasureId);
        Assert.Equal(obs.SentenceId, deserialized.SentenceId);
        Assert.Equal(obs.Time.ToUnixTimeMilliseconds(), deserialized.Time.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void MeasurementIdEqualForSameMeasureAndSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)420048260031uL, true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)420048260031uL, false);
        Assert.Equal(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)420048260031uL, true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)333041260044uL, false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)420048260031uL, true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure2, (SentenceId)420048260031uL, false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementHashCodeEqualForSameMeasureAndSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)420048260031uL, true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)420048260031uL, false);
        Assert.Equal(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)420048260031uL, true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)335681260044uL, false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, (SentenceId)420048260031uL, true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure2, (SentenceId)420048260031uL, false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }
}
