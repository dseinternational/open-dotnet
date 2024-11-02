// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public sealed record BinarySnapshot : Snapshot<BinaryObservation, bool>
{
    public static BinarySnapshot ForUtcNow(BinaryObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinarySnapshot ForUtcNow(BinaryObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinarySnapshot
        {
            Time = timeProvider.GetUtcNow(),
            Observation = observation
        };
    }
}
