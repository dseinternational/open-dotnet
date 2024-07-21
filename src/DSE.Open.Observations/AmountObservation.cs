// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record AmountObservation : Observation<Amount>
{
    public AmountObservation(uint measureId, DateTimeOffset time, Amount value)
        : base(measureId, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal AmountObservation(ulong id, uint measureId, DateTimeOffset time, Amount value)
        : base(id, measureId, time, value)
    {
    }

    public static AmountObservation Create(uint measureId, Amount value)
    {
        return Create(measureId, value, TimeProvider.System);
    }

    public static AmountObservation Create(
        uint measureId,
        Amount value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new AmountObservation(measureId, timeProvider.GetUtcNow(), value);
    }
}
