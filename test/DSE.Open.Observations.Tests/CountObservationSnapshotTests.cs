// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class CountObservationSnapshotTests
{
    [Fact]
    public void New_WithInvalidDate_ShouldThrow()
    {
        // Arrange
        var date = DateTime.Parse("01/01/2024", null).AddDays(-1);

        // Act
        void Act() => _ = new CountObservationSnapshot(date, CountObservation.Create(TestMeasures.CountMeasure, Count.Zero));

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void GetMeasurementCode_WithDifferentMeasures_ShouldReturnDifferentCodes()
    {
        // Arrange
        var one = CountObservation.Create(TestMeasures.CountMeasure, Count.Zero);
        var two = CountObservation.Create(TestMeasures.CountMeasure2, Count.Zero);
        var oneAgain = CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(1));

        // Assert
        Assert.NotEqual(one.GetMeasurementHashCode(), two.GetMeasurementHashCode());
        Assert.Equal(one.GetMeasurementHashCode(), oneAgain.GetMeasurementHashCode());
    }
}
