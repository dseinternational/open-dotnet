// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A measure represents a statement about a subject that is assigned a value by an observation.
/// </summary>
public abstract class Measure : IMeasure
{
    protected Measure() { }

    [SetsRequiredMembers]
    protected Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
    {
        Id = id;
        Uri = uri;
        MeasurementLevel = measurementLevel;
        Name = name;
        Statement = statement;
    }

    /// <inheritdoc/>
    [JsonPropertyName("id")]
    public required MeasureId Id { get; init; }

    /// <inheritdoc/>
    [JsonPropertyName("uri")]
    public required Uri Uri { get; init => field = Ensure.NotNull(value); }

    [JsonPropertyName("level")]
    public required MeasurementLevel MeasurementLevel { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init => field = Ensure.NotNullOrWhitespace(value); }

    [JsonPropertyName("statement")]
    public required string Statement { get; init => field = Ensure.NotNullOrWhitespace(value); }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}

public sealed class Measure<TValue> : Measure, IMeasure<TValue>
    where TValue : struct, IEquatable<TValue>
{
    public Measure()
    {
    }

    [SetsRequiredMembers]
    public Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

    public TObs CreateObservation<TObs>(TValue value, TimeProvider timeProvider)
        where TObs : IObservation<TValue>, IObservationFactory<TObs, TValue>
    {
        return TObs.Create(this, value, timeProvider);
    }
}

public sealed class Measure<TValue, TParam> : Measure, IMeasure<TValue, TParam>
    where TValue : struct, IEquatable<TValue>
    where TParam : IEquatable<TParam>
{
    public Measure()
    {
    }

    [SetsRequiredMembers]
    public Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(id, uri, measurementLevel, name, statement)
    {
    }

    public TObs CreateObservation<TObs>(TParam parameter, TValue value, TimeProvider timeProvider)
        where TObs : IObservation<TValue, TParam>, IObservationFactory<TObs, TValue, TParam>
    {
        return TObs.Create(this, parameter, value, timeProvider);
    }
}
