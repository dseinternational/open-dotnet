// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public partial class SeriesTests : LoggedTestsBase
{
    public SeriesTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create_FromVector()
    {
        var vector = Vector.Create([1, 2, 3, 4, 5]);
        var series = Series.Create(vector, "TestSeries");

        Assert.NotNull(series);
        Assert.Equal("TestSeries", series.Name);
        Assert.Equal(5, series.Length);
        Assert.True(series.AsSpan().SequenceEqual([1, 2, 3, 4, 5]));
    }

    [Fact]
    public void Create_FromArray()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var series = Series.Create(array, "TestSeries");

        Assert.NotNull(series);
        Assert.Equal("TestSeries", series.Name);
        Assert.Equal(5, series.Length);
        Assert.True(series.AsSpan().SequenceEqual(array));
    }

    [Fact]
    public void Create_FromReadOnlySpan()
    {
        var span = new ReadOnlySpan<int>([1, 2, 3, 4, 5]);
        var series = Series.Create(span, "TestSeries");

        Assert.NotNull(series);
        Assert.Equal("TestSeries", series.Name);
        Assert.Equal(5, series.Length);
        Assert.True(series.AsSpan().SequenceEqual(span.ToArray()));
    }

    [Fact]
    public void Create_FromLength()
    {
        var series = Series.Create<int>(5, "TestSeries");

        Assert.NotNull(series);
        Assert.Equal("TestSeries", series.Name);
        Assert.Equal(5, series.Length);
        Assert.True(series.AsSpan().SequenceEqual(new int[5]));
    }

    [Fact]
    public void Create_FromLengthAndScalar()
    {
        var series = Series.Create(5, 42, "TestSeries");

        Assert.NotNull(series);
        Assert.Equal("TestSeries", series.Name);
        Assert.Equal(5, series.Length);
        Assert.True(series.AsSpan().SequenceEqual([42, 42, 42, 42, 42]));
    }

    [Fact]
    public void CreateZeroes()
    {
        var series = Series.CreateZeroes<int>(5, "ZeroSeries");

        Assert.NotNull(series);
        Assert.Equal("ZeroSeries", series.Name);
        Assert.Equal(5, series.Length);
        Assert.True(series.AsSpan().SequenceEqual(new int[5]));
    }

    [Fact]
    public void CreateOnes()
    {
        var series = Series.CreateOnes<int>(5, "OneSeries");

        Assert.NotNull(series);
        Assert.Equal("OneSeries", series.Name);
        Assert.Equal(5, series.Length);
        Assert.True(series.AsSpan().SequenceEqual([1, 1, 1, 1, 1]));
    }

    [Fact]
    public void CreateUninitialized()
    {
        var series = Series.CreateUninitialized<int>(5, "UninitializedSeries");

        Assert.NotNull(series);
        Assert.Equal("UninitializedSeries", series.Name);
        Assert.Equal(5, series.Length);
        // Cannot assert contents as they are uninitialized
    }

    [Fact]
    public void Create_Categorical_FromVector()
    {
        var vector = Vector.Create([1, 2, 3, 4, 5]);
        var categories = new CategorySet<int>([1, 2, 3, 4, 5]);
        var series = Series.Create(vector, "TestSeries", categories);

        Assert.NotNull(series);
        Assert.Equal("TestSeries", series.Name);
        Assert.Equal(5, series.Length);
        Assert.True(series.AsSpan().SequenceEqual([1, 2, 3, 4, 5]));
        Assert.Equal(categories, series.Categories);
    }

    [Fact]
    public void Create_Categorical_FromVector_2()
    {
        var series = Series.Create([1, 2, 3, 4, 5], "TestSeries", categories: [1, 2, 3, 4, 5]);

        Assert.NotNull(series);
        Assert.Equal("TestSeries", series.Name);
        Assert.Equal(5, series.Length);
        Assert.True(series.AsSpan().SequenceEqual([1, 2, 3, 4, 5]));
        Assert.Equal(5, series.Categories.Count);
    }

    [Fact]
    public void Create_Categorical_FromVector_Error()
    {
        _ = Assert.Throws<NumericsArgumentException>(() => Series.Create([11, 12, 13, 14, 15], categories: [1, 2, 3, 4, 5]));
    }
}
