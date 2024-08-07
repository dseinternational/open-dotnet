// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.IO.Hashing;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public record SpokenWordClarityObservation : Observation<SpeechClarity, WordId>
{
    protected SpokenWordClarityObservation(Measure measure, WordId discriminator, DateTimeOffset time, SpeechClarity value)
        : base(measure, discriminator, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected SpokenWordClarityObservation(ObservationId id, MeasureId measureId, WordId discriminator, long timestamp, SpeechClarity value)
        : base(id, measureId, discriminator, timestamp, value)
    {
    }

    [JsonIgnore]
    public WordId WordId => Discriminator;

    public static SpokenWordClarityObservation Create(Measure measure, WordId speechSound, SpeechClarity value)
    {
        return Create(measure, speechSound, value, TimeProvider.System);
    }

    public static SpokenWordClarityObservation Create(
        Measure measure,
        WordId speechSound,
        SpeechClarity value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpokenWordClarityObservation(measure, speechSound, timeProvider.GetUtcNow(), value);
    }

    protected override ulong GetDiscriminatorId()
    {
        var chars = WordId.ToCharSequence().AsSpan();
        var c = Encoding.UTF8.GetByteCount(chars);
        Span<byte> b = stackalloc byte[c];
        _ = Encoding.UTF8.GetBytes(chars, b);
        return XxHash3.HashToUInt64(b);
    }
}
