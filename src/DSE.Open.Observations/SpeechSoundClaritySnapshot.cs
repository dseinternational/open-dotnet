// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record SpeechSoundClaritySnapshot : Snapshot<SpeechSoundClarityObservation, SpeechClarity, SpeechSound>
{
    [JsonConstructor]
    internal SpeechSoundClaritySnapshot(DateTimeOffset time, SpeechSoundClarityObservation observation) : base(time, observation)
    {
    }

    public static SpeechSoundClaritySnapshot ForUtcNow(SpeechSoundClarityObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SpeechSoundClaritySnapshot ForUtcNow(SpeechSoundClarityObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new SpeechSoundClaritySnapshot(timeProvider.GetUtcNow(), observation);
    }
}
