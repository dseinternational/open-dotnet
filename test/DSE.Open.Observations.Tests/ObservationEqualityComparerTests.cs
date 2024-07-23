// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public sealed class ObservationEqualityComparerTests
{
    [Fact]
    public void DefaultNotEqualIfValuesDiffer()
    {
        var obs1 = BinaryObservation.Create(TestMeasures.BinaryMeasure, true);
        var obs2 = BinaryObservation.Create(TestMeasures.BinaryMeasure2, false);
        Assert.False(ObservationEqualityComparer<BinaryObservation, bool>.Default.Equals(obs1, obs2));
    }

    [Fact]
    public void MeasurementEqualIfMeasurementEqual()
    {
        var obs1 = BinaryObservation.Create(TestMeasures.BinaryMeasure, true);
        var obs2 = BinaryObservation.Create(TestMeasures.BinaryMeasure, false);
        Assert.True(ObservationEqualityComparer<BinaryObservation, bool>.Measurement.Equals(obs1, obs2));
    }
}
