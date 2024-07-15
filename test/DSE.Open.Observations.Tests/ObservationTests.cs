// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public class ObservationTests : LoggedTestsBase
{
    public ObservationTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CanSerialize()
    {
        var obs = new Observation<bool>
        {
            MeasureId = 68436815,
            Value = true,
            Time = DateTimeOffset.FromUnixTimeMilliseconds(1721047510118)
        };
        var json = JsonSerializer.Serialize(obs, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.Equal("{\"m\":68436815,\"t\":1721047510118,\"v\":true}", json);
        Output.WriteLine(json);
    }

    [Fact]
    public void CanDeserialize()
    {
        var obs = JsonSerializer.Deserialize<Observation<bool>>(
            "{\"m\":68436815,\"t\":1721047510118,\"v\":true}",
            JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(obs);
        Assert.Equal(68436815u, obs.MeasureId);
        Assert.True(obs.Value);
        Assert.Equal(1721047510118, obs.Time.ToUnixTimeMilliseconds());
    }
}
