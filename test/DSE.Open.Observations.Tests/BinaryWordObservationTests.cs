// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public sealed class BinaryWordObservationTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var obs = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 96815631, true);
        var json = JsonSerializer.Serialize(obs, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<BinaryWordObservation>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.NotNull(deserialized);
        Assert.Equal(obs.Id, deserialized.Id);
        Assert.Equal(obs.MeasureId, deserialized.MeasureId);
        Assert.Equal(obs.WordId, deserialized.WordId);
        Assert.Equal(obs.Time.ToUnixTimeMilliseconds(), deserialized.Time.ToUnixTimeMilliseconds());
    }

    [Fact]
    public void MeasurementIdEqualForSameMeasureAndSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 9813861, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 9813861, false);
        Assert.Equal(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 9813861, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 68151, false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 9813861, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure2, 9813861, false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementHashCodeEqualForSameMeasureAndSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 9813861, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 9813861, false);
        Assert.Equal(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 9813861, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 981565, false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, 9813861, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure2, 9813861, false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }
}
