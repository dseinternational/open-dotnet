// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public sealed class ReadOnlySeriesExtensionsTests
{
    [Fact]
    public void CastToSeriesWithElementType_WithMismatchedType_ShouldThrowInvalidDataException()
    {
        // Arrange
        var series = ReadOnlySeries.Create([1, 2, 3], "Test Series");

        // Act
        void Act() => series.CastToSeriesWithElementType<string>();

        // Assert
        _ = Assert.Throws<InvalidDataException>(Act);
    }
}
