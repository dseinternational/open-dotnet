// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

/// <summary>
/// A measure of observed frequency relating to a speech sound.
/// </summary>
public sealed record SpeechSoundFrequencyMeasure
    : Measure<SpeechSoundFrequencyObservation, BehaviorFrequency, SpeechSound>
{
    [SetsRequiredMembers]
    public SpeechSoundFrequencyMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Binary;
        Name = name;
        Statement = statement;
    }

    public override SpeechSoundFrequencyObservation CreateObservation(
        SpeechSound speechSound,
        BehaviorFrequency value,
        DateTimeOffset timestamp)
    {
        return SpeechSoundFrequencyObservation.Create(this, speechSound, value, TimeProvider.System);
    }
}
