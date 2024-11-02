// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace DSE.Open.Observations;

public sealed record SpeechClarityMeasure : Measure<SpeechClarityObservation, SpeechClarity>
{
    [SetsRequiredMembers]
    public SpeechClarityMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.GradedMembership;
        Name = name;
        Statement = statement;
    }

    public override SpeechClarityObservation CreateObservation(SpeechClarity speechClarity, DateTimeOffset timestamp)
    {
        return SpeechClarityObservation.Create(this, speechClarity, TimeProvider.System);
    }
}
