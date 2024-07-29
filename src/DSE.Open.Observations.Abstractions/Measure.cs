// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A measure defines what the value of an observation refers to.
/// </summary>
public abstract class Measure : IEquatable<Measure>
{
    [JsonConstructor]
    protected Measure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = measurementLevel;
        Name = name;
        Statement = statement;
    }

    /// <summary>
    /// An identifier for the measure. This is generated from a hash of the <see cref="Uri" />.
    /// </summary>
    [JsonPropertyName("id")]
    public ulong Id { get; }

    /// <summary>
    /// A URI that uniquely identifies the measure.
    /// </summary>
    [JsonPropertyName("uri")]
    public Uri Uri { get; }

    [JsonPropertyName("level")]
    public MeasurementLevel MeasurementLevel { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("statement")]
    public string Statement { get; }

    public bool Equals(Measure? other)
    {
        return other is not null && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Measure);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}

public abstract class Measure<TObs, TValue> : Measure
    where TObs : Observation<TValue>
    where TValue : IEquatable<TValue>
{
    [JsonConstructor]
    protected Measure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

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

#pragma warning disable CA1005 // Avoid excessive parameters on generic types
public abstract class Measure<TObs, TValue, TDisc> : Measure
#pragma warning restore CA1005 // Avoid excessive parameters on generic types
    where TObs : Observation<TValue, TDisc>
    where TValue : IEquatable<TValue>
    where TDisc : IEquatable<TDisc>
{
    [JsonConstructor]
    protected Measure(ulong id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

    public TObs CreateObservation(TDisc discriminator, TValue value)
    {
        return CreateObservation(discriminator, value, TimeProvider.System);
    }

    public TObs CreateObservation(TDisc discriminator, TValue value, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return CreateObservation(discriminator, value, timeProvider.GetUtcNow());
    }

    public abstract TObs CreateObservation(TDisc discriminator, TValue value, DateTimeOffset timestamp);
}
