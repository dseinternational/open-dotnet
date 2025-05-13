// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;

namespace DSE.Open.Numerics.Data;

public class ReadOnlyDataFrameTests : LoggedTestsBase
{
    public ReadOnlyDataFrameTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void IndexerGet()
    {
        var series1 = Series.Create("series1", [1, 2, 3, 4, 5]).AsReadOnly();
        var series2 = Series.Create("series2", [5, 4, 3, 2, 1]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2];

        Assert.Same(series1, frame[0]);
        Assert.Same(series2, frame[1]);
    }

    [Fact]
    public void SerializeDeserializeReflectedInt32()
    {
        var series1 = Series.Create("series1", [.. Enumerable.Range(0, 100)]).AsReadOnly();
        var series2 = Series.Create("series2", [.. Enumerable.Range(0, 100)]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedFloat()
    {
        var series1 = Series.Create("series1", [.. Enumerable.Range(5000, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]).AsReadOnly();
        var series2 = Series.Create("series2", [.. Enumerable.Range(-77777, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedDouble()
    {
        var series1 = Series.Create("series1", [.. Enumerable.Range(500000, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]).AsReadOnly();
        var series2 = Series.Create("series2", [.. Enumerable.Range(-7777777, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedDateTime64Int64()
    {

        var series1 = Series.Create("series1", [DateTime64.Now, DateTime64.Now, DateTime64.Now]).AsReadOnly();
        var series2 = Series.Create("series2", [5L, 4L, 3L]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeReflectedDateMixed()
    {

        var series1 = Series.Create("series1", [DateTime64.Now, DateTime64.Now, DateTime64.Now]).AsReadOnly();
        var series2 = Series.Create("series2", [5L, 4L, 3L]).AsReadOnly();
        var series3 = Series.Create("series3", [186352.1111, 0.0000067, -6984135]).AsReadOnly();
        var series4 = Series.Create("series4", [(byte)1, (byte)2, (byte)3]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2, series3, series4];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.Reflected);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.Reflected);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedInt32()
    {
        var series1 = Series.Create("series1", [1, 2, 3, 4, 5]).AsReadOnly();
        var series2 = Series.Create("series2", [5, 4, 3, 2, 1]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedFloat()
    {
        var series1 = Series.Create("series1", [.. Enumerable.Range(5000, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]).AsReadOnly();
        var series2 = Series.Create("series2", [.. Enumerable.Range(-77777, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedDouble()
    {
        var series1 = Series.Create("series1", [.. Enumerable.Range(500000, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]).AsReadOnly();
        var series2 = Series.Create("series2", [.. Enumerable.Range(-7777777, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedDateTime64Int64()
    {

        var series1 = Series.Create("series1", [DateTime64.Now, DateTime64.Now, DateTime64.Now]).AsReadOnly();
        var series2 = Series.Create("series2", [5L, 4L, 3L]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeSourceGeneratedDateMixed()
    {

        var series1 = Series.Create("series1", [DateTime64.Now, DateTime64.Now, DateTime64.Now]).AsReadOnly();
        var series2 = Series.Create("series2", [5L, 4L, 3L]).AsReadOnly();
        var series3 = Series.Create("series3", [186352.1111, 0.0000067, -6984135]).AsReadOnly();
        var series4 = Series.Create("series4", [(byte)1, (byte)2, (byte)3]).AsReadOnly();

        ReadOnlyDataFrame frame = [series1, series2, series3, series4];

        var json = JsonSerializer.Serialize(frame, NumericsJsonSharedOptions.SourceGenerated);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<ReadOnlyDataFrame>(json, NumericsJsonSharedOptions.SourceGenerated);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }
}
