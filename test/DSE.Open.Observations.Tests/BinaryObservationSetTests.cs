// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public class BinaryObservationSetTests
{
    [Fact]
    public void CanSerializeAndDeserialize()
    {
        var obs = BinaryObservationSet.Create(
            Identifier.New("trk"),
            Identifier.New("obr"),
            new Uri("https://test.dsegroup.net"),
            [
                BinaryObservation.Create(68436815, true),
                BinaryObservation.Create(68436815, false),
            ]);
        var json = JsonSerializer.Serialize(obs, JsonSharedOptions.RelaxedJsonEscaping);
        var deserialized = JsonSerializer.Deserialize<BinaryObservationSet>(json, JsonSharedOptions.RelaxedJsonEscaping);
        Assert.NotNull(deserialized);
        Assert.Equal(obs.Id, deserialized.Id);
        Assert.Equal(obs.Created.ToUnixTimeMilliseconds(), deserialized.Created.ToUnixTimeMilliseconds());
        Assert.Equal(obs.TrackerReference, deserialized.TrackerReference);
        Assert.Equal(obs.ObserverReference, deserialized.ObserverReference);
        Assert.Equal(obs.Source, deserialized.Source);
        Assert.Equal(obs.Location, deserialized.Location);
        Assert.Equal(obs.Observations.Count, deserialized.Observations.Count);
    }
}
