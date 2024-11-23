// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;
using DSE.Open.Speech;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public static class TestMeasures
{
    public static readonly Measure<bool> BinaryMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<bool> BinaryMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<bool, SpeechSound> BinarySpeechSoundMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<bool, SpeechSound> BinarySpeechSoundMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<bool, WordId> BinaryWordMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<bool, WordId> BinaryWordMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<bool, SentenceId> BinarySentenceMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<bool, SentenceId> BinarySentenceMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure-2"),
        MeasurementLevel.Binary,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Count> CountMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/count-measure"),
        MeasurementLevel.Ordinal,
        "Test measure",
        "[subject] does something");

    public static readonly Measure<Count> CountMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/count-measure-2"),
        MeasurementLevel.Ordinal,
        "Test measure",
        "[subject] does something");

}
