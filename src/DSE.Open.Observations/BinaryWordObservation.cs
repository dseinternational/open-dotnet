// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Language;
using MessagePack;

namespace DSE.Open.Observations;

[MessagePackObject]
public record BinaryWordObservation : Observation<bool, WordId>
{
    protected BinaryWordObservation(Measure measure, WordId discriminator, DateTimeOffset time, bool value)
        : base(measure, discriminator, time, value)
    {
    }

    [JsonConstructor]
    [SerializationConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public BinaryWordObservation(ObservationId id, MeasureId measureId, long timestamp, bool value, WordId discriminator)
        : base(id, measureId, discriminator, timestamp, value)
    {
    }

    [IgnoreMember]
    [JsonIgnore]
    public WordId WordId => Discriminator;

    public static BinaryWordObservation Create(Measure measure, WordId wordId, bool value)
    {
        return Create(measure, wordId, value, TimeProvider.System);
    }

    public static BinaryWordObservation Create(
        Measure measure,
        WordId wordId,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinaryWordObservation(measure, wordId, timeProvider.GetUtcNow(), value);
    }

    protected override ulong GetDiscriminatorId()
    {
        return MeasureId | (ulong)WordId;
    }
}
