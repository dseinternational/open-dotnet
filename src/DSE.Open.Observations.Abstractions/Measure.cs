// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Observations;

/// <summary>
/// A measure represents a statement about a subject that is assigned a value by an observation.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Measure<>), 0)]
[JsonDerivedType(typeof(Measure<,>), 1)]
public abstract record Measure
{
    /// <summary>
    /// An identifier for the measure. This is generated from a predichash of the <see cref="Uri" />.
    /// </summary>
    [JsonPropertyName("id")]
    public required MeasureId Id { get; init; }

    /// <summary>
    /// A URI that uniquely identifies the measure.
    /// </summary>
    [JsonPropertyName("uri")]
    public required Uri Uri { get; init => field = Ensure.NotNull(value); }

    [JsonPropertyName("level")]
    public required MeasurementLevel MeasurementLevel { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init => field = Ensure.NotNullOrWhitespace(value); }

    [JsonPropertyName("statement")]
    public required string Statement { get; init => field = Ensure.NotNullOrWhitespace(value); }

    public virtual bool Equals(Measure? other)
    {
        return other is not null && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}

/// <summary>
/// A measure that represents a statement about a subject that is assigned a value
/// of type <typeparamref name="TValue"/> by an observation.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public record Measure<TValue> : Measure
    where TValue : struct, IEquatable<TValue>
{
    public Observation<TValue> CreateObservation(TValue value)
    {
        return CreateObservation(value, TimeProvider.System);
    }

    public Observation<TValue> CreateObservation(TValue value, TimeProvider timeProvider)
    {
        return new Observation<TValue>(this, value, timeProvider);
    }
}

/// <summary>
/// A measure that represents a statement about a subject that is assigned a value
/// of type <typeparamref name="TValue"/> and a parameter of type <typeparamref name="TParam"/>
/// by an observation.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TParam"></typeparam>
public record Measure<TValue, TParam> : Measure
    where TValue : struct, IEquatable<TValue>
    where TParam : IEquatable<TParam>
{
    public Observation<TValue, TParam> CreateObservation(TParam parameter, TValue value)
    {
        return CreateObservation(parameter, value, TimeProvider.System);
    }

    public Observation<TValue, TParam> CreateObservation(TParam parameter, TValue value, TimeProvider timeProvider)
    {
        return new Observation<TValue, TParam>(this, parameter, value, timeProvider);
    }
}
