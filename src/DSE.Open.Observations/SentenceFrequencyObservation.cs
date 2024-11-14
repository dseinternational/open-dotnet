// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

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
    [JsonIgnore]
    public SentenceId SentenceId => Discriminator;

    public static SentenceFrequencyObservation Create(Measure measure, SentenceId sentenceId, BehaviorFrequency value)
    {
        return Create(measure, sentenceId, value, TimeProvider.System);
    }

    public static SentenceFrequencyObservation Create(
        Measure measure,
        SentenceId sentenceId,
        BehaviorFrequency value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SentenceFrequencyObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value,
            Discriminator = sentenceId
        };
    }

    [SkipLocalsInit]
    protected override ulong GetDiscriminatorId()
    {
        var chars = SentenceId.ToString();
        var c = Encoding.UTF8.GetByteCount(chars);
        Span<byte> b = stackalloc byte[c];
        _ = Encoding.UTF8.GetBytes(chars, b);
        return XxHash3.HashToUInt64(b);
    }
}
