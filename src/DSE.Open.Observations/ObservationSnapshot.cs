// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// Records an observation at a point in time.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "_t")]
[JsonDerivedType(typeof(BinaryObservationSnapshot), typeDiscriminator: Schemas.BinaryObservationSnapshot)]
[JsonDerivedType(typeof(CountObservationSnapshot), typeDiscriminator: Schemas.CountObservationSnapshot)]
[JsonDerivedType(typeof(AmountObservationSnapshot), typeDiscriminator: Schemas.AmountObservationSnapshot)]
[JsonDerivedType(typeof(RatioObservationSnapshot), typeDiscriminator: Schemas.RatioObservationSnapshot)]
[JsonDerivedType(typeof(BinaryWordObservationSnapshot), typeDiscriminator: Schemas.BinaryWordObservationSnapshot)]
[JsonDerivedType(typeof(BinarySpeechSoundObservationSnapshot), typeDiscriminator: Schemas.BinarySpeechSoundObservationSnapshot)]
public abstract record ObservationSnapshot<TObs, TValue>
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
    protected ObservationSnapshot(DateTimeOffset time, TObs observation)
    {
        ObservationsValidator.EnsureMinimumObservationTime(time);
        Time = time;
        Observation = observation;
    }

    /// <summary>
    /// The time the snapshot was created.
    /// </summary>
    [JsonPropertyName("t")]
    public DateTimeOffset Time { get; private init; }

    /// <summary>
    /// The observation at the time the snapshot was created.
    /// </summary>
    [JsonPropertyName("o")]
    public TObs Observation { get; }

    /// <summary>
    /// Gets a code that discriminates between measurement types and is suitable for use as a hash code.
    /// </summary>
    public virtual int GetMeasurementCode()
    {
        return Observation.GetMeasurementCode();
    }
}
