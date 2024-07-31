// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record BinarySpeechSoundSnapshot : Snapshot<BinarySpeechSoundObservation, bool, SpeechSound>
{
    [JsonConstructor]
    internal BinarySpeechSoundSnapshot(DateTimeOffset time, BinarySpeechSoundObservation observation) : base(time, observation)
    {
    }

    public static BinarySpeechSoundSnapshot ForUtcNow(BinarySpeechSoundObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinarySpeechSoundSnapshot ForUtcNow(BinarySpeechSoundObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new BinarySpeechSoundSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
