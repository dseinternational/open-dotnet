// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Results;

/// <summary>
/// Base class for building instances of <typeparamref name="TResult"/> that carry a value of
/// type <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TResult">The type of <see cref="ValueResult{TValue}"/> produced.</typeparam>
/// <typeparam name="TValue">The type of the carried value.</typeparam>
public abstract class ValueResultBuilder<TResult, TValue> : ResultBuilder<TResult>
    where TResult : ValueResult<TValue>
{
    /// <summary>
    /// Gets or sets the value that will be assigned to the built result.
    /// </summary>
    public virtual TValue? Value { get; set; }

    /// <summary>
    /// Merges notifications from <paramref name="valueResult"/> and copies its value.
    /// </summary>
    public virtual void MergeNotificationsAndValue(TResult valueResult)
    {
        ArgumentNullException.ThrowIfNull(valueResult);
        MergeNotifications(valueResult);
        Value = valueResult.Value;
    }
}

/// <summary>
/// Builds a <see cref="ValueResult{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type of the carried value.</typeparam>
public class ValueResultBuilder<TValue> : ValueResultBuilder<ValueResult<TValue>, TValue>
{
    /// <inheritdoc />
    public override ValueResult<TValue> Build()
    {
        return new()
        {
            Status = Status,
            Value = Value,
            Notifications = [.. Notifications],
        };
    }
}
