// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record BinaryObservationSnapshot : ObservationSnapshot<BinaryObservation, bool>
{
    [JsonConstructor]
    internal BinaryObservationSnapshot(DateTimeOffset time, BinaryObservation observation) : base(time, observation)
    {
    }

    public static BinaryObservationSnapshot ForUtcNow(BinaryObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinaryObservationSnapshot ForUtcNow(BinaryObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new BinaryObservationSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
