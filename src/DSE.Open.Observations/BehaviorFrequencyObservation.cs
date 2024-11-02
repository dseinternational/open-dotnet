// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public record BehaviorFrequencyObservation : Observation<BehaviorFrequency>
{
    public static BehaviorFrequencyObservation Create(Measure measure, BehaviorFrequency value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static BehaviorFrequencyObservation Create(
        Measure measure,
        BehaviorFrequency value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BehaviorFrequencyObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value
        };
    }
}
