// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics;

public class DataFrameTests : LoggedTestsBase
{
    public DataFrameTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void IndexerGet()
    {
        var series1 = Series.Create([1, 2, 3, 4, 5]);
        var series2 = Series.Create([5, 4, 3, 2, 1]);

        var frame = new DataFrame
        {
            series1,
            series2
        };

        Assert.Same(series1, frame[0]);
        Assert.Same(series2, frame[1]);
    }

    [Fact]
    public void SerializeDeserializeReflectedInt32()
    {
        var series1 = Series.Create([.. Enumerable.Range(0, 100)]);
        var series2 = Series.Create([.. Enumerable.Range(0, 100)]);

        var frame = new DataFrame
        {
            series1,
            series2
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedFloat()
    {
        var series1 = Series.Create([.. Enumerable.Range(5000, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]);
        var series2 = Series.Create([.. Enumerable.Range(-77777, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]);

        var frame = new DataFrame
        {
            series1,
            series2
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedDouble()
    {
        var series1 = Series.Create([.. Enumerable.Range(500000, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]);
        var series2 = Series.Create([.. Enumerable.Range(-7777777, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]);

        var frame = new DataFrame
        {
            series1,
            series2
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedDateTime64Int64()
    {

        var series1 = Series.Create([DateTime64.Now, DateTime64.Now, DateTime64.Now]);
        var series2 = Series.Create([5L, 4L, 3L]);

        var frame = new DataFrame
        {
            series1,
            series2
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedDateMixed()
    {

        var series1 = Series.Create([DateTime64.Now, DateTime64.Now, DateTime64.Now]);
        var series2 = Series.Create([5L, 4L, 3L]);
        var series3 = Series.Create([186352.1111, 0.0000067, -6984135]);
        var series4 = Series.Create([(byte)1, (byte)2, (byte)3]);

        var frame = new DataFrame
        {
            series1,
            series2,
            series3,
            series4
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedInt32()
    {
        var series1 = Series.Create([1, 2, 3, 4, 5]);
        var series2 = Series.Create([5, 4, 3, 2, 1]);

        var frame = new DataFrame
        {
            series1,
            series2
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedFloat()
    {
        var series1 = Series.Create([.. Enumerable.Range(5000, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]);
        var series2 = Series.Create([.. Enumerable.Range(-77777, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]);

        var frame = new DataFrame
        {
            series1,
            series2
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedDouble()
    {
        var series1 = Series.Create([.. Enumerable.Range(500000, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]);
        var series2 = Series.Create([.. Enumerable.Range(-7777777, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]);

        var frame = new DataFrame
        {
            series1,
            series2
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedDateTime64Int64()
    {

        var series1 = Series.Create([DateTime64.Now, DateTime64.Now, DateTime64.Now]);
        var series2 = Series.Create([5L, 4L, 3L]);

        var frame = new DataFrame
        {
            series1,
            series2
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedDateMixed()
    {

        var series1 = Series.Create([DateTime64.Now, DateTime64.Now, DateTime64.Now]);
        var series2 = Series.Create([5L, 4L, 3L]);
        var series3 = Series.Create([186352.1111, 0.0000067, -6984135]);
        var series4 = Series.Create([(byte)1, (byte)2, (byte)3]);

        var frame = new DataFrame
        {
            series1,
            series2,
            series3,
            series4
        };

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }
}
