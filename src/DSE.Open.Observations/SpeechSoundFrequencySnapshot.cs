// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record SpeechSoundFrequencySnapshot : Snapshot<SpeechSoundFrequencyObservation, BehaviorFrequency, SpeechSound>
{
    [JsonConstructor]
    internal SpeechSoundFrequencySnapshot(DateTimeOffset time, SpeechSoundFrequencyObservation observation) : base(time, observation)
    {
    }

    public static SpeechSoundFrequencySnapshot ForUtcNow(SpeechSoundFrequencyObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static SpeechSoundFrequencySnapshot ForUtcNow(SpeechSoundFrequencyObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new SpeechSoundFrequencySnapshot(timeProvider.GetUtcNow(), observation);
    }
}
