// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed class BinarySpeechSoundMeasure : Measure<BinarySpeechSoundObservation, bool, SpeechSound>
{
    public BinarySpeechSoundMeasure(ulong id, Uri uri, string name, string statement)
        : base(id, uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    private BinarySpeechSoundMeasure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(measurementLevel, MeasurementLevel.Binary);
    }

#pragma warning disable CA1725 // Parameter names should match base declaration
    public override BinarySpeechSoundObservation CreateObservation(SpeechSound speechSound, bool value, DateTimeOffset timestamp)
#pragma warning restore CA1725 // Parameter names should match base declaration
    {
        return BinarySpeechSoundObservation.Create(this, speechSound, value, TimeProvider.System);
    }
}
