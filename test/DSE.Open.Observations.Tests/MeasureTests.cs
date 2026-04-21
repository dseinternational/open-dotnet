// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json;
using DSE.Open.Language;
using DSE.Open.Speech;
using DSE.Open.Testing.Xunit;
using DSE.Open.Text.Json;

namespace DSE.Open.Observations;

public sealed class MeasureTests
{
    private static readonly Uri s_measureUri = new("https://schema-test.dseapi.app/testing/measure");

    [Fact]
    public void CanSerializeAndDeserialize_Binary()
    {
        var measure = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_Binary()
    {
        var measure = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure, JsonContext.Default);
    }

    [Fact]
    public void CanCreateObservation_Binary()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new Measure<Binary>(uri, MeasurementLevel.Binary, "Test measure", "[subject] does something");
        var obs = measure.CreateObservation(true);
        Assert.Equal(measure.Id, obs.MeasureId);
        Assert.True(obs.Value);
    }
    [Fact]
    public void CanSerializeAndDeserialize_Amount()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new Measure<Amount>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_Amount()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new Measure<Amount>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");
        AssertJson.Roundtrip(measure, JsonContext.Default);
    }

    [Fact]
    public void CanCreateObservation_Amount()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");
        var measure = new Measure<Amount>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");
        var obs = measure.CreateObservation((Amount)42.123m);
        Assert.Equal(measure.Id, obs.MeasureId);
        Assert.Equal((Amount)42.123m, obs.Value);
    }

    [Fact]
    public void JsonRoundtrip_BehaviorFrequency()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");

        var measure = new Measure<BehaviorFrequency>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");

        AssertJson.Roundtrip(measure, JsonContext.Default);
    }

    [Fact]
    public void Equals_SameId_ReturnsTrueAndMatchesHashCode()
    {
        var a = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "A", "[subject] does A");
        var b = new Measure<Binary>(a.Id, s_measureUri, MeasurementLevel.Binary, "B (different name)", "[subject] does B", 0);

        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        var a = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "Test", "[subject] does something");
        var b = new Measure<Binary>(new Uri("https://schema-test.dseapi.app/testing/other"), MeasurementLevel.Binary, "Test", "[subject] does something");

        Assert.False(a.Equals(b));
        Assert.False(a.Equals((object)b));
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        var a = new Measure<Binary>(s_measureUri, MeasurementLevel.Binary, "Test", "[subject] does something");
        Assert.False(a.Equals("not a measure"));
    }

    [Fact]
    public void JsonRoundtrip_BehaviorFrequency_Polymorphic()
    {
        var uri = new Uri("https://schema-test.dseapi.app/testing/measure");

        Measure measure = new Measure<BehaviorFrequency>(uri, MeasurementLevel.Ordinal, "Test measure", "[subject] does something");

        var json = JsonSerializer.Serialize(measure, JsonSharedOptions.RelaxedJsonEscaping);

        var deserialized = JsonSerializer.Deserialize<Measure>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(deserialized);

        var measureDeserialized = Assert.IsType<Measure<BehaviorFrequency>>(deserialized);

        Assert.Equal(measure.Id, measureDeserialized.Id);
        Assert.Equal(measure.Uri, measureDeserialized.Uri);
        Assert.Equal(measure.MeasurementLevel, measureDeserialized.MeasurementLevel);
        Assert.Equal(measure.Name, measureDeserialized.Name);
        Assert.Equal(measure.Statement, measureDeserialized.Statement);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_Count()
    {
        AssertJson.Roundtrip(TestMeasures.CountMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_BinarySpeechSound()
    {
        AssertJson.Roundtrip(TestMeasures.BinarySpeechSoundMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_BinaryWord()
    {
        AssertJson.Roundtrip(TestMeasures.BinaryWordMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_BinarySentence()
    {
        AssertJson.Roundtrip(TestMeasures.BinarySentenceMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_BehaviorFrequencyMeasure()
    {
        AssertJson.Roundtrip(TestMeasures.BehaviorFrequencyMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_BehaviorFrequencySpeechSound()
    {
        AssertJson.Roundtrip(TestMeasures.BehaviorFrequencySpeechSoundMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_BehaviorFrequencyWord()
    {
        AssertJson.Roundtrip(TestMeasures.BehaviorFrequencyWordMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_BehaviorFrequencySentence()
    {
        AssertJson.Roundtrip(TestMeasures.BehaviorFrequencySentenceMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_SpeechClarity()
    {
        AssertJson.Roundtrip(TestMeasures.SpeechClarityMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_SpeechClaritySpeechSound()
    {
        AssertJson.Roundtrip(TestMeasures.SpeechClaritySpeechSoundMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_SpeechClarityWord()
    {
        AssertJson.Roundtrip(TestMeasures.SpeechClarityWordMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_SpeechClaritySentence()
    {
        AssertJson.Roundtrip(TestMeasures.SpeechClaritySentenceMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_Completeness()
    {
        AssertJson.Roundtrip(TestMeasures.CompletenessMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_CompletenessSpeechSound()
    {
        AssertJson.Roundtrip(TestMeasures.CompletenessSpeechSoundMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_CompletenessWord()
    {
        AssertJson.Roundtrip(TestMeasures.CompletenessWordMeasure, JsonContext.Default);
    }

    [Fact]
    public void JsonRoundtrip_WithContext_CompletenessSentence()
    {
        AssertJson.Roundtrip(TestMeasures.CompletenessSentenceMeasure, JsonContext.Default);
    }

    public static TheoryData<Measure, Type> PolymorphicCases { get; } = new()
    {
        { TestMeasures.BinaryMeasure, typeof(Measure<Binary>) },
        { TestMeasures.BinarySpeechSoundMeasure, typeof(Measure<Binary, SpeechSound>) },
        { TestMeasures.BinaryWordMeasure, typeof(Measure<Binary, WordId>) },
        { TestMeasures.BinarySentenceMeasure, typeof(Measure<Binary, SentenceId>) },
        { TestMeasures.BehaviorFrequencyMeasure, typeof(Measure<BehaviorFrequency>) },
        { TestMeasures.BehaviorFrequencySpeechSoundMeasure, typeof(Measure<BehaviorFrequency, SpeechSound>) },
        { TestMeasures.BehaviorFrequencyWordMeasure, typeof(Measure<BehaviorFrequency, WordId>) },
        { TestMeasures.BehaviorFrequencySentenceMeasure, typeof(Measure<BehaviorFrequency, SentenceId>) },
        { TestMeasures.CountMeasure, typeof(Measure<Count>) },
        { TestMeasures.AmountMeasure, typeof(Measure<Amount>) },
        { TestMeasures.SpeechClarityMeasure, typeof(Measure<SpeechClarity>) },
        { TestMeasures.SpeechClaritySpeechSoundMeasure, typeof(Measure<SpeechClarity, SpeechSound>) },
        { TestMeasures.SpeechClarityWordMeasure, typeof(Measure<SpeechClarity, WordId>) },
        { TestMeasures.SpeechClaritySentenceMeasure, typeof(Measure<SpeechClarity, SentenceId>) },
        { TestMeasures.CompletenessMeasure, typeof(Measure<Completeness>) },
        { TestMeasures.CompletenessSpeechSoundMeasure, typeof(Measure<Completeness, SpeechSound>) },
        { TestMeasures.CompletenessWordMeasure, typeof(Measure<Completeness, WordId>) },
        { TestMeasures.CompletenessSentenceMeasure, typeof(Measure<Completeness, SentenceId>) },
    };

    [Theory]
    [MemberData(nameof(PolymorphicCases))]
    public void JsonRoundtrip_Polymorphic_ResolvesBackToConcreteType(Measure measure, Type expected)
    {
        var json = JsonSerializer.Serialize(measure, JsonSharedOptions.RelaxedJsonEscaping);

        var deserialized = JsonSerializer.Deserialize<Measure>(json, JsonSharedOptions.RelaxedJsonEscaping);

        Assert.NotNull(deserialized);
        Assert.IsType(expected, deserialized);
        Assert.Equal(measure.Id, deserialized.Id);
        Assert.Equal(measure.Uri, deserialized.Uri);
        Assert.Equal(measure.MeasurementLevel, deserialized.MeasurementLevel);
        Assert.Equal(measure.Name, deserialized.Name);
        Assert.Equal(measure.Statement, deserialized.Statement);
    }
}
