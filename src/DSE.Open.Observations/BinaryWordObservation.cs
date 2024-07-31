// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public record BinaryWordObservation : Observation<bool, ulong>
{
    protected BinaryWordObservation(Measure measure, ulong discriminator, DateTimeOffset time, bool value)
        : base(measure, discriminator, time, value)
    {
    }

    [JsonConstructor]
    [Obsolete("For deserialization only", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected BinaryWordObservation(ulong id, ulong measureId, ulong discriminator, long timestamp, bool value)
        : base(id, measureId, discriminator, timestamp, value)
    {
    }

    [JsonIgnore]
    public ulong WordId => Discriminator;

    public static BinaryWordObservation Create(Measure measure, ulong wordId, bool value)
    {
        return Create(measure, wordId, value, TimeProvider.System);
    }

    public static BinaryWordObservation Create(
        Measure measure,
        ulong wordId,
        bool value,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinaryWordObservation(measure, wordId, timeProvider.GetUtcNow(), value);
    }

    protected override ulong GetDiscriminatorId()
    {
        return MeasureId | WordId;
    }
}
