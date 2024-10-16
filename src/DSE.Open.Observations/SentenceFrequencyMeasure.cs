// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

/// <summary>
/// A measure of observed frequency relating to a word.
/// </summary>
public sealed record SentenceFrequencyMeasure : Measure<SentenceFrequencyObservation, BehaviorFrequency, SentenceId>
{
    public SentenceFrequencyMeasure(MeasureId id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    internal SentenceFrequencyMeasure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.Binary);
    }

#pragma warning disable CA1725 // Parameter names should match base declaration
    public override SentenceFrequencyObservation CreateObservation(SentenceId speechSound, BehaviorFrequency value, DateTimeOffset timestamp)
#pragma warning restore CA1725 // Parameter names should match base declaration
    {
        return SentenceFrequencyObservation.Create(this, speechSound, value, TimeProvider.System);
    }
}
