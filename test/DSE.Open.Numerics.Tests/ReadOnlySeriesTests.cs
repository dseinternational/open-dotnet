// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

public sealed class ReadOnlySeriesTests
{
    [Fact]
    public void ToSeries_ShouldReturnEquivalentMutableCopy()
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
    public void JsonRoundtrip_Reflected()
    {
        JsonRoundtrip(NumericsJsonSharedOptions.Reflected);
    }

    [Fact]
    public void JsonRoundtrip_SourceGen()
    {
        JsonRoundtrip(NumericsJsonSharedOptions.SourceGenerated);
    }

    private static void JsonRoundtrip(JsonSerializerOptions options)
    {
        // Arrange
        var series = ReadOnlySeries.Create([1, 2, 3], "Test Series",
            new ReadOnlyCategorySet<int>([1, 2, 3]),
            new ReadOnlyValueLabelCollection<int>([
                new ValueLabel<int>(1, "One"),
                new ValueLabel<int>(2, "Two"),
                new ValueLabel<int>(3, "Three")
            ]));

        // Act
        var json = JsonSerializer.Serialize(series, options);
        var deserialized = JsonSerializer.Deserialize<ReadOnlySeries>(json, options);

        // Assert
        Assert.NotNull(deserialized);
        var actual = Assert.IsType<ReadOnlySeries<int>>(deserialized);
        Assert.Equal(series.Name, actual.Name);
        Assert.Equal(series.Length, actual.Length);
        Assert.Equal(series.Vector, actual.Vector);
        Assert.Equal(series.Categories, actual.Categories);
        Assert.Equal(series.ValueLabels, actual.ValueLabels);
    }
}
