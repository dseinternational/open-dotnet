// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

/// <summary>
/// An observation of frequency related to a sentence.
/// </summary>
public record SentenceFrequencyObservation : Observation<BehaviorFrequency, SentenceId>
{
    protected SentenceFrequencyObservation(Measure measure, SentenceId discriminator, DateTimeOffset time, BehaviorFrequency value)
        : base(measure, discriminator, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SentenceFrequencyObservation(ObservationId id, MeasureId measureId, SentenceId discriminator, long timestamp, BehaviorFrequency value)
        : base(id, measureId, discriminator, timestamp, value)
    {
    }

    [JsonIgnore]
    public SentenceId SentenceId => Discriminator;

    public static SentenceFrequencyObservation Create(Measure measure, SentenceId speechSound, BehaviorFrequency value)
    {
        return Create(measure, speechSound, value, TimeProvider.System);
    }

    public static SentenceFrequencyObservation Create(
        Measure measure,
        SentenceId speechSound,
        BehaviorFrequency value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SentenceFrequencyObservation(measure, speechSound, timeProvider.GetUtcNow(), value);
    }

    [SkipLocalsInit]
    protected override ulong GetDiscriminatorId()
    {
        var chars = SentenceId.ToCharSequence().AsSpan();
        var c = Encoding.UTF8.GetByteCount(chars);
        Span<byte> b = stackalloc byte[c];
        _ = Encoding.UTF8.GetBytes(chars, b);
        return XxHash3.HashToUInt64(b);
    }
}
