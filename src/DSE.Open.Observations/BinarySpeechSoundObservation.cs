// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record BinarySpeechSoundObservation : Observation<bool>, ISpeechSoundObservation
{
    [JsonPropertyName("s")]
    public required SpeechSound SpeechSound { get; init; }
}
