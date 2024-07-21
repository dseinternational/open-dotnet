// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record BinarySpeechSoundObservationSnapshot : ObservationSnapshot<BinarySpeechSoundObservation, bool>
{
    [JsonConstructor]
    internal BinarySpeechSoundObservationSnapshot(DateTimeOffset time, BinarySpeechSoundObservation observation) : base(time, observation)
    {
    }

    public static BinarySpeechSoundObservationSnapshot ForUtcNow(BinarySpeechSoundObservation observation)
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static BinarySpeechSoundObservationSnapshot ForUtcNow(BinarySpeechSoundObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new BinarySpeechSoundObservationSnapshot(timeProvider.GetUtcNow(), observation);
    }
}
