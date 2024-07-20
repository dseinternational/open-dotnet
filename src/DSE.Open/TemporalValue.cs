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

public static class TemporalValue
{
    public static TemporalValue<T> ForUtcNow<T>(T value)
    {
        return ForUtcNow(value, TimeProvider.System);
    }

    public static TemporalValue<T> ForUtcNow<T>(T value, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new TemporalValue<T>(timeProvider.GetUtcNow(), value);
    }

    public static TemporalValue<T> ForLocalNow<T>(T value)
    {
        return ForLocalNow(value, TimeProvider.System);
    }

    public static TemporalValue<T> ForLocalNow<T>(T value, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        return new TemporalValue<T>(timeProvider.GetLocalNow(), value);
    }
}
