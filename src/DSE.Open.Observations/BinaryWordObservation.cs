// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed record BinaryWordObservation : Observation<bool>, IWordObservation
{
    [JsonPropertyName("w")]
    public required uint WordId { get; init; }

    public static BinaryWordObservation Create(uint measureId, uint wordId, bool value)
    {
        return new()
        {
            Id = RandomNumberHelper.GetJsonSafeInteger(),
            MeasureId = measureId,
            WordId = wordId,
            Value = value,
            Time = DateTimeOffset.UtcNow
        };
    }
}
