// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Observations;

public record CountObservation : Observation<Count>
{
    public static CountObservation Create(Measure measure, Count value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static CountObservation Create(
        Measure measure,
        Count value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new CountObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value
        };
    }
}
