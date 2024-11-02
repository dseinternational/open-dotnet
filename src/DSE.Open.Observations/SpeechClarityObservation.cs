// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Observations;

public record SpeechClarityObservation : Observation<SpeechClarity>
{
    public static SpeechClarityObservation Create(Measure measure, SpeechClarity value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static SpeechClarityObservation Create(
        Measure measure,
        SpeechClarity value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpeechClarityObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value
        };
    }
}
