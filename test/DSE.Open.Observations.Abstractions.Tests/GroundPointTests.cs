// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values.Units;

namespace DSE.Open.Observations;

public sealed class GroundPointTests
{
    [Fact]
    public void New_ShouldConstruct()
    {
        // Arrange
        const double longitude = 1;
        const double latitude = 2;
        var accuracy = Length.FromMetres(5);

        // Act
        var point = new GroundPoint(latitude, longitude, accuracy);

        // Assert
        Assert.Equal(latitude, point.Latitude);
        Assert.Equal(longitude, point.Longitude);
        Assert.Equal(accuracy, point.Accuracy);
    }
}
