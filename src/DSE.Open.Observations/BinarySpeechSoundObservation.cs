// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public record BinarySpeechSoundObservation : Observation<bool, SpeechSound>
{
    [JsonIgnore]
    public SpeechSound SpeechSound => Discriminator;

    public static BinarySpeechSoundObservation Create(Measure measure, SpeechSound speechSound, bool value)
    {
        return Create(measure, speechSound, value, TimeProvider.System);
    }

    public static BinarySpeechSoundObservation Create(
        Measure measure,
        SpeechSound speechSound,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinarySpeechSoundObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value,
            Discriminator = speechSound
        };
    }

    [SkipLocalsInit]
    protected override ulong GetDiscriminatorId()
    {
        Span<byte> buffer = stackalloc byte[SpeechSound.MaxLength];
        SpeechSound.TryFormat(buffer, out var written, default, default);
        return XxHash3.HashToUInt64(buffer[..written]);
    }
}
