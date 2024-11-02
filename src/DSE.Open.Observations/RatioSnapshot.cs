// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record RatioSnapshot : Snapshot<RatioObservation, Ratio>
{
    public static RatioSnapshot ForUtcNow(RatioObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static RatioSnapshot ForUtcNow(RatioObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new RatioSnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
