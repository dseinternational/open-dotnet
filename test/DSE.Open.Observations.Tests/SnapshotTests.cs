// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Speech;
using DSE.Open.Testing;
using Microsoft.Extensions.Time.Testing;

namespace DSE.Open.Observations;

public class SnapshotTests
{
    [Fact]
    public void CreateSnapshot()
    {
        FakeTimeProvider timeProvider = new();
        timeProvider.SetUtcNow(DateTimeOffset.UtcNow);

        var o = Observation.Create(TestMeasures.BinaryMeasure, true);
        var s = new Snapshot<Observation<Binary>>(o, timeProvider);

        var roundedTime = DateTimeOffset.FromUnixTimeMilliseconds(timeProvider.GetUtcNow().ToUnixTimeMilliseconds());

        Assert.Equal(o, s.Observation);
        Assert.Equal(roundedTime, s.Time);
    }

    [Fact]
    public void SerializeDeserialize()
    {
        var o = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, false);
        var s = new Snapshot<Observation<Binary, SpeechSound>>(o);

        var json = JsonSerializer.Serialize(s);

        var s2 = JsonSerializer.Deserialize<Snapshot<Observation<Binary, SpeechSound>>>(json);

        Assert.Equal(s, s2);
    }

    [Fact]
    public void JsonSmokeTest_Concrete()
    {
        // Arrange
        const string expected =
            """{"o":{"i":3840829473618409,"t":1733011200000,"m":728752763463,"v":42},"t":1733011201000}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409");

        var observation = new Observation<Count>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.CountMeasure.Id,
            Count.FromValue(42),
            timeProvider);

        timeProvider.AddSeconds(1);

        var snapshot = new Snapshot<Observation<Count>>(observation, timeProvider);

        // Act
        var json = JsonSerializer.Serialize(snapshot, JsonContext.Default.SnapshotObservationCount);

        // Assert
        Assert.Equal(expected, json);
    }


    [Fact]
    public void JsonSmokeTest_Interface()
    {
        // Arrange
        const string expected =
            """{"o":{"i":3840829473618409,"m":728752763463,"t":1733011200000,"v":42},"t":1733011201000}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409");

        var observation = new Observation<Count>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.CountMeasure.Id,
            Count.FromValue(42),
            timeProvider);

        timeProvider.AddSeconds(1);

        var snapshot = new Snapshot<Observation<Count>>(observation, timeProvider);

        // Act
        var json = JsonSerializer.Serialize(snapshot, JsonContext.Default.ISnapshot);

        // Assert
        Assert.Equal(expected, json);
    }

    [Fact]
    public void Json_DeserializeFromInterface()
    {
        // Arrange
        var observation = Observation.Create(TestMeasures.CountMeasure, Count.FromValue(42));
        var snapshot = new Snapshot<Observation<Count>>(observation);

        // Act
        var json = JsonSerializer.Serialize(snapshot, JsonContext.Default.ISnapshot);
        var deserialized = JsonSerializer.Deserialize(json, JsonContext.Default.SnapshotObservationCount);

        // Assert
        Assert.Equal(snapshot, deserialized);
    }
}
