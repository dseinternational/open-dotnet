// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Language;
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
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

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
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

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
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

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
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

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
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

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
    public void RepeatableHash_Value_DistinguishesByValue()
    {
        var tp = new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        var obs1 = new Observation<Binary>(id, tp.GetUtcNow(), TestMeasures.BinaryMeasure.Id, Binary.True, tp);
        var obs2 = new Observation<Binary>(id, tp.GetUtcNow(), TestMeasures.BinaryMeasure.Id, Binary.False, tp);

        Assert.NotEqual(obs1.GetRepeatableHashCode(), obs2.GetRepeatableHashCode());
    }

    [Fact]
    public void RepeatableHash_ValueParam_DistinguishesByValue()
    {
        // Regression: Observation<TValue, TParam>.GetRepeatableHashCode previously hashed
        // Parameter but not Value, so two observations differing only by Value collided.
        var tp = new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        var obs1 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ay.Abstraction, BehaviorFrequency.Achieved, tp);
        var obs2 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ay.Abstraction, BehaviorFrequency.Developing, tp);

        Assert.NotEqual(obs1.GetRepeatableHashCode(), obs2.GetRepeatableHashCode());
    }

    [Fact]
    public void RepeatableHash_ValueParam_DistinguishesByParameter()
    {
        var tp = new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        var obs1 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ay.Abstraction, BehaviorFrequency.Achieved, tp);
        var obs2 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ch.Abstraction, BehaviorFrequency.Achieved, tp);

        Assert.NotEqual(obs1.GetRepeatableHashCode(), obs2.GetRepeatableHashCode());
    }

    [Fact]
    public void RepeatableHash_ValueParam_StableAcrossInstances()
    {
        var tp = new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        var obs1 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ay.Abstraction, BehaviorFrequency.Achieved, tp);
        var obs2 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ay.Abstraction, BehaviorFrequency.Achieved, tp);

        Assert.Equal(obs1.GetRepeatableHashCode(), obs2.GetRepeatableHashCode());
    }

    [Fact]
    public void GetHashCode_ValueParam_DistinguishesByParameter()
    {
        // Regression: Observation<TValue, TParam>.GetHashCode previously combined Value
        // with the base hash but ignored Parameter, while Equals checked Parameter —
        // breaking the Equals/GetHashCode contract for two observations differing only
        // by Parameter.
        var tp = new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        var obs1 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ay.Abstraction, BehaviorFrequency.Achieved, tp);
        var obs2 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ch.Abstraction, BehaviorFrequency.Achieved, tp);

        Assert.NotEqual(obs1, obs2);
        Assert.NotEqual(obs1.GetHashCode(), obs2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ValueParam_StableAcrossEqualInstances()
    {
        var tp = new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        var obs1 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ay.Abstraction, BehaviorFrequency.Achieved, tp);
        var obs2 = new Observation<BehaviorFrequency, SpeechSound>(
            id, tp.GetUtcNow(), TestMeasures.BehaviorFrequencySpeechSoundMeasure.Id,
            Phonemes.English.ay.Abstraction, BehaviorFrequency.Achieved, tp);

        Assert.Equal(obs1, obs2);
        Assert.Equal(obs1.GetHashCode(), obs2.GetHashCode());
    }

    [Fact]
    public void Equals_RecordedDiffers_NotEqual()
    {
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);
        var time = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var obs1 = new Observation<Binary>(
            id,
            time,
            time,
            TestMeasures.BinaryMeasure.Id,
            true);
        var obs2 = new Observation<Binary>(
            id,
            time,
            time.AddMinutes(1),
            TestMeasures.BinaryMeasure.Id,
            true);

        Assert.NotEqual(obs1, obs2);
        Assert.NotEqual(obs1.GetHashCode(), obs2.GetHashCode());
        Assert.NotEqual(obs1.GetRepeatableHashCode(), obs2.GetRepeatableHashCode());
    }

    [Fact]
    public void Deserialize_TimeFarInFutureOfMachineClock_Succeeds()
    {
        // Simulates a sender whose clock is ahead of the receiver (or future-dated payload).
        // The JsonConstructor path must not validate `time` against the receiver's wall-clock.
        var future = DateTimeOffset.UtcNow.AddHours(2);
        var id = ObservationId.GetRandomId();
        var json = $$"""{"i":{{id}},"t":{{future.ToUnixTimeMilliseconds()}},"m":{{TestMeasures.BinaryMeasure.Id}},"v":"1"}""";

        var observation = JsonSerializer.Deserialize(json, JsonContext.Default.ObservationBinary);

        Assert.NotNull(observation);
        Assert.Equal(future.Truncate(DateTimeTruncation.Millisecond), observation.Time);
    }

    [Fact]
    public void Deserialize_TimeBeforeMinimum_Throws()
    {
        // Floor (MinimumObservationTime = 2000-01-01) must still be enforced.
        var tooEarly = new DateTimeOffset(1999, 12, 31, 23, 59, 59, TimeSpan.Zero);
        var id = ObservationId.GetRandomId();
        var json = $$"""{"i":{{id}},"t":{{tooEarly.ToUnixTimeMilliseconds()}},"m":{{TestMeasures.BinaryMeasure.Id}},"v":"1"}""";

        _ = Assert.ThrowsAny<Exception>(() =>
            JsonSerializer.Deserialize(json, JsonContext.Default.ObservationBinary));
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

    [Fact]
    public void JsonRoundtrip_AllVariants()
    {
        var word = WordId.GetRandomId();
        var sentence = SentenceId.GetRandomId();
        var sound = Phonemes.English.ay.Abstraction;

        Observation[] observations =
        [
            Observation.Create(TestMeasures.BinaryMeasure, Binary.True),
            Observation.Create(TestMeasures.BinarySpeechSoundMeasure, sound, Binary.True),
            Observation.Create(TestMeasures.BinaryWordMeasure, word, Binary.False),
            Observation.Create(TestMeasures.BinarySentenceMeasure, sentence, Binary.True),
            Observation.Create(TestMeasures.BehaviorFrequencyMeasure, BehaviorFrequency.Developing),
            Observation.Create(TestMeasures.BehaviorFrequencySpeechSoundMeasure, sound, BehaviorFrequency.Achieved),
            Observation.Create(TestMeasures.BehaviorFrequencyWordMeasure, word, BehaviorFrequency.Emerging),
            Observation.Create(TestMeasures.BehaviorFrequencySentenceMeasure, sentence, BehaviorFrequency.Never),
            Observation.Create(TestMeasures.CountMeasure, (Count)42uL),
            Observation.Create(TestMeasures.AmountMeasure, (Amount)42.123m),
            Observation.Create(TestMeasures.SpeechClarityMeasure, SpeechClarity.Clear),
            Observation.Create(TestMeasures.SpeechClaritySpeechSoundMeasure, sound, SpeechClarity.Developing),
            Observation.Create(TestMeasures.SpeechClarityWordMeasure, word, SpeechClarity.Unclear),
            Observation.Create(TestMeasures.SpeechClaritySentenceMeasure, sentence, SpeechClarity.Clear),
            Observation.Create(TestMeasures.CompletenessMeasure, Completeness.Developing),
            Observation.Create(TestMeasures.CompletenessSpeechSoundMeasure, sound, Completeness.Partial),
            Observation.Create(TestMeasures.CompletenessWordMeasure, word, Completeness.Complete),
            Observation.Create(TestMeasures.CompletenessSentenceMeasure, sentence, Completeness.Developing),
        ];

        foreach (var observation in observations)
        {
            AssertJson.Roundtrip(observation, JsonContext.Default.Observation);
        }
    }

    [Fact]
    public void JsonRoundtrip_AllVariants_Historic()
    {
        var tp = new FakeTimeProvider(new DateTimeOffset(2025, 6, 1, 0, 0, 0, TimeSpan.Zero));
        var madeAt = tp.GetUtcNow().AddDays(-3);
        var word = WordId.GetRandomId();
        var sentence = SentenceId.GetRandomId();
        var sound = Phonemes.English.ay.Abstraction;

        Observation[] observations =
        [
            Observation.CreateHistorical(TestMeasures.BinaryMeasure, Binary.True, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.BinarySpeechSoundMeasure, sound, Binary.True, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.BinaryWordMeasure, word, Binary.False, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.BinarySentenceMeasure, sentence, Binary.True, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.BehaviorFrequencyMeasure, BehaviorFrequency.Developing, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.BehaviorFrequencySpeechSoundMeasure, sound, BehaviorFrequency.Achieved, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.BehaviorFrequencyWordMeasure, word, BehaviorFrequency.Emerging, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.BehaviorFrequencySentenceMeasure, sentence, BehaviorFrequency.Never, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.CountMeasure, (Count)42uL, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.AmountMeasure, (Amount)42.123m, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.SpeechClarityMeasure, SpeechClarity.Clear, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.SpeechClaritySpeechSoundMeasure, sound, SpeechClarity.Developing, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.SpeechClarityWordMeasure, word, SpeechClarity.Unclear, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.SpeechClaritySentenceMeasure, sentence, SpeechClarity.Clear, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.CompletenessMeasure, Completeness.Developing, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.CompletenessSpeechSoundMeasure, sound, Completeness.Partial, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.CompletenessWordMeasure, word, Completeness.Complete, madeAt, tp),
            Observation.CreateHistorical(TestMeasures.CompletenessSentenceMeasure, sentence, Completeness.Developing, madeAt, tp),
        ];

        foreach (var observation in observations)
        {
            Assert.True(observation.Recorded.HasValue);
            AssertJson.Roundtrip(observation, JsonContext.Default.Observation);
        }
    }

    [Fact]
    public void JsonRoundtrip_Polymorphic_ResolvesBackToConcreteType_AllVariants()
    {
        var word = WordId.GetRandomId();
        var sentence = SentenceId.GetRandomId();
        var sound = Phonemes.English.ay.Abstraction;

        (Observation Observation, Type ExpectedType)[] cases =
        [
            (Observation.Create(TestMeasures.BinaryMeasure, Binary.True), typeof(Observation<Binary>)),
            (Observation.Create(TestMeasures.BinarySpeechSoundMeasure, sound, Binary.True), typeof(Observation<Binary, SpeechSound>)),
            (Observation.Create(TestMeasures.BinaryWordMeasure, word, Binary.False), typeof(Observation<Binary, WordId>)),
            (Observation.Create(TestMeasures.BinarySentenceMeasure, sentence, Binary.True), typeof(Observation<Binary, SentenceId>)),
            (Observation.Create(TestMeasures.BehaviorFrequencyMeasure, BehaviorFrequency.Developing), typeof(Observation<BehaviorFrequency>)),
            (Observation.Create(TestMeasures.BehaviorFrequencySpeechSoundMeasure, sound, BehaviorFrequency.Achieved), typeof(Observation<BehaviorFrequency, SpeechSound>)),
            (Observation.Create(TestMeasures.BehaviorFrequencyWordMeasure, word, BehaviorFrequency.Emerging), typeof(Observation<BehaviorFrequency, WordId>)),
            (Observation.Create(TestMeasures.BehaviorFrequencySentenceMeasure, sentence, BehaviorFrequency.Never), typeof(Observation<BehaviorFrequency, SentenceId>)),
            (Observation.Create(TestMeasures.CountMeasure, (Count)42uL), typeof(Observation<Count>)),
            (Observation.Create(TestMeasures.AmountMeasure, (Amount)42.123m), typeof(Observation<Amount>)),
            (Observation.Create(TestMeasures.SpeechClarityMeasure, SpeechClarity.Clear), typeof(Observation<SpeechClarity>)),
            (Observation.Create(TestMeasures.SpeechClaritySpeechSoundMeasure, sound, SpeechClarity.Developing), typeof(Observation<SpeechClarity, SpeechSound>)),
            (Observation.Create(TestMeasures.SpeechClarityWordMeasure, word, SpeechClarity.Unclear), typeof(Observation<SpeechClarity, WordId>)),
            (Observation.Create(TestMeasures.SpeechClaritySentenceMeasure, sentence, SpeechClarity.Clear), typeof(Observation<SpeechClarity, SentenceId>)),
            (Observation.Create(TestMeasures.CompletenessMeasure, Completeness.Developing), typeof(Observation<Completeness>)),
            (Observation.Create(TestMeasures.CompletenessSpeechSoundMeasure, sound, Completeness.Partial), typeof(Observation<Completeness, SpeechSound>)),
            (Observation.Create(TestMeasures.CompletenessWordMeasure, word, Completeness.Complete), typeof(Observation<Completeness, WordId>)),
            (Observation.Create(TestMeasures.CompletenessSentenceMeasure, sentence, Completeness.Developing), typeof(Observation<Completeness, SentenceId>)),
        ];

        foreach (var (observation, expected) in cases)
        {
            var json = JsonSerializer.Serialize(observation, JsonContext.Default.Observation);
            var deserialized = JsonSerializer.Deserialize(json, JsonContext.Default.Observation);
            Assert.NotNull(deserialized);
            Assert.IsType(expected, deserialized);
            Assert.Equal(observation, deserialized);
        }
    }

    [Fact]
    public void JsonSmokeTest_Concrete_BehaviorFrequency()
    {
        const string expected = """{"i":3840829473618409,"t":1733011200000,"m":203189995833,"v":50}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        var observation = new Observation<BehaviorFrequency>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            BehaviorFrequency.Developing,
            timeProvider);

        var json = JsonSerializer.Serialize(observation, JsonContext.Default.ObservationBehaviorFrequency);

        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonSmokeTest_Concrete_SpeechClarity()
    {
        const string expected = """{"i":3840829473618409,"t":1733011200000,"m":203189995833,"v":50}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        var observation = new Observation<SpeechClarity>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            SpeechClarity.Developing,
            timeProvider);

        var json = JsonSerializer.Serialize(observation, JsonContext.Default.ObservationSpeechClarity);

        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonSmokeTest_Concrete_Completeness()
    {
        const string expected = """{"i":3840829473618409,"t":1733011200000,"m":203189995833,"v":50}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        var observation = new Observation<Completeness>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            Completeness.Developing,
            timeProvider);

        var json = JsonSerializer.Serialize(observation, JsonContext.Default.ObservationCompleteness);

        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonSmokeTest_Abstract_BehaviorFrequency()
    {
        const string expected = """{"d":1001,"i":3840829473618409,"t":1733011200000,"m":203189995833,"v":50}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        Observation observation = new Observation<BehaviorFrequency>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            BehaviorFrequency.Developing,
            timeProvider);

        var json = JsonSerializer.Serialize(observation, JsonContext.Default.Observation);

        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonSmokeTest_Abstract_SpeechClarity()
    {
        const string expected = """{"d":3001,"i":3840829473618409,"t":1733011200000,"m":203189995833,"v":50}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        Observation observation = new Observation<SpeechClarity>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            SpeechClarity.Developing,
            timeProvider);

        var json = JsonSerializer.Serialize(observation, JsonContext.Default.Observation);

        Assert.Equal(expected, json);
    }

    [Fact]
    public void JsonSmokeTest_Abstract_Completeness()
    {
        const string expected = """{"d":4001,"i":3840829473618409,"t":1733011200000,"m":203189995833,"v":50}""";

        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 12, 1, 0, 0, 0, TimeSpan.Zero));
        var id = ObservationId.Parse("3840829473618409", CultureInfo.InvariantCulture);

        Observation observation = new Observation<Completeness>(
            id,
            timeProvider.GetUtcNow(),
            TestMeasures.BinaryMeasure.Id,
            Completeness.Developing,
            timeProvider);

        var json = JsonSerializer.Serialize(observation, JsonContext.Default.Observation);

        Assert.Equal(expected, json);
    }
}
