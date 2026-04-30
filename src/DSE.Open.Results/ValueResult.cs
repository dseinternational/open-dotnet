// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Notifications;

namespace DSE.Open.Results;

/// <summary>
/// Provides factory methods for creating <see cref="ValueResult{T}"/> instances.
/// </summary>
public static class ValueResult
{
    /// <summary>
    /// Returns an empty <see cref="ValueResult{T}"/> with a default value.
    /// </summary>
    public static ValueResult<T> Create<T>()
    {
        return ValueResult<T>.Empty;
    }

    /// <summary>
    /// Creates a <see cref="ValueResult{T}"/> carrying the specified value.
    /// </summary>
    public static ValueResult<T> Create<T>(T? value)
    {
        return new()
        {
            Value = value
        };
    }

    /// <summary>
    /// Creates a <see cref="ValueResult{T}"/> carrying the specified value and notifications.
    /// </summary>
    public static ValueResult<T> Create<T>(T? value, IEnumerable<Notification> notifications)
    {
        return new()
        {
            Value = value,
            Notifications = [.. notifications]
        };
    }
}

/// <summary>
/// A <see cref="Result"/> that carries a value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the carried value.</typeparam>
public record ValueResult<T> : Result
{
    /// <summary>
    /// An empty <see cref="ValueResult{T}"/> whose value is <see langword="default"/>.
    /// </summary>
    public static new readonly ValueResult<T> Empty = new() { Value = default };

    /// <summary>
    /// Gets or initializes the value provided by the result. We explicitly require this to be set
    /// as the purpose of a ValueResult is to carry a value = even if set to null/default.
    /// </summary>
    [JsonPropertyName("value")]
    public virtual required T? Value { get; init; }

    /// <summary>
    /// Gets a value indicating whether <see cref="Value"/> is non-null.
    /// </summary>
    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(Value))]
    public virtual bool HasValue => Value is not null;

    /// <summary>
    /// Returns <see langword="true"/> if <see cref="Value"/> is non-null and there are no
    /// error-level notifications.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValueAndNoErrorNotifications()
    {
        return HasValue && !HasAnyErrorNotifications();
    }

    /// <summary>
    /// Returns <see cref="Value"/> if non-null; otherwise throws
    /// <see cref="InvalidOperationException"/>.
    /// </summary>
    public T RequiredValue()
    {
        if (HasValue)
        {
            return Value;
        }

        return ThrowHelper.ThrowInvalidOperationException<T>($"Required '{typeof(T).Name}' value is not available.");
    }
}
