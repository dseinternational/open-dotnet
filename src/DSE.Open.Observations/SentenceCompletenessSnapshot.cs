// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record SentenceCompletenessSnapshot : Snapshot<SentenceCompletenessObservation, Completeness, SentenceId>
{
    public static SentenceCompletenessSnapshot ForUtcNow(SentenceCompletenessObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SentenceCompletenessSnapshot ForUtcNow(SentenceCompletenessObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SentenceCompletenessSnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
