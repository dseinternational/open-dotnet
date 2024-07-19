// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record BinarySpeechSoundObservation : Observation<bool>, ISpeechSoundObservation
{
    [JsonPropertyName("s")]
    public required SpeechSound SpeechSound { get; init; }

    public override int GetDiscriminatorCode()
    {
        return HashCode.Combine(MeasureId, SpeechSound);
    }

    public static BinarySpeechSoundObservation Create(uint measureId, SpeechSound speechSound, bool value)
    {
        return Create(measureId, speechSound, value, TimeProvider.System);
    }

    public static BinarySpeechSoundObservation Create(
        uint measureId,
        SpeechSound speechSound,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinarySpeechSoundObservation
        {
            Id = RandomNumberHelper.GetJsonSafeInteger(),
            MeasureId = measureId,
            SpeechSound = speechSound,
            Value = value,
            Time = timeProvider.GetUtcNow()
        };
    }
}
