// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// Records an observation at a point in time.
/// </summary>
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
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public DateTimeOffset Time { get; private init; }

    /// <summary>
    /// The observation at the time the snapshot was created.
    /// </summary>
    [JsonPropertyName("o")]
    public TObs Observation { get; }

    /// <summary>
    /// Gets a code that discriminates between measurement types and is suitable for use as a hash code.
    /// </summary>
    public virtual int GetMeasurementHashCode()
    {
        return Observation.GetMeasurementHashCode();
    }
}