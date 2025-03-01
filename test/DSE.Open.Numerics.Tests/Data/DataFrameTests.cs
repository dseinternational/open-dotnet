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
    public void SerializeDeserialize()
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
}
