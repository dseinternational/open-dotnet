// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A measure defines what the value of an observation refers to.
/// </summary>
public abstract record Measure
{
    /// <summary>
    /// An identifier for the measure. This is generated from a hash of the <see cref="Uri" />.
    /// </summary>
    [JsonPropertyName("id")]
    public required MeasureId Id { get; init; }

    /// <summary>
    /// A URI that uniquely identifies the measure.
    /// </summary>
    [JsonPropertyName("uri")]
    public required Uri Uri { get; init; }

    [JsonPropertyName("level")]
    public required MeasurementLevel MeasurementLevel { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("statement")]
    public required string Statement { get; init; }

    public virtual bool Equals(Measure? other)
    {
        return other is not null && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}

public abstract record Measure<TObs, TValue> : Measure
    where TObs : Observation<TValue>
    where TValue : struct, IEquatable<TValue>
{
    public TObs CreateObservation(TValue value)
    {
        return CreateObservation(value, TimeProvider.System);
    }

    public TObs CreateObservation(TValue value, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return CreateObservation(value, timeProvider.GetUtcNow());
    }

    public abstract TObs CreateObservation(TValue value, DateTimeOffset timestamp);
}
