// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record CountSnapshot : Snapshot<CountObservation, Count>
{
    public static CountSnapshot ForUtcNow(CountObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static CountSnapshot ForUtcNow(CountObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new CountSnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
