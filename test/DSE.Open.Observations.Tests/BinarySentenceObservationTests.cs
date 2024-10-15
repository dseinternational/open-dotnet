// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class BinarySentenceObservationTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var obs = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), true);
        AssertJson.Roundtrip(obs);
    }

    [Fact]
    public void JsonRoundtrip_WithContext()
    {
        var obs = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), true);
        AssertJson.Roundtrip(obs, ObservationsJsonSerializerContext.RelaxedJsonEscaping);
    }

    [Fact]
    public void MeasurementIdEqualForSameMeasureAndSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), false);
        Assert.Equal(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(333041260044uL), false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure2, SentenceId.FromUInt64(420048260031uL), false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementHashCodeEqualForSameMeasureAndSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), false);
        Assert.Equal(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(335681260044uL), false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure, SentenceId.FromUInt64(420048260031uL), true);
        var obs2 = BinarySentenceObservation.Create(TestMeasures.BinarySentenceMeasure2, SentenceId.FromUInt64(420048260031uL), false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }
}
