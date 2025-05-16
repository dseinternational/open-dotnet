// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics;

public partial class CategorialSeriesTests : LoggedTestsBase
{
    private static readonly Lazy<JsonSerializerOptions> s_jsonOptions = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping);
        return options;
    });

    public CategorialSeriesTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CreateCategorial_WithCategories()
    {
        var series = CategoricalSeries.Create(
            [1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6],
            [1, 2, 3, 4, 5, 6]);

        Assert.Equal(18, series.Length);
        Assert.True(series.AsSpan().SequenceEqual([1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6]));
        Assert.Equal(6, series.Categories.Count);
    }

    [Fact]
    public void CreateCategorial_WithCategoriesAndLabels()
    {
        var series = CategoricalSeries.Create(
            [1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6],
            [1, 2, 3, 4, 5, 6],
            [(1, "one"), (2, "two")]);

        Assert.Equal(18, series.Length);
        Assert.True(series.AsSpan().SequenceEqual([1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6]));
        Assert.Equal(6, series.Categories.Count);
        Assert.Equal(2, series.ValueLabels.Count);

        var labels = series.GetLabelledData().ToArray();

        Assert.Equal("one", labels[0]);
        Assert.Equal("two", labels[1]);
        Assert.Equal("3", labels[2]);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var series = CategoricalSeries.Create(
            [1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6],
            [1, 2, 3, 4, 5, 6],
            [(1, "one"), (2, "two")]);

        var json = JsonSerializer.Serialize(series, s_jsonOptions.Value);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Series<int>>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);

        Assert.Equal(18, series.Length);
        Assert.True(series.AsSpan().SequenceEqual([1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6]));
        Assert.Equal(6, series.Categories.Count);
        Assert.Equal(2, series.ValueLabels.Count);

        var labels = series.GetLabelledData().ToArray();

        Assert.Equal("one", labels[0]);
        Assert.Equal("two", labels[1]);
        Assert.Equal("3", labels[2]);
    }
    /*

    [Fact]
    public void AsReadOnly_ShouldReturnReadOnlySeries()
    {
        // Arrange
        var series = Series.Create([1, 1, 2, 3, 2],
        [
            KeyValuePair.Create("one", 1),
            KeyValuePair.Create("two", 2),
            KeyValuePair.Create("three", 3),
        ]);

        // Act
        var readOnlyVector = series.AsReadOnly();

        // Assert
        _ = Assert.IsType<ReadOnlySeries<int>>(readOnlyVector);
    }
    */
}
