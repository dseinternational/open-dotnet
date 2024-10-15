// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public record BehaviorFrequencyObservation : Observation<BehaviorFrequency>
{
    protected BehaviorFrequencyObservation(Measure measure, DateTimeOffset time, BehaviorFrequency value)
        : base(measure, time, value)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal BehaviorFrequencyObservation(ObservationId id, MeasureId measureId, long timestamp, BehaviorFrequency value)
        : base(id, measureId, timestamp, value)
    {
    }

    public static BehaviorFrequencyObservation Create(Measure measure, BehaviorFrequency value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static BehaviorFrequencyObservation Create(
        Measure measure,
        BehaviorFrequency value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BehaviorFrequencyObservation(measure, timeProvider.GetUtcNow(), value);
    }
}
