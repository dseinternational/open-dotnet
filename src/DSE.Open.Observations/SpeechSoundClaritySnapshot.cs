// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record SpeechSoundClaritySnapshot : Snapshot<SpeechSoundClarityObservation, SpeechClarity, SpeechSound>
{
    public static SpeechSoundClaritySnapshot ForUtcNow(SpeechSoundClarityObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SpeechSoundClaritySnapshot ForUtcNow(SpeechSoundClarityObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpeechSoundClaritySnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
