// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Observations;

public sealed class Snapshot<TObs> : IEquatable<Snapshot<TObs>>, ISnapshot<TObs>
    where TObs : IObservation
{
    public Snapshot(TObs observation) : this(observation, TimeProvider.System)
    {
    }

    internal Snapshot(TObs observation, TimeProvider timeProvider)
    {
        Observation = observation;
        Time = DateTimeOffset.FromUnixTimeMilliseconds(timeProvider.GetUtcNow().ToUnixTimeMilliseconds());
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Snapshot(TObs observation, DateTimeOffset time) : this(observation, time, TimeProvider.System)
    {
    }

    internal Snapshot(TObs observation, DateTimeOffset time, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);
        Guard.IsInRange(time, Observations.Observation.MinimumObservationTime,
            timeProvider.GetUtcNow().AddSeconds(Observations.Observation.TimeToleranceSeconds));

        Observation = observation;
        Time = time;
    }

    [JsonPropertyName("o")]
    public TObs Observation { get; }

    [JsonPropertyName("t")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public DateTimeOffset Time { get; }

    public bool Equals(Snapshot<TObs>? other)
    {
        return other is not null && Observation.Equals(other.Observation) && Time.Equals(other.Time);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Snapshot<TObs>);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Observation, Time);
    }

    public override string ToString()
    {
        return $"Snapshot ({Time:o}): {Observation}";
    }
}
