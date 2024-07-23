// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record class BinarySpeechSoundMeasure : Measure<BinarySpeechSoundObservation, bool, SpeechSound>
{
    public BinarySpeechSoundMeasure(Uri uri, string name, string statement)
        : base(uri, MeasurementLevel.Binary, name, statement)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public BinarySpeechSoundMeasure(uint id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

#pragma warning disable CA1725 // Parameter names should match base declaration
    public override BinarySpeechSoundObservation CreateObservation(SpeechSound speechSound, bool value, DateTimeOffset timestamp)
#pragma warning restore CA1725 // Parameter names should match base declaration
    {
        return BinarySpeechSoundObservation.Create(this, speechSound, value, TimeProvider.System);
    }
}
