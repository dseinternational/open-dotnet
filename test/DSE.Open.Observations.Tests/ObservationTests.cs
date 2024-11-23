// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class ObservationTests
{
    [Fact]
    public void SerializeDeserializeBinary()
    {
        var observation = Observation.Create(TestMeasures.BinaryMeasure, true);
        var json = JsonSerializer.Serialize(observation);
        var deserialized = JsonSerializer.Deserialize<Observation<bool>>(json);
        Assert.Equal(observation, deserialized);
    }

    [Fact]
    public void SerializeDeserializeCollection()
    {
        Observation[] observations =
        [
            Observation.Create(TestMeasures.BinaryMeasure, true),
            Observation.Create(TestMeasures.CountMeasure,(Count)42)
        ];

        var json = JsonSerializer.Serialize(observations);

        var deserialized = JsonSerializer.Deserialize<Observation[]>(json);

        Assert.NotNull(deserialized);
        Assert.Equal(observations[0], deserialized[0]);
    }
}
