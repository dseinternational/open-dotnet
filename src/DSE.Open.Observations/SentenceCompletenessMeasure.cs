// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record SentenceCompletenessMeasure : Measure<SentenceCompletenessObservation, Completeness, SentenceId>
{
    [SetsRequiredMembers]
    public SentenceCompletenessMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Binary;
        Name = name;
        Statement = statement;
    }

    public override SentenceCompletenessObservation CreateObservation(SentenceId wordId, Completeness value, DateTimeOffset timestamp)
    {
        return SentenceCompletenessObservation.Create(this, wordId, value, TimeProvider.System);
    }
}
