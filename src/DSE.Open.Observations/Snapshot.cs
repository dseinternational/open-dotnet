// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open.Observations;

public abstract class Snapshot : IEquatable<Snapshot>, ISnapshot
{
    internal Snapshot(IObservation observation, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);

        Time = DateTimeOffset.FromUnixTimeMilliseconds(timeProvider.GetUtcNow().ToUnixTimeMilliseconds());
        Observation = observation;
    }

    internal Snapshot(IObservation observation, DateTimeOffset time, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(observation);
        ArgumentNullException.ThrowIfNull(timeProvider);
        Guard.IsInRange(time, Observations.Observation.MinimumObservationTime,
            timeProvider.GetUtcNow().AddSeconds(Observations.Observation.TimeToleranceSeconds));

        Time = time;
        Observation = observation;
    }

    [JsonPropertyName("t")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public DateTimeOffset Time { get; }

    [JsonPropertyName("o")]
    public IObservation Observation { get; }

    public bool Equals(Snapshot? other)
    {
        return other is not null && Time.Equals(other.Time);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Snapshot);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Time);
    }

    public abstract int GetMeasurementHashCode();

    public static Snapshot<Observation<TValue>> Create<TValue>(
        Observation<TValue> observation)
        where TValue : struct, IEquatable<TValue>, IObservationValue
    {
        return Create(observation, TimeProvider.System);
    }

    internal static Snapshot<Observation<TValue>> Create<TValue>(
        Observation<TValue> observation,
        TimeProvider timeProvider)
        where TValue : struct, IEquatable<TValue>, IObservationValue
    {
        return new Snapshot<Observation<TValue>>(observation, timeProvider);
    }

    public static Snapshot<Observation<TValue, TParam>> Create<TValue, TParam>(
        Observation<TValue, TParam> observation)
        where TValue : struct, IEquatable<TValue>, IObservationValue
        where TParam : struct, IEquatable<TParam>
    {
        return Create(observation, TimeProvider.System);
    }

    internal static Snapshot<Observation<TValue, TParam>> Create<TValue, TParam>(
        Observation<TValue, TParam> observation,
        TimeProvider timeProvider)
        where TValue : struct, IEquatable<TValue>, IObservationValue
        where TParam : struct, IEquatable<TParam>
    {
        return new Snapshot<Observation<TValue, TParam>>(observation, timeProvider);
    }
}

public sealed class Snapshot<TObs> : Snapshot, IEquatable<Snapshot<TObs>>, ISnapshot<TObs>
    where TObs : IObservation
{
    public Snapshot(TObs observation) : this(observation, TimeProvider.System)
    {
    }

    internal Snapshot(TObs observation, TimeProvider timeProvider) : base(observation, timeProvider)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Snapshot(TObs observation, DateTimeOffset time) : this(observation, time, TimeProvider.System)
    {
    }

    internal Snapshot(TObs observation, DateTimeOffset time, TimeProvider timeProvider) : base(observation, time, timeProvider)
    {
    }

    [JsonPropertyName("o")]
    public new TObs Observation => (TObs)base.Observation;

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

    public override int GetMeasurementHashCode()
    {
        return Observation.GetMeasurementHashCode();
    }

    public override string ToString()
    {
        return $"Snapshot ({Time:o}): {Observation}";
    }
}
