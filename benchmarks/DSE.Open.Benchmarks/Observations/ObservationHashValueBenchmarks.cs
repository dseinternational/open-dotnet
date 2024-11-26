// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using BenchmarkDotNet.Attributes;
using DSE.Open.Language;
using DSE.Open.Observations;
using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Benchmarks.Observations;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class ObservationHashValueBenchmarks
{
    private static readonly Observation[] s_observations =
    [
        Observation.Create(TestMeasures.BinaryMeasure, true),
        Observation.Create(TestMeasures.CountMeasure,(Count)42),
        Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ay.Abstraction, true),
        Observation.Create(TestMeasures.BinarySpeechSoundMeasure, Phonemes.English.ch.Abstraction, false),
    ];

    [Benchmark(Baseline = true)]
    public int[] UsingGetHashCode()
    {
        var result = new int[s_observations.Length];

        for (var i = 0; i < s_observations.Length; i++)
        {
            result[i] = s_observations[i].GetHashCode();
        }

        return result;
    }

    [Benchmark]
    public ulong[] UsingGetRepeatableHashCode()
    {
        var result = new ulong[s_observations.Length];

        for (var i = 0; i < s_observations.Length; i++)
        {
            result[i] = s_observations[i].GetRepeatableHashCode();
        }

        return result;
    }
}

public static class TestMeasures
{
    public static readonly Measure<Binary> BinaryMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Binary> BinaryMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Binary, SpeechSound> BinarySpeechSoundMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Binary, SpeechSound> BinarySpeechSoundMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Binary, WordId> BinaryWordMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Binary, WordId> BinaryWordMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Binary, SentenceId> BinarySentenceMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Binary, SentenceId> BinarySentenceMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Count> CountMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/count-measure"),
        MeasurementLevel.Ordinal,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Count> CountMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/count-measure-2"),
        MeasurementLevel.Ordinal,
        "Test measure",
        "[subject] does something");

}
