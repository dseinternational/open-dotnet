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
    protected Snapshot(DateTimeOffset time)
    {
        ObservationsValidator.EnsureMinimumObservationTime(time);
        Time = time;
    }

    /// <summary>
    /// The time the snapshot was created.
    /// </summary>
    [JsonPropertyName("t")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public DateTimeOffset Time { get; private init; }

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
    protected Snapshot(DateTimeOffset time, TObs observation) : base(time)
    {
        Observation = observation;
    }

    /// <summary>
    /// The observation at the time the snapshot was created.
    /// </summary>
    [JsonPropertyName("o")]
    public TObs Observation { get; }

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
    protected Snapshot(DateTimeOffset time, TObs observation) : base(time, observation)
    {
    }
}

/// <summary>
/// Records an observation at a point in time.
/// </summary>
public abstract record Snapshot<TObs, TValue, TDisc> : Snapshot<TObs, TValue>
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
    where TDisc : IEquatable<TDisc>
{
    protected Snapshot(DateTimeOffset time, TObs observation) : base(time, observation)
    {
    }
}
