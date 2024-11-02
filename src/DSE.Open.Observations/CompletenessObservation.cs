// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public record CompletenessObservation : Observation<Completeness>
{
    public static CompletenessObservation Create(Measure measure, Completeness value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static CompletenessObservation Create(
        Measure measure,
        Completeness value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new CompletenessObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value
        };
    }
}
