// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record CountSnapshot : Snapshot<CountObservation, Count>
{
    [JsonConstructor]
    internal CountSnapshot(DateTimeOffset time, CountObservation observation) : base(time, observation)
    {
    }

    public static CountSnapshot ForUtcNow(CountObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static CountSnapshot ForUtcNow(CountObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new CountSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
