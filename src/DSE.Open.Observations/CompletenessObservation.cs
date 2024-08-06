// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public record CompletenessObservation : Observation<Completeness>
{
    protected CompletenessObservation(Measure measure, DateTimeOffset time, Completeness value)
        : base(measure, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected CompletenessObservation(ObservationId id, MeasureId measureId, long timestamp, Completeness value)
        : base(id, measureId, timestamp, value)
    {
    }

    public static CompletenessObservation Create(Measure measure, Completeness value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static CompletenessObservation Create(
        Measure measure,
        Completeness value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new CompletenessObservation(measure, timeProvider.GetUtcNow(), value);
    }
}
