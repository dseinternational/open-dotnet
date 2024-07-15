// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

internal static class Schemas
{
    public const string Measures = "https://schema.dseapi.app/v1.0/records/measures";


    public const string BinaryObservation = $"{Measures}/binary";
    public const string BinaryWordObservation = $"{Measures}/binary/word";
    public const string BinarySpeechSoundObservation = $"{Measures}/binary/speech-sound";
}
