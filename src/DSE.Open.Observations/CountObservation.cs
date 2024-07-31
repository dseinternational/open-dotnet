// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

public record CountObservation : Observation<Count>
{
    protected CountObservation(Measure measure, DateTimeOffset time, Count value)
        : base(measure, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected CountObservation(ObservationId id, MeasureId measureId, long timestamp, Count value)
        : base(id, measureId, timestamp, value)
    {
    }

    public static CountObservation Create(Measure measure, Count value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static CountObservation Create(
        Measure measure,
        Count value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new CountObservation(measure, timeProvider.GetUtcNow(), value);
    }
}
