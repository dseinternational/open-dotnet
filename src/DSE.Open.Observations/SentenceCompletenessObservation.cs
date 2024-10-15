// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public record SentenceCompletenessObservation : Observation<Completeness, SentenceId>
{
    protected SentenceCompletenessObservation(Measure measure, SentenceId discriminator, DateTimeOffset time, Completeness value)
        : base(measure, discriminator, time, value)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal SentenceCompletenessObservation(ObservationId id, MeasureId measureId, SentenceId discriminator, long timestamp, Completeness value)
        : base(id, measureId, discriminator, timestamp, value)
    {
    }

    [JsonIgnore]
    public SentenceId SentenceId => Discriminator;

    public static SentenceCompletenessObservation Create(Measure measure, SentenceId sentenceId, Completeness value)
    {
        return Create(measure, sentenceId, value, TimeProvider.System);
    }

    public static SentenceCompletenessObservation Create(
        Measure measure,
        SentenceId sentenceId,
        Completeness value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new SentenceCompletenessObservation(measure, sentenceId, timeProvider.GetUtcNow(), value);
    }

    protected override ulong GetDiscriminatorId()
    {
        return MeasureId | (ulong)SentenceId;
    }
}
