// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Observations;

public sealed class BinaryWordObservationTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var obs = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, true);
        AssertJson.Roundtrip(obs);
    }

    [Fact]
    public void JsonRoundtrip_WithContext()
    {
        var obs = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, true);
        var typeInfo = ObservationsJsonSerializerContext.Default.BinaryWordObservation;
        AssertJson.Roundtrip(obs, typeInfo);
    }

    [Fact]
    public void MeasurementIdEqualForSameMeasureAndSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, false);
        Assert.Equal(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)333041260044uL, false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementIdNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure2, (WordId)420048260031uL, false);
        Assert.NotEqual(obs1.GetMeasurementId(), obs2.GetMeasurementId());
    }

    [Fact]
    public void MeasurementHashCodeEqualForSameMeasureAndSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, false);
        Assert.Equal(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForSameMeasureAndDifferentSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)335681260044uL, false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }

    [Fact]
    public void MeasurementHashCodeNotEqualForDifferentMeasureAndSameSound()
    {
        var obs1 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure, (WordId)420048260031uL, true);
        var obs2 = BinaryWordObservation.Create(TestMeasures.BinaryWordMeasure2, (WordId)420048260031uL, false);
        Assert.NotEqual(obs1.GetMeasurementHashCode(), obs2.GetMeasurementHashCode());
    }
}
