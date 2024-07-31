// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record BinarySnapshot : Snapshot<BinaryObservation, bool>
{
    [JsonConstructor]
    internal BinarySnapshot(DateTimeOffset time, BinaryObservation observation) : base(time, observation)
    {
    }

    public static BinarySnapshot ForUtcNow(BinaryObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinarySnapshot ForUtcNow(BinaryObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new BinarySnapshot(timeProvider.GetUtcNow(), observation);
    }
}
