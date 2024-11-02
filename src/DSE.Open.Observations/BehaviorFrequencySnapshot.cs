// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public sealed record BehaviorFrequencySnapshot : Snapshot<BehaviorFrequencyObservation, BehaviorFrequency>
{
    public static BehaviorFrequencySnapshot ForUtcNow(BehaviorFrequencyObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BehaviorFrequencySnapshot ForUtcNow(
        BehaviorFrequencyObservation observation,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BehaviorFrequencySnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
