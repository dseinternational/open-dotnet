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

    [Theory]
    [InlineData(-90, -180)]
    [InlineData(90, 180)]
    [InlineData(0, 0)]
    public void New_AcceptsBoundaryValues(double latitude, double longitude)
    {
        var point = new GroundPoint(latitude, longitude, Length.FromMetres(1));
        Assert.Equal(latitude, point.Latitude);
        Assert.Equal(longitude, point.Longitude);
    }

    [Theory]
    [InlineData(-91)]
    [InlineData(91)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void New_ThrowsForInvalidLatitude(double latitude)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new GroundPoint(latitude, 0, Length.FromMetres(1)));
    }

    [Theory]
    [InlineData(-181)]
    [InlineData(181)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void New_ThrowsForInvalidLongitude(double longitude)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new GroundPoint(0, longitude, Length.FromMetres(1)));
    }
}
