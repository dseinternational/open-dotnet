// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record BinarySpeechSoundMeasure : Measure<BinarySpeechSoundObservation, bool, SpeechSound>
{
    [SetsRequiredMembers]
    public BinarySpeechSoundMeasure(MeasureId id, Uri uri, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = MeasurementLevel.Binary;
        Name = name;
        Statement = statement;
    }

    public override BinarySpeechSoundObservation CreateObservation(SpeechSound speechSound, bool value, DateTimeOffset timestamp)
    {
        return BinarySpeechSoundObservation.Create(this, speechSound, value, TimeProvider.System);
    }
}
