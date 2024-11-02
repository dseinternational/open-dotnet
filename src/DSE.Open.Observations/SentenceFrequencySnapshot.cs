// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record SentenceFrequencySnapshot : Snapshot<SentenceFrequencyObservation, BehaviorFrequency, SentenceId>
{
    public static SentenceFrequencySnapshot ForUtcNow(SentenceFrequencyObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SentenceFrequencySnapshot ForUtcNow(SentenceFrequencyObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SentenceFrequencySnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
