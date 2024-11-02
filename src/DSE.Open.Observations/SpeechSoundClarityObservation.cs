// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public record SpeechSoundClarityObservation : Observation<SpeechClarity, SpeechSound>
{
    [JsonIgnore]
    public SpeechSound SpeechSound => Discriminator;

    public static SpeechSoundClarityObservation Create(Measure measure, SpeechSound speechSound, SpeechClarity value)
    {
        return Create(measure, speechSound, value, TimeProvider.System);
    }

    public static SpeechSoundClarityObservation Create(
        Measure measure,
        SpeechSound speechSound,
        SpeechClarity value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpeechSoundClarityObservation
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
        var chars = SpeechSound.ToCharSequence().AsSpan();
        var c = Encoding.UTF8.GetByteCount(chars);
        Span<byte> b = stackalloc byte[c];
        _ = Encoding.UTF8.GetBytes(chars, b);
        return XxHash3.HashToUInt64(b);
    }
}
