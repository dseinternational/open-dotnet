// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics;

public class VectorDataSeriesSetTests : LoggedTestsBase
{
    private static readonly Lazy<JsonSerializerOptions> s_jsonOptions = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping);
        options.AddDefaultNumericsJsonConverters();
        return options;
    });

    public VectorDataSeriesSetTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void VectorDataSet_Serialize()
    {
        int[] x = [.. Enumerable.Range(0, 5)];
        int[] y0 = [.. Enumerable.Range(0, 5)];
        int[] y1 = [.. Enumerable.Range(0, 5)];

        var dataSet = new VectorDataSeriesSet<int, int>
        {
            VectorsX = [x],
            VectorsY = [y0, y1],
        };

        var dataSetJson = JsonSerializer.Serialize(dataSet, s_jsonOptions.Value);

        Assert.NotNull(dataSetJson);

        Output.WriteLine(dataSetJson);

        Assert.Equal("{\"x\":[[0,1,2,3,4]],\"y\":[[0,1,2,3,4],[0,1,2,3,4]]}", dataSetJson);

        var series = dataSet.GetDataSeries().ToArray();

        var seriesJson = JsonSerializer.Serialize(series, s_jsonOptions.Value);

        Assert.NotNull(seriesJson);

        Output.WriteLine(seriesJson);

        Assert.Equal("[{\"x\":[0,1,2,3,4],\"y\":[0,1,2,3,4]},{\"x\":[0,1,2,3,4],\"y\":[0,1,2,3,4]}]", seriesJson);

        var dataPoints = dataSet.GetDataPoints().ToArray();

        var dataPointsJson = JsonSerializer.Serialize(dataPoints, s_jsonOptions.Value);

        Assert.NotNull(dataPointsJson);

        Output.WriteLine(dataPointsJson);

        Assert.Equal("[[[0,0],[1,1],[2,2],[3,3],[4,4]],[[0,0],[1,1],[2,2],[3,3],[4,4]]]", dataPointsJson);

        // ** without DataPointArrayJsonConverter **

        dataPointsJson = JsonSerializer.Serialize(dataPoints, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(dataPointsJson);

        Output.WriteLine(dataPointsJson);

        Assert.Equal("[[{\"x\":0,\"y\":0},{\"x\":1,\"y\":1},{\"x\":2,\"y\":2},{\"x\":3,\"y\":3},{\"x\":4,\"y\":4}],[{\"x\":0,\"y\":0},{\"x\":1,\"y\":1},{\"x\":2,\"y\":2},{\"x\":3,\"y\":3},{\"x\":4,\"y\":4}]]", dataPointsJson);
    }
}
