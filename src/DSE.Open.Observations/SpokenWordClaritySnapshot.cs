// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record SpokenWordClaritySnapshot : Snapshot<SpokenWordClarityObservation, SpeechClarity, WordId>
{
    public static SpokenWordClaritySnapshot ForUtcNow(SpokenWordClarityObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SpokenWordClaritySnapshot ForUtcNow(SpokenWordClarityObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpokenWordClaritySnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
