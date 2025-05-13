// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics.Data;

public class SeriesTests : LoggedTestsBase
{
    public SeriesTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void SerializeDeserializeReflected()
    {
        var series = Series.Create("test", [1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Series<int>>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(series, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGenerated()
    {
        var series = Series.Create("test", [1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Series<int>>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(series, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedPolymorphic()
    {
        var series = (Series)Series.Create("test", [1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Series>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        var series2 = Assert.IsType<Series<int>>(deserialized);
        Assert.Equivalent(series, series2);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedPolymorphic()
    {
        var series = (Series)Series.Create("test", [1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Series>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        var series2 = Assert.IsType<Series<int>>(deserialized);
        Assert.Equivalent(series, series2);
    }
}
