// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

/// <summary>
/// An observation of frequency related to a speech sound.
/// </summary>
public record SpeechSoundFrequencyObservation : Observation<BehaviorFrequency, SpeechSound>
{
    [JsonIgnore]
    public SpeechSound SpeechSound => Discriminator;

    public static SpeechSoundFrequencyObservation Create(Measure measure, SpeechSound speechSound, BehaviorFrequency value)
    {
        return Create(measure, speechSound, value, TimeProvider.System);
    }

    public static SpeechSoundFrequencyObservation Create(
        Measure measure,
        SpeechSound speechSound,
        BehaviorFrequency value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpeechSoundFrequencyObservation
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
