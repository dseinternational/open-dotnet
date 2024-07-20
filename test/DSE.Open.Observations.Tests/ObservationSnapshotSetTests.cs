// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class ObservationSnapshotSetTests
{
    [Fact]
    public void JsonRoundtrip_WithObservationCount()
    {
        // Arrange
        var set = new ObservationSnapshotSet<Observation<Count>>
        {
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(1))),
            ObservationSnapshot.ForUtcNow(Observation.Create(2, Count.FromValue(1)))
        };

        // Act
        var json = JsonSerializer.Serialize(set);
        var deserialized = JsonSerializer.Deserialize<ObservationSnapshotSet<Observation<Count>>>(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(set, deserialized);
    }


    [Fact]
    public void JsonRoundtrip_WithObservationBool()
    {
        // Arrange
        var set = new ObservationSnapshotSet<Observation<bool>>
        {
            ObservationSnapshot.ForUtcNow(Observation.Create(1, true)),
            ObservationSnapshot.ForUtcNow(Observation.Create(2, false))
        };

        // Act
        var json = JsonSerializer.Serialize(set);
        var deserialized = JsonSerializer.Deserialize<ObservationSnapshotSet<Observation<bool>>>(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(set, deserialized);
    }

    [Fact]
    public void JsonRoundtrip_WithObservationAmount()
    {
        // Arrange
        var set = new ObservationSnapshotSet<Observation<Amount>>
        {
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Amount.FromValue(1.2m))),
            ObservationSnapshot.ForUtcNow(Observation.Create(2, Amount.FromValue(2.1m)))
        };

        // Act
        var json = JsonSerializer.Serialize(set);
        var deserialized = JsonSerializer.Deserialize<ObservationSnapshotSet<Observation<Amount>>>(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(set, deserialized);
    }

    [Fact]
    public void ObservationSnapshotSet_ShouldNotHoldDuplicates()
    {
        // Arrange
        var set1 = new ObservationSnapshotSet<Observation<Count>>
        {
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(1))),
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(1))),
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(3))),
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(2))),
        };

        var set2 = new ObservationSnapshotSet<Observation<Count>>
        {
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(2))),
        };

        // Assert
        Assert.Equal(set1, set2);
    }

    [Fact]
    public void New_WithCollection_ShouldBeEquivalent()
    {
        // Arrange
        var set1 = new ObservationSnapshotSet<Observation<Count>>
        {
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(1))),
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(1))),
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(3))),
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(2))),
        };

        var set2 = new ObservationSnapshotSet<Observation<Count>>
        {
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(2))),
        };

        var set3 = new ObservationSnapshotSet<Observation<Count>>([
            ObservationSnapshot.ForUtcNow(Observation.Create(1, Count.FromValue(2)))
        ]);

        // Assert
        Assert.Equal(set1, set2);
        Assert.Equal(set1, set3);
        Assert.Equal(set2, set3);
    }

    [Fact]
    public void Deserialize_WithWhat_ShouldDoWhat()
    {
        // Arrange
        const string json =
            """
            [{"o":{"i":5506732958415133,"m":1,"t":1721485055917,"v":1},"t":"2024-07-20T14:17:35.920112+00:00"}]
            """;

        // Act
        var result = JsonSerializer.Deserialize<ObservationSnapshotSet<Observation<Count>>>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
