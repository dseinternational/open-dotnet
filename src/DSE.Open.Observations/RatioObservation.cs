// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record RatioObservation : Observation<Ratio>
{
    protected RatioObservation(Measure measure, DateTimeOffset time, Ratio value)
        : base(measure, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected RatioObservation(ulong id, ulong measureId, long timestamp, Ratio value)
        : base(id, measureId, timestamp, value)
    {
    }

    public static RatioObservation Create(Measure measure, Ratio value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static RatioObservation Create(
        Measure measure,
        Ratio value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new RatioObservation(measure, timeProvider.GetUtcNow(), value);
    }
}
