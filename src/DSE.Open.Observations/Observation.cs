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
    public ulong Id { get; protected init; }

    /// <summary>
    /// The identifier for the measure.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("m")]
    [JsonPropertyOrder(-90000)]
    public uint MeasureId { get; protected init; }

    /// <summary>
    /// Gets a code that discriminates between measurement types.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// For 'simple' measures, this may simply be derived from the <see cref="MeasureId"/>. For types with additional
    /// identifying/discriminating state, additional values may need to be taken into account (for example, a
    /// <see cref="BinaryWordObservation"/> will derive a value from <see cref="MeasureId"/> and
    /// <see cref="BinaryWordObservation.WordId"/>.
    /// </remarks>
    public virtual int GetDiscriminatorCode()
    {
        return HashCode.Combine(MeasureId);
    }

    /// <summary>
    /// The time of the observation.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("t")]
    [JsonPropertyOrder(-89800)]
    [JsonConverter(typeof(DateTimeOffsetUnixTimeMillisecondsJsonConverter))]
    public DateTimeOffset Time { get; protected init; }

    /// <summary>
    /// Creates an <see cref="Observation{T}"/> using the System <see cref="TimeProvider"/>.
    /// </summary>
    /// <param name="measureId"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Observation<T> Create<T>(uint measureId, T value)
        where T : IEquatable<T>
    {
        return Create(measureId, value, TimeProvider.System);
    }

    /// <summary>
    /// Creates an <see cref="Observation{T}"/>
    /// </summary>
    /// <param name="measureId"></param>
    /// <param name="value"></param>
    /// <param name="timeProvider"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Observation<T> Create<T>(uint measureId, T value, TimeProvider timeProvider)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(timeProvider);

        return new Observation<T>
        {
            Id = RandomNumberHelper.GetJsonSafeInteger(),
            MeasureId = measureId,
            Value = value,
            Time = timeProvider.GetUtcNow()
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
