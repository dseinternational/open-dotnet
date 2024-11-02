// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public record BinaryObservation : Observation<bool>
{
    public static BinaryObservation Create(Measure measure, bool value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static BinaryObservation Create(
        Measure measure,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinaryObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value
        };
    }
}
