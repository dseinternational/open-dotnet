// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record AmountSnapshot : Snapshot<AmountObservation, Amount>
{
    [JsonConstructor]
    internal AmountSnapshot(DateTimeOffset time, AmountObservation observation) : base(time, observation)
    {
    }

    public static AmountSnapshot ForUtcNow(AmountObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static AmountSnapshot ForUtcNow(AmountObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new AmountSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
