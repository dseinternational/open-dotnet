// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public class DataPoint3DTests : LoggedTestsBase
{
    public DataPoint3DTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create()
    {
        var point = DataPoint.Create(1, 2, 3);
        Assert.Equal(1, point.X);
        Assert.Equal(2, point.Y);
        Assert.Equal(3, point.Z);
    }

    [Fact]
    public void SerializeDeserializeReflected()
    {
        var point = DataPoint.Create(1, 2, 3);

        var json = JsonSerializer.Serialize(point, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<DataPoint3D<int>>(json, NumericsJsonSharedOptions.Reflected);

        Assert.Equal(1, deserialized.X);
        Assert.Equal(2, deserialized.Y);
        Assert.Equal(3, deserialized.Z);
        Assert.Equal(point, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGenerated()
    {
        var point = DataPoint.Create(1, 2, 3);

        var json = JsonSerializer.Serialize(point, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(json);

        Output.WriteLine(json);

        var deserialized = JsonSerializer.Deserialize<DataPoint3D<int>>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.Equal(1, deserialized.X);
        Assert.Equal(2, deserialized.Y);
        Assert.Equal(3, deserialized.Z);
        Assert.Equal(point, deserialized);
    }
}
