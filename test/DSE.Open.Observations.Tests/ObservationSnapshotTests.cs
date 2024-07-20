// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class ObservationSnapshotTests
{
    [Fact]
    public void New_WithInvalidDate_ShouldThrow()
    {
        // Arrange
        var date = DateTime.Parse("01/01/2024", null).AddDays(-1);

        // Act
        void Act() => _ = new ObservationSnapshot<Observation<Count>>(date, Observation.Create(1, Count.Zero));

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(Act);
    }

    [Fact]
    public void GetDiscriminatorCode_WithDifferentMeasures_ShouldReturnDifferentCodes()
    {
        // Arrange
        var one = Observation.Create(1, Count.Zero);
        var two = Observation.Create(2, Count.Zero);
        var oneAgain = Observation.Create(1, Count.FromValue(1));

        // Assert
        Assert.NotEqual(one.GetDiscriminatorCode(), two.GetDiscriminatorCode());
        Assert.Equal(one.GetDiscriminatorCode(), oneAgain.GetDiscriminatorCode());
    }
}
