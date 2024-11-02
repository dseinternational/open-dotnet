// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

/// <summary>
/// An observation of frequency related to a word.
/// </summary>
public record WordFrequencyObservation : Observation<BehaviorFrequency, WordId>
{
    [JsonIgnore]
    public WordId WordId => Discriminator;

    public static WordFrequencyObservation Create(Measure measure, WordId wordId, BehaviorFrequency value)
    {
        return Create(measure, wordId, value, TimeProvider.System);
    }

    public static WordFrequencyObservation Create(
        Measure measure,
        WordId wordId,
        BehaviorFrequency value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new WordFrequencyObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value,
            Discriminator = wordId
        };
    }

    [SkipLocalsInit]
    protected override ulong GetDiscriminatorId()
    {
        var chars = WordId.ToCharSequence().AsSpan();
        var c = Encoding.UTF8.GetByteCount(chars);
        Span<byte> b = stackalloc byte[c];
        _ = Encoding.UTF8.GetBytes(chars, b);
        return XxHash3.HashToUInt64(b);
    }
}
