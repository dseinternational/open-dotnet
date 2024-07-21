// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Values;

namespace DSE.Open.Observations;

/// <summary>
/// Records an observation at a point in time.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "_t")]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<bool>>), typeDiscriminator: Schemas.BinaryObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<Count>>), typeDiscriminator: Schemas.CountObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<Amount>>), typeDiscriminator: Schemas.AmountObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<Ratio>>), typeDiscriminator: Schemas.RatioObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<BinaryWordObservation>), typeDiscriminator: Schemas.BinaryWordObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<BinarySpeechSoundObservation>), typeDiscriminator: Schemas.BinarySpeechSoundObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<int>>), typeDiscriminator: Schemas.IntegerObservationSnapshot)]
[JsonDerivedType(typeof(ObservationSnapshot<Observation<decimal>>), typeDiscriminator: Schemas.DecimalObservationSnapshot)]
public abstract record ObservationSnapshot
{
    protected ObservationSnapshot(DateTimeOffset time)
    {
        ObservationsValidator.EnsureMinimumObservationTime(time);
        Time = time;
    }

    /// <summary>
    /// The time the snapshot was created.
    /// </summary>
    [JsonPropertyName("t")]
    public DateTimeOffset Time { get; private init; }

    /// <summary>
    /// Gets a code that discriminates between measurement types and is suitable for use as a hash code.
    /// </summary>
    public abstract int GetMeasurementCode();

    public static ObservationSnapshot<TObs> ForUtcNow<TObs>(TObs observation)
        where TObs : Observation
    {
        return ForUtcNow(observation, TimeProvider.System);
    }

    public static ObservationSnapshot<TObs> ForUtcNow<TObs>(TObs observation, TimeProvider timeProvider)
        where TObs : Observation
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new ObservationSnapshot<TObs>(timeProvider.GetUtcNow(), observation);
    }
}

public record ObservationSnapshot<TObs> : ObservationSnapshot
    where TObs : Observation
{
    [SetsRequiredMembers]
    public ObservationSnapshot(DateTimeOffset time, TObs observation) : base(time)
    {
        Observation = observation;
    }

    /// <summary>
    /// The observation at the time the snapshot was created.
    /// </summary>
    [JsonPropertyName("o")]
    public required TObs Observation { get; init; }

    public override int GetMeasurementCode()
    {
        return Observation.GetMeasurementCode();
    }
}
