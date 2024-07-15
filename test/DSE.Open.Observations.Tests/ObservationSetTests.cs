// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public class ObservationSetTests : LoggedTestsBase
{
    private const string Serialized = "{\"trk\":\"SQklNhktDJ4MaEDBDJsOLnaFAHlssKROaVhx8em2ZiRLLqSL\",\"obr\":\"gWegtqxJ7fqAXU4vfOroDOh8ge4qfkb5yXzqaZmgMpNQVTJx\",\"src\":\"https://test.dsegroup.net\",\"obs\":[{\"m\":68436815,\"t\":1721047510118,\"v\":true},{\"m\":68436815,\"t\":1721047513548,\"v\":false}]}";

    public ObservationSetTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CanSerialize()
    {
        var obs = new ObservationSet<Observation<bool>>
        {
            ObserverReference = Identifier.ParseInvariant("gWegtqxJ7fqAXU4vfOroDOh8ge4qfkb5yXzqaZmgMpNQVTJx"),
            TrackerReference = Identifier.ParseInvariant("SQklNhktDJ4MaEDBDJsOLnaFAHlssKROaVhx8em2ZiRLLqSL"),
            Source = new Uri("https://test.dsegroup.net"),
            Observations =
            [
                new Observation<bool>
                {
                    MeasureId = 68436815,
                    Value = true,
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(1721047510118)
                },
                new Observation<bool>
                {
                    MeasureId = 68436815,
                    Value = false,
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(1721047513548)
                }
            ]
        };
        var json = JsonSerializer.Serialize(obs, JsonSharedOptions.RelaxedJsonEscaping);
        Output.WriteLine(json);
        Assert.Equal(Serialized, json);
    }

    [Fact]
    public void CanDeserialize()
    {
        var obs = JsonSerializer.Deserialize<ObservationSet<Observation<bool>>>(Serialized, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(obs);
        Assert.Equal(Identifier.ParseInvariant("gWegtqxJ7fqAXU4vfOroDOh8ge4qfkb5yXzqaZmgMpNQVTJx"), obs.ObserverReference);
        Assert.Equal(Identifier.ParseInvariant("SQklNhktDJ4MaEDBDJsOLnaFAHlssKROaVhx8em2ZiRLLqSL"), obs.TrackerReference);
        Assert.Equal(new Uri("https://test.dsegroup.net"), obs.Source);
        Assert.Collection(obs.Observations,
            obs =>
            {
                Assert.Equal(68436815u, obs.MeasureId);
                Assert.True(obs.Value);
                Assert.Equal(1721047510118, obs.Time.ToUnixTimeMilliseconds());
            },
            obs =>
            {
                Assert.Equal(68436815u, obs.MeasureId);
                Assert.False(obs.Value);
                Assert.Equal(1721047513548, obs.Time.ToUnixTimeMilliseconds());
            });
    }
}
