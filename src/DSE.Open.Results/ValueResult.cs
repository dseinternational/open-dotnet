// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Notifications;

namespace DSE.Open.Results;

public static class ValueResult
{
    public static ValueResult<T> Create<T>()
    {
        return ValueResult<T>.Empty;
    }

    public static ValueResult<T> Create<T>(T? value)
    {
        return new()
        {
            Value = value
        };
    }

    public static ValueResult<T> Create<T>(T? value, IEnumerable<Notification> notifications)
    {
        return new()
        {
            Value = value,
            Notifications = [.. notifications]
        };
    }
}

public record ValueResult<T> : Result
{
    public static new readonly ValueResult<T> Empty = new() { Value = default };

    /// <summary>
    /// Gets or initializes the value provided by the result. We explicitly require this to be set
    /// as the purpose of a ValueResult is to carry a value = even if set to null/default.
    /// </summary>
    [JsonPropertyName("value")]
    public virtual required T? Value { get; init; }

    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(Value))]
    public virtual bool HasValue => Value is not null;

    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValueAndNoErrorNotifications()
    {
        return HasValue && !HasAnyErrorNotifications();
    }

    public T RequiredValue()
    {
        if (HasValue)
        {
            return Value;
        }

        return ThrowHelper.ThrowInvalidOperationException<T>($"Required '{typeof(T).Name}' value is not available.");
    }
}
