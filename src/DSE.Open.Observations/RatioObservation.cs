// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Observations;

public record RatioObservation : Observation<Ratio>
{
    public static RatioObservation Create(Measure measure, Ratio value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static RatioObservation Create(
        Measure measure,
        Ratio value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new RatioObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value
        };
    }
}
