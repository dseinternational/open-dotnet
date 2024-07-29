// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public static class TestMeasures
{
    public static readonly BinaryMeasure BinaryMeasure = new(
        RandomNumberHelper.GetJsonSafeInteger(),
        new Uri("https://schema-test.dseapi.app/testing/binary-measure"),
        "Test measure",
        "[subject] does something");

    public static readonly BinaryMeasure BinaryMeasure2 = new(
        RandomNumberHelper.GetJsonSafeInteger(),
        new Uri("https://schema-test.dseapi.app/testing/binary-measure-2"),
        "Test measure",
        "[subject] does something");

    public static readonly BinarySpeechSoundMeasure BinarySpeechSoundMeasure = new(
        RandomNumberHelper.GetJsonSafeInteger(),
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure"),
        "Test measure",
        "[subject] does something");

    public static readonly BinarySpeechSoundMeasure BinarySpeechSoundMeasure2 = new(
        RandomNumberHelper.GetJsonSafeInteger(),
        new Uri("https://schema-test.dseapi.app/testing/binary-speech-sound-measure-2"),
        "Test measure",
        "[subject] does something");

    public static readonly BinaryWordMeasure BinaryWordMeasure = new(
        RandomNumberHelper.GetJsonSafeInteger(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure"),
        "Test measure",
        "[subject] does something");

    public static readonly BinaryWordMeasure BinaryWordMeasure2 = new(
        RandomNumberHelper.GetJsonSafeInteger(),
        new Uri("https://schema-test.dseapi.app/testing/binary-word-measure-2"),
        "Test measure",
        "[subject] does something");

    public static readonly CountMeasure CountMeasure = new(
        RandomNumberHelper.GetJsonSafeInteger(),
        new Uri("https://schema-test.dseapi.app/testing/count-measure"),
        "Test measure",
        "[subject] does something");

    public static readonly CountMeasure CountMeasure2 = new(
        RandomNumberHelper.GetJsonSafeInteger(),
        new Uri("https://schema-test.dseapi.app/testing/count-measure-2"),
        "Test measure",
        "[subject] does something");

}