// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public record SpeechClarityObservation : Observation<SpeechClarity>
{
    protected SpeechClarityObservation(Measure measure, DateTimeOffset time, SpeechClarity value)
        : base(measure, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SpeechClarityObservation(ObservationId id, MeasureId measureId, long timestamp, SpeechClarity value)
        : base(id, measureId, timestamp, value)
    {
    }

    public static SpeechClarityObservation Create(Measure measure, SpeechClarity value)
    {
        return Create(measure, value, TimeProvider.System);
    }

    public static SpeechClarityObservation Create(
        Measure measure,
        SpeechClarity value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpeechClarityObservation(measure, timeProvider.GetUtcNow(), value);
    }
}
