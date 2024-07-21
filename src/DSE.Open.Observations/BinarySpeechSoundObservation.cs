// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public sealed record BinarySpeechSoundObservation : Observation<bool, SpeechSound>
{
    internal BinarySpeechSoundObservation(uint measureId, SpeechSound discriminator, DateTimeOffset time, bool value)
        : base(measureId, discriminator, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal BinarySpeechSoundObservation(ulong id, uint measureId, SpeechSound discriminator, DateTimeOffset time, bool value)
        : base(id, measureId, discriminator, time, value)
    {
    }

    [JsonIgnore]
    public SpeechSound SpeechSound => Discriminator;

    public static BinarySpeechSoundObservation Create(uint measureId, SpeechSound speechSound, bool value)
    {
        return Create(measureId, speechSound, value, TimeProvider.System);
    }

    public static BinarySpeechSoundObservation Create(
        uint measureId,
        SpeechSound speechSound,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinarySpeechSoundObservation(measureId, speechSound, timeProvider.GetUtcNow(), value);
    }
}
