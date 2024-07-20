// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

/// <summary>
/// Records an observation at a point in time.
/// </summary>
/// <typeparam name="TObs"></typeparam>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "_t")]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<bool>>), typeDiscriminator: Schemas.BinaryObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<Count>>), typeDiscriminator: Schemas.CountObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<Amount>>), typeDiscriminator: Schemas.AmountObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<Ratio>>), typeDiscriminator: Schemas.RatioObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<BinaryWordObservation>), typeDiscriminator: Schemas.BinaryWordObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<BinarySpeechSoundObservation>), typeDiscriminator: Schemas.BinarySpeechSoundObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<int>>), typeDiscriminator: Schemas.IntegerObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<decimal>>), typeDiscriminator: Schemas.DecimalObservationSnapshot)]
public sealed record ObservationSnapshot<TObs>
    where TObs : IObservation
{
    public ObservationSnapshot()
    {
    }

    [SetsRequiredMembers]
    public ObservationSnapshot(DateTimeOffset time, TObs observation)
    {
        ObservationsValidator.EnsureMinimumObservationTime(time);
        Time = time;
        Observation = observation;
    }

    /// <summary>
    /// The time the snapshot was created.
    /// </summary>
    [JsonPropertyName("t")]
    public required DateTimeOffset Time { get; init; }

    /// <summary>
    /// The observation at the time the snapshot was created.
    /// </summary>
    [JsonPropertyName("o")]
    public required TObs Observation { get; init; }

    public int GetDiscriminatorCode()
    {
        return Observation.GetDiscriminatorCode();
    }
}

public static class ObservationSnapshot
{
    public static ObservationSnapshot<TObs> ForUtcNow<TObs>(TObs observation)
        where TObs : IObservation
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static ObservationSnapshot<TObs> ForUtcNow<TObs>(TObs observation, TimeProvider timeProvider)
        where TObs : IObservation
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new ObservationSnapshot<TObs>(timeProvider.GetUtcNow(), observation);
    }
}
