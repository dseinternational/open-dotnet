// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Numerics.Serialization;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;

namespace DSE.Open.Numerics;

public class SeriesTests : LoggedTestsBase
{
    private static readonly Lazy<JsonSerializerOptions> s_jsonOptions = new(() =>
    {
        var options = new JsonSerializerOptions(JsonSharedOptions.RelaxedJsonEscaping);
        options.AddDefaultNumericsJsonConverters();
        return options;
    });

    public SeriesTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var series = Series.CreateNumeric("test", [1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, s_jsonOptions.Value);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<NumericSeries<int>>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);
        Assert.Equivalent(series, deserialized);
    }
    [Fact]
    public void SerializeDeserialize_Polymorphic()
    {
        var series = (Series)Series.CreateNumeric("test", [1, 2, 3, 4, 5]);

        var json = JsonSerializer.Serialize(series, s_jsonOptions.Value);

        Output.WriteLine(json);

        Assert.NotNull(json);

        var deserialized = JsonSerializer.Deserialize<Series>(json, s_jsonOptions.Value);

        Assert.NotNull(deserialized);
        var series2 = Assert.IsType<NumericSeries<int>>(deserialized);
        Assert.Equivalent(series, series2);
    }
}
