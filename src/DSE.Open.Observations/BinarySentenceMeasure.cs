// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record BinarySentenceMeasure : Measure<BinarySentenceObservation, bool, SentenceId>
{
    [SetsRequiredMembers]
    public BinarySentenceMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Binary;
        Name = name;
        Statement = statement;
    }

    public override BinarySentenceObservation CreateObservation(SentenceId sentenceId, bool value, DateTimeOffset timestamp)
    {
        return BinarySentenceObservation.Create(this, sentenceId, value, TimeProvider.System);
    }
}
