// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public static class Schemas
{
    internal const string ObservationSets = "https://schema.dseapi.app/v1.0/observations/sets";

    public const string BinaryObservationSet = $"{ObservationSets}/binary";

    public const string CountObservationSet = $"{ObservationSets}/count";

    public const string AmountObservationSet = $"{ObservationSets}/amount";

    public const string RatioObservationSet = $"{ObservationSets}/ratio";

    public const string BinaryWordObservationSet = $"{ObservationSets}/binary/word";

    public const string BinarySpeechSoundObservationSet = $"{ObservationSets}/binary/speech-sound";

    public const string IntegerObservationSet = $"{ObservationSets}/int";

    public const string DecimalObservationSet = $"{ObservationSets}/decimal";
}
