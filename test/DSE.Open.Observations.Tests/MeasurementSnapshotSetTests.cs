// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class MeasurementSnapshotSetTests
{
    [Fact]
    public void JsonRoundtrip_WithObservationCount()
    {
        // Arrange
        var set = new MeasurementSnapshotSet<CountObservationSnapshot, CountObservation, Count>
        {
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(1))),
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(2, Count.FromValue(1)))
        };

        // Act
        var json = JsonSerializer.Serialize(set);
        var deserialized = JsonSerializer.Deserialize<MeasurementSnapshotSet<CountObservationSnapshot, CountObservation, Count>>(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(set, deserialized); // xUnit HashSet equality calls `SetEquals`
    }

    [Fact]
    public void JsonRoundtrip_WithObservationBool()
    {
        // Arrange
        var set = new MeasurementSnapshotSet<BinaryObservationSnapshot, BinaryObservation, bool>
        {
            BinaryObservationSnapshot.ForUtcNow(BinaryObservation.Create(1, true)),
            BinaryObservationSnapshot.ForUtcNow(BinaryObservation.Create(2, false))
        };

        // Act
        var json = JsonSerializer.Serialize(set);
        var deserialized = JsonSerializer.Deserialize<MeasurementSnapshotSet<BinaryObservationSnapshot, BinaryObservation, bool>>(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(set, deserialized); // xUnit HashSet equality calls `SetEquals`
    }

    [Fact]
    public void JsonRoundtrip_WithObservationAmount()
    {
        // Arrange
        var set = new MeasurementSnapshotSet<AmountObservationSnapshot, AmountObservation, Amount>
        {
            AmountObservationSnapshot.ForUtcNow(AmountObservation.Create(1, Amount.FromValue(1.2m))),
            AmountObservationSnapshot.ForUtcNow(AmountObservation.Create(2, Amount.FromValue(2.1m)))
        };

        // Act
        var json = JsonSerializer.Serialize(set);
        var deserialized = JsonSerializer.Deserialize<MeasurementSnapshotSet<AmountObservationSnapshot, AmountObservation, Amount>>(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(set, deserialized); // xUnit HashSet equality calls `SetEquals`
    }

    [Fact]
    public void ObservationSnapshotSet_ShouldNotHoldDuplicates()
    {
        // Arrange
        var set1 = new MeasurementSnapshotSet<CountObservationSnapshot, CountObservation, Count>
        {
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(1))),
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(1))),
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(3))),
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(2))),
        };

        var set2 = new MeasurementSnapshotSet<CountObservationSnapshot, CountObservation, Count>
        {
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(2))),
        };

        // Assert
        Assert.Equal(set1, set2); // xUnit HashSet equality calls `SetEquals`
    }

    [Fact]
    public void New_WithCollection_ShouldBeEquivalent()
    {
        // Arrange
        var set1 = new MeasurementSnapshotSet<CountObservationSnapshot, CountObservation, Count>
        {
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(1))),
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(1))),
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(3))),
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(2))),
        };

        var set2 = new MeasurementSnapshotSet<CountObservationSnapshot, CountObservation, Count>
        {
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(2))),
        };

        var set3 = new MeasurementSnapshotSet<CountObservationSnapshot, CountObservation, Count>([
            CountObservationSnapshot.ForUtcNow(CountObservation.Create(1, Count.FromValue(2)))
        ]);

        // Assert
        // xUnit HashSet equality calls `SetEquals`
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
            [{"o":{"i":5506732958415133,"m":1,"t":1721485055917,"v":1},"t":1721485057117}]
            """;

        // Act
        var result = JsonSerializer.Deserialize<MeasurementSnapshotSet<CountObservationSnapshot, CountObservation, Count>>(json);

        // Assert
        Assert.NotNull(result);
        _ = Assert.Single(result);
    }
}
