// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public sealed class ReadOnlyDataFrameExtensionsTests
{
    [Fact]
    public void GetRequiredNamedSeriesWithElementType_WithValidSeries_ShouldReturnTypedSeries()
    {
        // Arrange
        const string seriesName = "Test Series";
        var expected = ReadOnlySeries.Create([1, 2, 3], seriesName);
        var dataFrame = ReadOnlyDataFrame.Create([expected]);

        // Act
        var series = dataFrame.GetRequiredNamedSeriesWithElementType<int>(seriesName);

        // Assert
        Assert.Same(expected, series);
    }

    [Fact]
    public void GetRequiredNamedSeriesWithElementType_WithMissingSeries_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var dataFrame = ReadOnlyDataFrame.Create([]);

        // Act
        void Act() => dataFrame.GetRequiredNamedSeriesWithElementType<int>("Missing Series");

        // Assert
        _ = Assert.Throws<KeyNotFoundException>(Act);
    }

    [Fact]
    public void GetRequiredNamedSeriesWithElementType_WithMismatchedType_ShouldThrowInvalidDataException()
    {
        // Arrange
        const string seriesName = "Test Series";
        var dataFrame = ReadOnlyDataFrame.Create([ReadOnlySeries.Create([1, 2, 3], seriesName)]);

        // Act
        void Act() => dataFrame.GetRequiredNamedSeriesWithElementType<string>(seriesName);

        // Assert
        _ = Assert.Throws<InvalidDataException>(Act);
    }
}
