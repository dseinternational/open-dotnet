// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record SpeechSoundFrequencySnapshot : Snapshot<SpeechSoundFrequencyObservation, BehaviorFrequency, SpeechSound>
{
    public static SpeechSoundFrequencySnapshot ForUtcNow(SpeechSoundFrequencyObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SpeechSoundFrequencySnapshot ForUtcNow(SpeechSoundFrequencyObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpeechSoundFrequencySnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
