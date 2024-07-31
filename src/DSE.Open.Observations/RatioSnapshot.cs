// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record RatioSnapshot : Snapshot<RatioObservation, Ratio>
{
    [JsonConstructor]
    internal RatioSnapshot(DateTimeOffset time, RatioObservation observation) : base(time, observation)
    {
    }

    public static RatioSnapshot ForUtcNow(RatioObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static RatioSnapshot ForUtcNow(RatioObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new RatioSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
