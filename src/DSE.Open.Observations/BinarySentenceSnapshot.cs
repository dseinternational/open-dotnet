// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record BinarySentenceSnapshot : Snapshot<BinarySentenceObservation, bool, SentenceId>
{
    public static BinarySentenceSnapshot ForUtcNow(BinarySentenceObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinarySentenceSnapshot ForUtcNow(BinarySentenceObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinarySentenceSnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
