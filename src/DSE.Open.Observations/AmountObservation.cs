// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;

namespace DSE.Open.Observations;

public record AmountObservation : Observation<Amount>
{
    public static AmountObservation Create(Measure measure, Amount value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static AmountObservation Create(
        Measure measure,
        Amount value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new AmountObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value
        };
    }
}
