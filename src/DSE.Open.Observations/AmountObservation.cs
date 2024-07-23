// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record AmountObservation : Observation<Amount>
{
    public AmountObservation(Measure measure, DateTimeOffset time, Amount value)
        : base(measure, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal AmountObservation(ulong id, uint measureId, long timestamp, Amount value)
        : base(id, measureId, timestamp, value)
    {
    }

    public static AmountObservation Create(Measure measure, Amount value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static AmountObservation Create(
        Measure measure,
        Amount value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new AmountObservation(measure, timeProvider.GetUtcNow(), value);
    }
}
