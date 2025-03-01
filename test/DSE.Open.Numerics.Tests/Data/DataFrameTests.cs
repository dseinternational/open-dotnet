// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics.Data;

public class DataFrameTests : LoggedTestsBase
{
    private static readonly Lazy<JsonSerializerOptions> s_jsonOptions = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping);
        options.AddDefaultNumericsJsonConverters();
        return options;
    });

    public DataFrameTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Indexer_Get()
    {
        var series1 = Series.CreateNumeric("series1", [1, 2, 3, 4, 5]);
        var series2 = Series.CreateNumeric("series2", [5, 4, 3, 2, 1]);

        var frame = new DataFrame();
        frame.Columns.Add(series1);
        frame.Columns.Add(series2);

        Assert.Same(series1, frame[0]);
        Assert.Same(series2, frame[1]);
    }

    [Fact]
    public void SerializeDeserializeInt32()
    {
        var series1 = Series.CreateNumeric("series1", [1, 2, 3, 4, 5]);
        var series2 = Series.CreateNumeric("series2", [5, 4, 3, 2, 1]);

        var frame = new DataFrame();
        frame.Columns.Add(series1);
        frame.Columns.Add(series2);

        var json = JsonSerializer.Serialize(frame, s_jsonOptions.Value);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeFloat()
    {
        var series1 = Series.CreateNumeric("series1", [.. Enumerable.Range(5000, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]);
        var series2 = Series.CreateNumeric("series2", [.. Enumerable.Range(-77777, 50).Select(i => i / 333f), float.MinValue, float.MaxValue]);

        var frame = new DataFrame();
        frame.Columns.Add(series1);
        frame.Columns.Add(series2);

        var json = JsonSerializer.Serialize(frame, s_jsonOptions.Value);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeDouble()
    {
        var series1 = Series.CreateNumeric("series1", [.. Enumerable.Range(500000, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]);
        var series2 = Series.CreateNumeric("series2", [.. Enumerable.Range(-7777777, 50).Select(i => i / 333.3), double.MinValue, double.MaxValue]);

        var frame = new DataFrame();
        frame.Columns.Add(series1);
        frame.Columns.Add(series2);

        var json = JsonSerializer.Serialize(frame, s_jsonOptions.Value);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeDateTime64Int64()
    {

        var series1 = Series.CreateNumeric("series1", [DateTime64.Now, DateTime64.Now, DateTime64.Now]);
        var series2 = Series.CreateNumeric("series2", [5L, 4L, 3L]);

        var frame = new DataFrame();

        frame.Columns.Add(series1);
        frame.Columns.Add(series2);

        var json = JsonSerializer.Serialize(frame, s_jsonOptions.Value);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }

    [Fact]
    public void SerializeDeserializeDateMixed()
    {

        var series1 = Series.CreateNumeric("series1", [DateTime64.Now, DateTime64.Now, DateTime64.Now]);
        var series2 = Series.CreateNumeric("series2", [5L, 4L, 3L]);
        var series3 = Series.CreateNumeric("series3", [186352.1111, 0.0000067, -6984135]);
        var series4 = Series.CreateNumeric("series4", [(byte)1, (byte)2, (byte)3]);

        var frame = new DataFrame();

        frame.Columns.Add(series1);
        frame.Columns.Add(series2);
        frame.Columns.Add(series3);
        frame.Columns.Add(series4);

        var json = JsonSerializer.Serialize(frame, s_jsonOptions.Value);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<DataFrame>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);
        Assert.Equivalent(frame, deserialized);
    }
}
