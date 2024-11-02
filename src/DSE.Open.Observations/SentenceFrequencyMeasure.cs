﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Language;

namespace DSE.Open.Observations;

/// <summary>
/// A measure of observed frequency relating to a word.
/// </summary>
public sealed record SentenceFrequencyMeasure : Measure<SentenceFrequencyObservation, BehaviorFrequency, SentenceId>
{
    [SetsRequiredMembers]
    public SentenceFrequencyMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Binary;
        Name = name;
        Statement = statement;
    }

    public override SentenceFrequencyObservation CreateObservation(SentenceId sentenceId, BehaviorFrequency value, DateTimeOffset timestamp)
    {
        return SentenceFrequencyObservation.Create(this, sentenceId, value, TimeProvider.System);
    }
}
