// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record WordFrequencySnapshot : Snapshot<WordFrequencyObservation, BehaviorFrequency, WordId>
{
    public static WordFrequencySnapshot ForUtcNow(WordFrequencyObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static WordFrequencySnapshot ForUtcNow(WordFrequencyObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new WordFrequencySnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
