// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public static class TestMeasures
{
    public static readonly Measure<Binary> BinaryMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something",
        10000);

    public static readonly Measure<Binary> BinaryMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something",
        120000);

    public static readonly Measure<Binary, SpeechSound> BinarySpeechSoundMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something",
        1000000);

    public static readonly Measure<Binary, SpeechSound> BinarySpeechSoundMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something",
        150000);

    public static readonly Measure<Binary, WordId> BinaryWordMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something",
        100005050);

    public static readonly Measure<Binary, WordId> BinaryWordMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something",
        50610010);

    public static readonly Measure<Binary, SentenceId> BinarySentenceMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something",
        6081850);

    public static readonly Measure<Binary, SentenceId> BinarySentenceMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something",
        24100);

    public static readonly Measure<Count> CountMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/count-measure"),
        MeasurementLevel.Ordinal,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Count> CountMeasure2 = new(
        new Uri("https://schema-test.dseapi.app/testing/count-measure-2"),
        MeasurementLevel.Ordinal,
        "Test measure",
        "[subject] does something",
        40500);

    public static readonly Measure<Amount> AmountMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/amount-measure"),
        MeasurementLevel.Interval,
        "Test measure",
        "[subject] does something",
        80500);

    public static readonly Measure<BehaviorFrequency, SpeechSound> BehaviorFrequencySpeechSoundMeasure = new(
        new Uri("https://schema-test.dseapi.app/testing/frequency-speech-sound-measure"),
        MeasurementLevel.Ordinal,
        "Test measure",
        "[subject] does something",
        160900);
}
