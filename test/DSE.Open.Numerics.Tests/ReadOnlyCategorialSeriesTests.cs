// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public partial class ReadOnlyCategorialSeriesTests
{
    [Fact]
    public void CreateReadOnlyCategorial_WithCategories()
    {
        var series = ReadOnlyCategoricalSeries.Create(
            [1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6],
            [1, 2, 3, 4, 5, 6]);

        Assert.Equal(18, series.Length);
        Assert.True(series.AsReadOnlySpan().SequenceEqual([1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6]));
        Assert.Equal(6, series.Categories.Count);
    }

    [Fact]
    public void CreateReadOnlyCategorial_WithCategoriesAndLabels()
    {
        var series = ReadOnlyCategoricalSeries.Create(
            [1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6],
            [1, 2, 3, 4, 5, 6],
            [(1, "one"), (2, "two")]);

        Assert.Equal(18, series.Length);
        Assert.True(series.AsReadOnlySpan().SequenceEqual([1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6]));
        Assert.Equal(6, series.Categories.Count);
        Assert.Equal(2, series.ValueLabels.Count);

        var labels = series.GetLabelledData().ToArray();

        Assert.Equal("one", labels[0]);
        Assert.Equal("two", labels[1]);
        Assert.Equal("3", labels[2]);
    }
}
