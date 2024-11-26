// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A measure represents a statement about a subject that is assigned a value by an observation.
/// </summary>
public abstract class Measure : IMeasure
{
    protected Measure(Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : this(MeasureId.FromUri(uri), uri, measurementLevel, name, statement)
    {
    }

    protected Measure(MeasureId id, Uri uri, MeasurementLevel measurementLevel, string name, string statement)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(statement);

        Id = id;
        Uri = uri;
        MeasurementLevel = measurementLevel;
        Name = name;
        Statement = statement;
    }

    /// <inheritdoc/>
    [JsonPropertyName("id")]
    public MeasureId Id { get; }

    /// <inheritdoc/>
    [JsonPropertyName("uri")]
    public Uri Uri { get; }

    [JsonPropertyName("level")]
    public MeasurementLevel MeasurementLevel { get; }

    [JsonPropertyName("name")]
    public string Name { get; }

    [JsonPropertyName("statement")]
    public string Statement { get; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}

public sealed class Measure<TValue> : Measure, IMeasure<TValue>
    where TValue : struct, IEquatable<TValue>, IObservationValue
{
    public Measure(Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(uri, measurementLevel, name, statement)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
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
    where TValue : struct, IEquatable<TValue>, IObservationValue
    where TParam : struct, IEquatable<TParam>
{
    public Measure(Uri uri, MeasurementLevel measurementLevel, string name, string statement)
        : base(uri, measurementLevel, name, statement)
    {
    }

    [JsonConstructor]
    [EditorBrowsable(EditorBrowsableState.Never)]
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
