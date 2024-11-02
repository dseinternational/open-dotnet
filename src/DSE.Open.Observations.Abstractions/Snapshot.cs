// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Serialization.DataTransfer;
using DSE.Open.Text.Json.Serialization;

#pragma warning disable CA1005 // Avoid excessive parameters on generic types

namespace DSE.Open.Observations;

/// <summary>
/// Records an observation at a point in time.
/// </summary>
public abstract record Snapshot : ImmutableDataTransferObject
{
    private readonly DateTimeOffset _time;

    /// <summary>
    /// The time the snapshot was created.
    /// </summary>
    [JsonPropertyName("t")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public required DateTimeOffset Time
    {
        get => _time;
        init
        {
            ObservationsValidator.EnsureMinimumObservationTime(value);
            _time = DateTimeOffset.FromUnixTimeMilliseconds(value.ToUnixTimeMilliseconds());
        }
    }

    public bool HasMeasure(Measure measure)
    {
        ArgumentNullException.ThrowIfNull(measure);
        return HasMeasureId(measure.Id);
    }

    public abstract bool HasMeasureId(MeasureId measureId);

    /// <summary>
    /// Gets a code that discriminates between measurement types and is suitable for use as a hash code.
    /// </summary>
    public abstract int GetMeasurementHashCode();
}

/// <summary>
/// Records an observation at a point in time.
/// </summary>
public abstract record Snapshot<TObs> : Snapshot
    where TObs : Observation
{
    /// <summary>
    /// The observation at the time the snapshot was created.
    /// </summary>
    [JsonPropertyName("o")]
    public required TObs Observation { get; init; }

    public override bool HasMeasureId(MeasureId measureId)
    {
        return Observation.HasMeasureId(measureId);
    }

    /// <summary>
    /// Gets a code that discriminates between measurement types and is suitable for use as a hash code.
    /// </summary>
    public override int GetMeasurementHashCode()
    {
        return Observation.GetMeasurementHashCode();
    }
}

/// <summary>
/// Records an observation at a point in time.
/// </summary>
public abstract record Snapshot<TObs, TValue> : Snapshot<TObs>
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
}

/// <summary>
/// Records an observation at a point in time.
/// </summary>
public abstract record Snapshot<TObs, TValue, TDisc> : Snapshot<TObs, TValue>
    where TObs : Observation<TValue, TDisc>
    where TValue : IEquatable<TValue>
    where TDisc : IEquatable<TDisc>
{
    public virtual bool HasMeasurement(Measure measure, TDisc discriminator)
    {
        ArgumentNullException.ThrowIfNull(measure);
        return HasMeasureId(measure.Id) && Observation.Discriminator.Equals(discriminator);
    }
}
