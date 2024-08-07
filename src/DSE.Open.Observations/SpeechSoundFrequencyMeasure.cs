// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

/// <summary>
/// A measure of observed frequency relating to a speech sound.
/// </summary>
public sealed record SpeechSoundFrequencyMeasure : Measure<SpeechSoundFrequencyObservation, BehaviorFrequency, SpeechSound>
{
    public SpeechSoundFrequencyMeasure(MeasureId id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    private SpeechSoundFrequencyMeasure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.Binary);
    }

#pragma warning disable CA1725 // Parameter names should match base declaration
    public override SpeechSoundFrequencyObservation CreateObservation(SpeechSound speechSound, BehaviorFrequency value, DateTimeOffset timestamp)
#pragma warning restore CA1725 // Parameter names should match base declaration
    {
        return SpeechSoundFrequencyObservation.Create(this, speechSound, value, TimeProvider.System);
    }
}
