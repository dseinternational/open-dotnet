// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public static class TestMeasures
{
    public static readonly BinaryMeasure BinaryMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-measure"),
        "Test measure",
        "[subject] does something");

    public static readonly BinaryMeasure BinaryMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-measure-2"),
        "Test measure",
        "[subject] does something");

    public static readonly BinarySpeechSoundMeasure BinarySpeechSoundMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure"),
        "Test measure",
        "[subject] does something");

    public static readonly BinarySpeechSoundMeasure BinarySpeechSoundMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure-2"),
        "Test measure",
        "[subject] does something");

    public static readonly BinaryWordMeasure BinaryWordMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
        "Test measure",
        "[subject] does something");

    public static readonly BinaryWordMeasure BinaryWordMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure-2"),
        "Test measure",
        "[subject] does something");

    public static readonly BinarySentenceMeasure BinarySentenceMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
        "Test measure",
        "[subject] does something");

    public static readonly BinarySentenceMeasure BinarySentenceMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure-2"),
        "Test measure",
        "[subject] does something");

    public static readonly CountMeasure CountMeasure = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/count-measure"),
        "Test measure",
        "[subject] does something");

    public static readonly CountMeasure CountMeasure2 = new(
        MeasureId.GetRandomId(),
        new Uri("https://schema-test.dseapi.app/testing/count-measure-2"),
        "Test measure",
        "[subject] does something");

}
