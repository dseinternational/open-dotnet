// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record SpeechClarityMeasure : Measure<SpeechClarityObservation, SpeechClarity>
{
    public SpeechClarityMeasure(MeasureId id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.GradedMembership, name, statement)
    {
    }

    [JsonConstructor]
    private SpeechClarityMeasure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.GradedMembership);
    }

    public override SpeechClarityObservation CreateObservation(SpeechClarity value, DateTimeOffset timestamp)
    {
        return SpeechClarityObservation.Create(this, value, TimeProvider.System);
    }
}
