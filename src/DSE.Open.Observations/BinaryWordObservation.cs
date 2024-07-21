// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record BinaryWordObservation : Observation<bool>
{
    [JsonPropertyName("w")]
    public required uint WordId { get; init; }

    public override int GetDiscriminatorCode()
    {
        return HashCode.Combine(MeasureId, WordId);
    }

    public static BinaryWordObservation Create(uint measureId, uint wordId, bool value)
    {
        return Create(measureId, wordId, value, TimeProvider.System);
    }

    public static BinaryWordObservation Create(uint measureId, uint wordId, bool value, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new BinaryWordObservation
        {
            Id = RandomNumberHelper.GetJsonSafeInteger(),
            MeasureId = measureId,
            WordId = wordId,
            Value = value,
            Time = timeProvider.GetUtcNow()
        };
    }
}
