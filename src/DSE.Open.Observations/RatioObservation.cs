// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public sealed record RatioObservation : Observation<Ratio>
{
    internal RatioObservation(uint measureId, DateTimeOffset time, Ratio value)
        : base(measureId, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal RatioObservation(ulong id, uint measureId, DateTimeOffset time, Ratio value)
        : base(id, measureId, time, value)
    {
    }

    public static RatioObservation Create(uint measureId, Ratio value)
    {
        return Create(measureId, value, TimeProvider.System);
    }

    public static RatioObservation Create(
        uint measureId,
        Ratio value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new RatioObservation(measureId, timeProvider.GetUtcNow(), value);
    }
}
