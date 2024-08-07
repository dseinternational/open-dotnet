// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

/// <summary>
/// A measure of observed frequency relating to a word.
/// </summary>
public sealed record WordFrequencyMeasure : Measure<WordFrequencyObservation, BehaviorFrequency, WordId>
{
    public WordFrequencyMeasure(MeasureId id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    private WordFrequencyMeasure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.Binary);
    }

#pragma warning disable CA1725 // Parameter names should match base declaration
    public override WordFrequencyObservation CreateObservation(WordId speechSound, BehaviorFrequency value, DateTimeOffset timestamp)
#pragma warning restore CA1725 // Parameter names should match base declaration
    {
        return WordFrequencyObservation.Create(this, speechSound, value, TimeProvider.System);
    }
}
