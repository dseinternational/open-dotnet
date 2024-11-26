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
