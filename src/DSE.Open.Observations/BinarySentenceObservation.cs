// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public record BinarySentenceObservation : Observation<bool, SentenceId>
{
    [JsonIgnore]
    public SentenceId SentenceId => Discriminator;

    public static BinarySentenceObservation Create(Measure measure, SentenceId sentenceId, bool value)
    {
        return Create(measure, sentenceId, value, TimeProvider.System);
    }

    public static BinarySentenceObservation Create(
        Measure measure,
        SentenceId sentenceId,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinarySentenceObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value,
            Discriminator = sentenceId
        };
    }

    protected override ulong GetDiscriminatorId()
    {
        return MeasureId | (ulong)SentenceId;
    }
}
