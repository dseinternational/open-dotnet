// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record BinarySentenceObservationSnapshot : ObservationSnapshot<BinarySentenceObservation, bool, SentenceId>
{
    [JsonConstructor]
    internal BinarySentenceObservationSnapshot(DateTimeOffset time, BinarySentenceObservation observation) : base(time, observation)
    {
    }

    public static BinarySentenceObservationSnapshot ForUtcNow(BinarySentenceObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinarySentenceObservationSnapshot ForUtcNow(BinarySentenceObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new BinarySentenceObservationSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
