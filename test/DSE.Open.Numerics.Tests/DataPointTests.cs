// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public class DataPointTests : LoggedTestsBase
{
    public DataPointTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create()
    {
        var point = DataPoint.Create(1, 2);
        Assert.Equal(1, point.X);
        Assert.Equal(2, point.Y);
    }

    [Fact]
    public void CreateRange()
    {
        var x = new[] { 1, 2, 3 };
        var y = new[] { 4, 5, 6 };

        var points = DataPoint.CreateRange(x, y).ToArray();

        Assert.Equal(3, points.Length);

        for (var i = 0; i < points.Length; i++)
        {
            Assert.Equal(x[i], points[i].X);
            Assert.Equal(y[i], points[i].Y);
        }
    }

    [Fact]
    public void SerializeDeserializeReflected()
    {
        var point = DataPoint.Create(1, 2);

        var json = JsonSerializer.Serialize(point, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<DataPoint<int>>(json, NumericsJsonSharedOptions.Reflected);

        Assert.Equal(1, deserialized.X);
        Assert.Equal(2, deserialized.Y);
        Assert.Equal(point, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGenerated()
    {
        var point = DataPoint.Create(1, 2);

        var json = JsonSerializer.Serialize(point, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<DataPoint<int>>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.Equal(1, deserialized.X);
        Assert.Equal(2, deserialized.Y);
        Assert.Equal(point, deserialized);
    }
}
