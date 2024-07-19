// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "_t")]
[JsonDerivedType(typeof(Observation<bool>), typeDiscriminator: Schemas.BinaryObservation)]
[JsonDerivedType(typeof(Observation<Count>), typeDiscriminator: Schemas.CountObservation)]
[JsonDerivedType(typeof(Observation<Amount>), typeDiscriminator: Schemas.AmountObservation)]
[JsonDerivedType(typeof(Observation<Ratio>), typeDiscriminator: Schemas.RatioObservation)]
[JsonDerivedType(typeof(BinaryWordObservation), typeDiscriminator: Schemas.BinaryWordObservation)]
[JsonDerivedType(typeof(BinarySpeechSoundObservation), typeDiscriminator: Schemas.BinarySpeechSoundObservation)]
[JsonDerivedType(typeof(Observation<int>), typeDiscriminator: Schemas.IntegerObservation)]
[JsonDerivedType(typeof(Observation<decimal>), typeDiscriminator: Schemas.DecimalObservation)]
public abstract record Observation
{
    /// <summary>
    /// A randomly generated number between 0 and <see cref="RandomNumberHelper.MaxJsonSafeInteger"/> that,
    /// together with the timestamp, uniquely identifies this observation.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("i")]
    [JsonPropertyOrder(-91000)]
    public ulong Id { get; private init; }

    /// <summary>
    /// The identifier for the measure.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("m")]
    [JsonPropertyOrder(-90000)]
    public uint MeasureId { get; private init; }

    /// <summary>
    /// The time of the observation.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("t")]
    [JsonPropertyOrder(-89800)]
    [JsonConverter(typeof(DateTimeOffsetUnixTimeMillisecondsJsonConverter))]
    public DateTimeOffset Time { get; private init; }

    public static Observation<T> Create<T>(uint measureId, T value)
        where T : IEquatable<T>
    {
        return new()
        {
            Id = RandomNumberHelper.GetJsonSafeInteger(),
            MeasureId = measureId,
            Value = value,
            Time = DateTimeOffset.UtcNow
        };
    }
}

public record Observation<T> : Observation, IObservation<T>
    where T : IEquatable<T>
{
    [JsonPropertyName("v")]
    [JsonPropertyOrder(-1)]
    public required T Value { get; init; }

    object IObservation.Value => Value!;
}
