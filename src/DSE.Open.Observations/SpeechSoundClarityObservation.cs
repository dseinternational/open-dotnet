// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Speech;

namespace DSE.Open.Observations;

public record SpeechSoundClarityObservation : Observation<SpeechClarity, SpeechSound>
{
    protected SpeechSoundClarityObservation(Measure measure, SpeechSound discriminator, DateTimeOffset time, SpeechClarity value)
        : base(measure, discriminator, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SpeechSoundClarityObservation(ObservationId id, MeasureId measureId, SpeechSound discriminator, long timestamp, SpeechClarity value)
        : base(id, measureId, discriminator, timestamp, value)
    {
    }

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
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpeechSoundClarityObservation(measure, speechSound, timeProvider.GetUtcNow(), value);
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
