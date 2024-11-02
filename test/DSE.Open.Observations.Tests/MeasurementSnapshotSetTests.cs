// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed class MeasurementSnapshotSetTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void JsonRoundtrip_WithObservationCount(bool useContext)
    {
        // Arrange
        var set = new MeasurementSnapshotSet<CountSnapshot, CountObservation, Count>
        {
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(1))),
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure2, Count.FromValue(1)))
        };

        // Act
        var deserialized = useContext
            ? JsonRoundtripCore(set, JsonContext.Default)
            : JsonRoundtripCore(set);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(set, deserialized); // xUnit HashSet equality calls `SetEquals`
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void JsonRoundtrip_WithObservationBool(bool useContext)
    {
        // Arrange
        var set = new MeasurementSnapshotSet<BinarySnapshot, BinaryObservation, bool>
        {
            BinarySnapshot.ForUtcNow(BinaryObservation.Create(TestMeasures.CountMeasure, true)),
            BinarySnapshot.ForUtcNow(BinaryObservation.Create(TestMeasures.CountMeasure2, false))
        };

        // Act
        var deserialized = useContext
            ? JsonRoundtripCore(set, JsonContext.Default)
            : JsonRoundtripCore(set);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(set, deserialized); // xUnit HashSet equality calls `SetEquals`
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void JsonRoundtrip_WithObservationAmount(bool useContext)
    {
        // Arrange
        var set = new MeasurementSnapshotSet<AmountSnapshot, AmountObservation, Amount>
        {
            AmountSnapshot.ForUtcNow(AmountObservation.Create(TestMeasures.CountMeasure, Amount.FromValue(1.2m))),
            AmountSnapshot.ForUtcNow(AmountObservation.Create(TestMeasures.CountMeasure2, Amount.FromValue(2.1m)))
        };

        // Act
        var deserialized = useContext
            ? JsonRoundtripCore(set, JsonContext.Default)
            : JsonRoundtripCore(set);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(set, deserialized); // xUnit HashSet equality calls `SetEquals`
    }

    private static TSet? JsonRoundtripCore<TSet>(TSet set, JsonSerializerContext context)
    {
        var json = JsonSerializer.Serialize(set, typeof(TSet), context);
        return (TSet?)JsonSerializer.Deserialize(json, typeof(TSet), context);
    }

    private static TSet? JsonRoundtripCore<TSet>(TSet set)
    {
        var json = JsonSerializer.Serialize(set);
        return JsonSerializer.Deserialize<TSet>(json);
    }

    [Fact]
    public void SnapshotSet_ShouldNotHoldDuplicates()
    {
        // Arrange
        var set1 = new MeasurementSnapshotSet<CountSnapshot, CountObservation, Count>
        {
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(1))),
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(1))),
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(3))),
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(2))),
        };

        var set2 = new MeasurementSnapshotSet<CountSnapshot, CountObservation, Count>
        {
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(2))),
        };

        // Assert
        Assert.Equal(set1, set2); // xUnit HashSet equality calls `SetEquals`
    }

    [Fact]
    public void New_WithCollection_ShouldBeEquivalent()
    {
        // Arrange
        var set1 = new MeasurementSnapshotSet<CountSnapshot, CountObservation, Count>
        {
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(1))),
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(1))),
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(3))),
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(2))),
        };

        var set2 = new MeasurementSnapshotSet<CountSnapshot, CountObservation, Count>
        {
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(2))),
        };

        var set3 = new MeasurementSnapshotSet<CountSnapshot, CountObservation, Count>([
            CountSnapshot.ForUtcNow(CountObservation.Create(TestMeasures.CountMeasure, Count.FromValue(2)))
        ]);

        // Assert
        // xUnit HashSet equality calls `SetEquals`
        Assert.Equal(set1, set2);
        Assert.Equal(set1, set3);
        Assert.Equal(set2, set3);
    }

    [Fact]
    public void Deserialize_Compat()
    {
        // Arrange
        const string json =
            """
            [{"o":{"i":5506732958415133,"m":100000000001,"t":1721485055917,"v":1},"t":1721485057117}]
            """;

        // Act
        var result = JsonSerializer.Deserialize<MeasurementSnapshotSet<CountSnapshot, CountObservation, Count>>(json);

        // Assert
        Assert.NotNull(result);
        _ = Assert.Single(result);
    }
}
