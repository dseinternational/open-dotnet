// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public sealed record BinaryWordMeasure : Measure<BinaryWordObservation, bool, WordId>
{
    [SetsRequiredMembers]
    public BinaryWordMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Binary;
        Name = name;
        Statement = statement;
    }

    public override BinaryWordObservation CreateObservation(WordId wordId, bool value, DateTimeOffset timestamp)
    {
        return BinaryWordObservation.Create(this, wordId, value, TimeProvider.System);
    }
}
