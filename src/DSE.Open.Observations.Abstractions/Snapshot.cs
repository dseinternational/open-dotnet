// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// Records an observation at a point in time.
/// </summary>
public abstract record Snapshot
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

public static class ObservationExtensions
{
    public static bool HasMeasurement(this Observation observation, Measure measure)
    {
        ArgumentNullException.ThrowIfNull(measure);
        return observation.HasMeasureId(measure.Id);
    }
}
