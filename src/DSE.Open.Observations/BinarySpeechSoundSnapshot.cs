// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record BinarySpeechSoundSnapshot : Snapshot<BinarySpeechSoundObservation, bool, SpeechSound>
{
    public static BinarySpeechSoundSnapshot ForUtcNow(BinarySpeechSoundObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinarySpeechSoundSnapshot ForUtcNow(BinarySpeechSoundObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinarySpeechSoundSnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation,
        };
    }
}
