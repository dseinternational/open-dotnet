// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.IO.Hashing;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public record SpokenWordClarityObservation : Observation<SpeechClarity, WordId>
{
    [JsonIgnore]
    public WordId WordId => Discriminator;

    public static SpokenWordClarityObservation Create(Measure measure, WordId wordId, SpeechClarity value)
    {
        return Create(measure, wordId, value, TimeProvider.System);
    }

    public static SpokenWordClarityObservation Create(
        Measure measure,
        WordId wordId,
        SpeechClarity value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SpokenWordClarityObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value,
            Discriminator = wordId
        };
    }

    protected override ulong GetDiscriminatorId()
    {
        return WordId.ToUInt64();
    }
}
