// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public static class Schemas
{
    internal const string Observations = "https://schema.dseapi.app/v1.0/observations";
    internal const string ObservationSets = "https://schema.dseapi.app/v1.0/observations/sets";


    public const string BinaryObservation = $"{Observations}/binary";
    public const string BinaryObservationSet = $"{ObservationSets}/binary";

    public const string CountObservation = $"{Observations}/count";
    public const string CountObservationSet = $"{ObservationSets}/count";

    public const string AmountObservation = $"{Observations}/amount";
    public const string AmountObservationSet = $"{ObservationSets}/amount";

    public const string RatioObservation = $"{Observations}/ratio";
    public const string RatioObservationSet = $"{ObservationSets}/ratio";

    public const string BinaryWordObservation = $"{Observations}/binary/word";
    public const string BinaryWordObservationSet = $"{ObservationSets}/binary/word";

    public const string BinarySpeechSoundObservation = $"{Observations}/binary/speech-sound";
    public const string BinarySpeechSoundObservationSet = $"{ObservationSets}/binary/speech-sound";

    public const string IntegerObservation = $"{Observations}/int";
    public const string IntegerObservationSet = $"{ObservationSets}/int";

    public const string DecimalObservation = $"{Observations}/decimal";
    public const string DecimalObservationSet = $"{ObservationSets}/decimal";
}
