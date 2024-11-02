// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Language;

namespace DSE.Open.Observations;

public record BinaryWordObservation : Observation<bool, WordId>
{
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
        ArgumentNullException.ThrowIfNull(measure);
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinaryWordObservation
        {
            Time = timeProvider.GetUtcNow(),
            MeasureId = measure.Id,
            Value = value,
            Discriminator = wordId
        };
    }

    protected override ulong GetDiscriminatorId()
    {
        return MeasureId | (ulong)WordId;
    }
}
