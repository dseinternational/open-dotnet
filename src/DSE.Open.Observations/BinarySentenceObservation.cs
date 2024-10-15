// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public record BinarySentenceObservation : Observation<bool, SentenceId>
{
    protected BinarySentenceObservation(Measure measure, SentenceId discriminator, DateTimeOffset time, bool value)
        : base(measure, discriminator, time, value)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal BinarySentenceObservation(ObservationId id, MeasureId measureId, SentenceId discriminator, long timestamp, bool value)
        : base(id, measureId, discriminator, timestamp, value)
    {
    }

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
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinarySentenceObservation(measure, sentenceId, timeProvider.GetUtcNow(), value);
    }

    protected override ulong GetDiscriminatorId()
    {
        return MeasureId | (ulong)SentenceId;
    }
}
