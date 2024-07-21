// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record CountObservationSnapshot : ObservationSnapshot<CountObservation, Count>
{
    [JsonConstructor]
    internal CountObservationSnapshot(DateTimeOffset time, CountObservation observation) : base(time, observation)
    {
    }

    public static CountObservationSnapshot ForUtcNow(CountObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static CountObservationSnapshot ForUtcNow(CountObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new CountObservationSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
