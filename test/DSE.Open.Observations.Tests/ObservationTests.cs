// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Speech;
using DSE.Open.Testing.Xunit;
using Microsoft.Extensions.Time.Testing;

namespace DSE.Open.Observations;

public sealed class ObservationTests
{
    [Fact]
    public void Create_Binary()
    {
        var tp = new FakeTimeProvider();
        var now = DateTimeOffset.Now;
        tp.SetUtcNow(now);
        var observation = Observation.Create(TestMeasures.BinaryMeasure, true, tp);
        Assert.True(observation.Value);
        Assert.Equal(now.Truncate(DateTimeTruncation.Millisecond), observation.Time);
    }

    [Fact]
    public void Create_Binary_Historic()
    {
        var tp = new FakeTimeProvider();
        var now = DateTimeOffset.Now;
        tp.SetUtcNow(now);
        var observation = Observation.CreateHistorical(
            TestMeasures.BinaryMeasure,
            true,
            now.AddDays(-7),
            tp);
        Assert.True(observation.Value);
        Assert.Equal(now.Truncate(DateTimeTruncation.Millisecond), observation.Recorded);
        Assert.Equal(now.Truncate(DateTimeTruncation.Millisecond).AddDays(-7), observation.Time);
    }

    [Fact]
    public void Create_SpeechSound_BehaviorFrequency()
    {
        var tp = new FakeTimeProvider();
        var now = DateTimeOffset.Now;
        tp.SetUtcNow(now);
        var observation = Observation.Create(
            TestMeasures.BehaviorFrequencySpeechSoundMeasure,
            SpeechSound.CloseCentralUnroundedVowel,
            BehaviorFrequency.Developing,
            tp);
        Assert.Equal(SpeechSound.CloseCentralUnroundedVowel, observation.Parameter);
        Assert.Equal(BehaviorFrequency.Developing, observation.Value);
        Assert.Equal(now.Truncate(DateTimeTruncation.Millisecond), observation.Time);
    }

    [Fact]
    public void Create_SpeechSound_BehaviorFrequency_Historic()
    {
        var tp = new FakeTimeProvider();
        var now = DateTimeOffset.Now;
        tp.SetUtcNow(now);
        var observation = Observation.CreateHistorical(
            TestMeasures.BehaviorFrequencySpeechSoundMeasure,
            SpeechSound.CloseCentralUnroundedVowel,
            BehaviorFrequency.Developing,
            now.AddDays(-7),
            tp);
        Assert.Equal(SpeechSound.CloseCentralUnroundedVowel, observation.Parameter);
        Assert.Equal(BehaviorFrequency.Developing, observation.Value);
        Assert.Equal(now.Truncate(DateTimeTruncation.Millisecond), observation.Recorded);
        Assert.Equal(now.Truncate(DateTimeTruncation.Millisecond).AddDays(-7), observation.Time);
    }

    [Fact]
    public void SerializeDeserializeBinary()
    {
        var observation = Observation.Create(TestMeasures.BinaryMeasure, true);
        var json = JsonSerializer.Serialize(observation);
        var deserialized = JsonSerializer.Deserialize<Observation<Binary>>(json);
        Assert.Equal(observation, deserialized);
    }

    [Fact]
    public void SerializeDeserialize_SpeechSound_BehaviorFrequency_Historic()
    {
        var tp = new FakeTimeProvider();
        var now = DateTimeOffset.Now;
        tp.SetUtcNow(now);
        var observation = Observation.CreateHistorical(
            TestMeasures.BehaviorFrequencySpeechSoundMeasure,
            SpeechSound.CloseCentralUnroundedVowel,
            BehaviorFrequency.Developing,
            now.AddDays(-7),
            tp);
        var json = JsonSerializer.Serialize(observation);
        var deserialized = JsonSerializer.Deserialize<Observation<BehaviorFrequency, SpeechSound>>(json);
        Assert.NotNull(deserialized);
        Assert.Equal(observation.Id, deserialized.Id);
        Assert.Equal(SpeechSound.CloseCentralUnroundedVowel, deserialized.Parameter);
        Assert.Equal(BehaviorFrequency.Developing, deserialized.Value);
        Assert.Equal(now.Truncate(DateTimeTruncation.Millisecond), deserialized.Recorded);
        Assert.Equal(now.Truncate(DateTimeTruncation.Millisecond).AddDays(-7), deserialized.Time);
    }

    [Fact]
    public void JsonRoundtrip()
    {
        Observation[] observations =
        [
            Observation.Create(TestMeasures.BinaryMeasure, true),
            Observation.Create(TestMeasures.CountMeasure, (Count)42),
            Observation.Create(TestMeasures.AmountMeasure, (Amount)42.0m),
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true),
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, false),
        ];

        foreach (var observation in observations)
        {
            AssertJson.Roundtrip(observation, JsonContext.Default.Observation);
        }
    }

    [Fact]
    public void GetMeasurementHashCode_ShouldReturnMeasurementHashCode()
    {
        // Arrange
        var observation = Observation.Create(TestMeasures.BinaryMeasure, true);
        var expected = HashCode.Combine(observation.MeasureId);

        // Act
        var hashCode = observation.GetMeasurementHashCode();

        // Assert
        Assert.Equal(expected, hashCode);
        Assert.Equal(expected, observation.GetMeasurementHashCode());
    }

    [Fact]
    public void GetMeasurementHashCode_WithParam_ShouldReturnMeasurementAndParamHashCode()
    {
        // Arrange
        var obs = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var expected = HashCode.Combine(HashCode.Combine(obs.MeasureId), obs.Parameter);

        // Act
        var binarySpeechSoundHashCode = obs.GetMeasurementHashCode();

        // Assert
        Assert.Equal(expected, binarySpeechSoundHashCode);
        Assert.Equal(expected, obs.GetMeasurementHashCode());
    }

    [Fact]
    public void GetMeasurementHashCode_ShouldDistinguishByParam()
    {
        // Arrange
        var obs1 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true);
        var obs2 = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, true);

        // Act
        var obs1HashCode = obs1.GetMeasurementHashCode();
        var obs2HashCode = obs2.GetMeasurementHashCode();

        // Assert
        Assert.NotEqual(obs1HashCode, obs2HashCode);
    }

    [Fact]
    public void JsonSmokeTest_Abstract()
    {
        // Arrange
        const string expected = """{"d":101,"i":3840829473618409,"t":1733011200000,"m":203189995833,"v":"1"}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409");

        var observation = new Observation<Binary>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            true,
            timeProvider);

        // Act
        var json = JsonSerializer.Serialize(observation, JsonContext.Default.Observation);

        // Assert
        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonSmokeTest_Concrete()
    {
        // Arrange
        const string expected = """{"i":3840829473618409,"t":1733011200000,"m":203189995833,"v":"1"}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409");

        var observation = new Observation<Binary>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            true,
            timeProvider);

        // Act
        var json = JsonSerializer.Serialize(observation, JsonContext.Default.ObservationBinary);

        // Assert
        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonSmokeTest_Param_Concrete()
    {
        // Arrange
        const string expected = """{"i":3840829473618409,"t":1733011200000,"m":203189995833,"p":"e\u026A","v":90}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409");

        var observation = new Observation<BehaviorFrequency, SpeechSound>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            Phonemes.English.ay.Abstraction,
            BehaviorFrequency.Achieved,
            timeProvider);

        // Act
        var json = JsonSerializer.Serialize(observation, JsonContext.Default.ObservationBehaviorFrequencySpeechSound);

        // Assert
        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonSmokeTest_Interface()
    {
        // Arrange
        const string expected = """{"i":3840829473618409,"m":203189995833,"t":1733011200000,"v":"1"}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409");

        var observation = new Observation<Binary>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            true,
            timeProvider);

        // Act
        var json = JsonSerializer.Serialize(observation, JsonContext.Default.IObservation);

        // Assert
        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonSmokeTest_Param_Interface()
    {
        // Arrange
        const string expected = """{"i":3840829473618409,"m":203189995833,"t":1733011200000,"v":90,"p":"e\u026A"}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409");

        var observation = new Observation<BehaviorFrequency, SpeechSound>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            Phonemes.English.ay.Abstraction,
            BehaviorFrequency.Achieved,
            timeProvider);

        // Act
        var json = JsonSerializer.Serialize(observation, JsonContext.Default.IObservation);

        // Assert
        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonInterfaceToConcreteRoundtrip()
    {
        // Arrange
        var observation = Observation.Create(TestMeasures.BinaryMeasure, Binary.True);

        // Act
        var json = JsonSerializer.Serialize(observation, JsonContext.Default.IObservation);
        var deserialized = JsonSerializer.Deserialize(json, JsonContext.Default.ObservationBinary);

        // Assert
        Assert.Equal(observation, deserialized);
    }

    [Fact]
    public void JsonInterfaceToConcreteRoundtrip_Param()
    {
        // Arrange
        var observation = Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, Binary.True);

        // Act
        var json = JsonSerializer.Serialize(observation, JsonContext.Default.IObservation);
        var deserialized = JsonSerializer.Deserialize(json, JsonContext.Default.ObservationBinarySpeechSound);

        // Assert
        Assert.Equal(observation, deserialized);
    }

    [Fact]
    public void New_WithTime_ShouldTruncateToMilliseconds()
    {
        // Arrange
        var time = DateTimeOffset.Now;
        var recorded = time.AddMilliseconds(123);

        // Act
        var observation = new Observation<Binary>(
            ObservationId.GetRandomId(),
            time,
            recorded,
            TestMeasures.BinaryMeasure.Id,
            true);

        // Assert
        Assert.Equal(time.Truncate(DateTimeTruncation.Millisecond), observation.Time);
        Assert.Equal(recorded.Truncate(DateTimeTruncation.Millisecond), observation.Recorded);
    }
}
