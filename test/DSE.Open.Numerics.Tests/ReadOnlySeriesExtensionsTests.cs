// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public sealed class ReadOnlySeriesExtensionsTests
{
    [Fact]
    public void Copied_ShouldReturnEquivalentMutableCopy()
    {
        // Arrange
        const string seriesName = "Test Series";

        var original = ReadOnlySeries.Create([1, 2, 3], seriesName,
            new ReadOnlyCategorySet<int>([1, 2, 3]),
            new ReadOnlyValueLabelCollection<int>([
                new ValueLabel<int>(1, "One"),
                new ValueLabel<int>(2, "Two"),
                new ValueLabel<int>(3, "Three")
            ]));

        // Act
        var copied = original.ToSeries();

        // Assert
        Assert.NotSame(original, copied);
        Assert.Equal(original.Name, copied.Name);
        Assert.Equal(original.Length, copied.Length);
        Assert.Equal(original.Vector, copied.Vector);
        Assert.Equal(original.Categories, copied.Categories);
        Assert.Equal(original.ValueLabels, copied.ValueLabels);
    }

    [Fact]
    public void Expect_WithMismatchedType_ShouldThrowInvalidDataException()
    {
        // Arrange
        var series = ReadOnlySeries.Create([1, 2, 3], "Test Series");

        // Act
        void Act() => series.ExpectElementOfType<string>();

        // Assert
        _ = Assert.Throws<InvalidDataException>(Act);
    }
}
