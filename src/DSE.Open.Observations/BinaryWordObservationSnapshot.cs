// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record BinaryWordObservationSnapshot : ObservationSnapshot<BinaryWordObservation, bool, WordId>
{
    [JsonConstructor]
    internal BinaryWordObservationSnapshot(DateTimeOffset time, BinaryWordObservation observation) : base(time, observation)
    {
    }

    public static BinaryWordObservationSnapshot ForUtcNow(BinaryWordObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinaryWordObservationSnapshot ForUtcNow(BinaryWordObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new BinaryWordObservationSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
