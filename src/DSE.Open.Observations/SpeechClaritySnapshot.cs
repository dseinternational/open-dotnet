// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public sealed record SpeechClaritySnapshot : Snapshot<SpeechClarityObservation, SpeechClarity>
{
    public static SpeechClaritySnapshot ForUtcNow(SpeechClarityObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SpeechClaritySnapshot ForUtcNow(SpeechClarityObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpeechClaritySnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
