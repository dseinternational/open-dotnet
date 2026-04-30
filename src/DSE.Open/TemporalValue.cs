// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// A value associated with a time.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly record struct TemporalValue<T>
{
    /// <summary>
    /// Initialises a new <see cref="TemporalValue{T}"/> with the specified <paramref name="time"/> and <paramref name="value"/>.
    /// </summary>
    [SetsRequiredMembers]
    public TemporalValue(DateTimeOffset time, T value)
    {
        Time = time;
        Value = value;
    }

    /// <summary>
    /// The time that is related to the value.
    /// </summary>
    [JsonPropertyName("t")]
    [JsonConverter(typeof(JsonDateTimeOffsetUnixTimeMillisecondsConverter))]
    public required DateTimeOffset Time { get; init; }

    /// <summary>
    /// The value in relation to the time.
    /// </summary>
    [JsonPropertyName("v")]
    public required T Value { get; init; }
}

/// <summary>
/// Helpers for creating <see cref="TemporalValue{T}"/> instances.
/// </summary>
public static class TemporalValue
{
    /// <summary>
    /// Creates a <see cref="TemporalValue{T}"/> at the current UTC time using <see cref="TimeProvider.System"/>.
    /// </summary>
    public static TemporalValue<T> ForUtcNow<T>(T value)
    {
        return ForUtcNow(value, TimeProvider.System);
    }

    /// <summary>
    /// Creates a <see cref="TemporalValue{T}"/> at the current UTC time obtained from <paramref name="timeProvider"/>.
    /// </summary>
    public static TemporalValue<T> ForUtcNow<T>(T value, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new TemporalValue<T>(timeProvider.GetUtcNow(), value);
    }

    /// <summary>
    /// Creates a <see cref="TemporalValue{T}"/> at the current local time using <see cref="TimeProvider.System"/>.
    /// </summary>
    public static TemporalValue<T> ForLocalNow<T>(T value)
    {
        return ForLocalNow(value, TimeProvider.System);
    }

    /// <summary>
    /// Creates a <see cref="TemporalValue{T}"/> at the current local time obtained from <paramref name="timeProvider"/>.
    /// </summary>
    public static TemporalValue<T> ForLocalNow<T>(T value, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new TemporalValue<T>(timeProvider.GetLocalNow(), value);
    }
}
