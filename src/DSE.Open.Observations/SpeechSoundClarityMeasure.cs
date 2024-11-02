// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record SpeechSoundClarityMeasure : Measure<SpeechSoundClarityObservation, SpeechClarity, SpeechSound>
{
    [SetsRequiredMembers]
    public SpeechSoundClarityMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Binary;
        Name = name;
        Statement = statement;
    }

    public override SpeechSoundClarityObservation CreateObservation(
        SpeechSound speechSound,
        SpeechClarity value,
        DateTimeOffset timestamp)
    {
        return SpeechSoundClarityObservation.Create(this, speechSound, value, TimeProvider.System);
    }
}
