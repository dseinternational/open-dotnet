// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;

namespace DSE.Open.Results;

/// <summary>
/// Extension methods for <see cref="ValueResultBuilder{TResult, TValue}"/> and its derivatives.
/// </summary>
public static class ValueResultBuilderExtensions
{
    /// <summary>
    /// Sets <paramref name="value"/> on <paramref name="builder"/> and builds the result.
    /// </summary>
    public static TResult BuildWithValue<TResult, TValue>(
        this ValueResultBuilder<TResult, TValue> builder,
        TValue value)
        where TResult : ValueResult<TValue>
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Value = value;
        return builder.Build();
    }

    /// <summary>
    /// Sets <paramref name="value"/> on <paramref name="builder"/> and builds the result.
    /// </summary>
    public static ValueResult<TValue> BuildWithValue<TValue>(
        this ValueResultBuilder<TValue> builder,
        TValue value)
    {
        return builder.BuildWithValue<ValueResult<TValue>, TValue>(value);
    }

    /// <summary>
    /// Sets <paramref name="value"/> on <paramref name="builder"/> and builds the result.
    /// </summary>
    public static CollectionValueResult<TValue> BuildWithValue<TValue>(
        this CollectionValueResultBuilder<TValue> builder,
        ReadOnlyValueCollection<TValue> value)
    {
        return builder.BuildWithValue<CollectionValueResult<TValue>, ReadOnlyValueCollection<TValue>>(value);
    }

    /// <summary>
    /// Sets <paramref name="value"/> on <paramref name="builder"/> and builds the result.
    /// </summary>
    public static CollectionValueAsyncResult<TValue> BuildWithValue<TValue>(
        this CollectionValueAsyncResultBuilder<TValue> builder,
        IAsyncEnumerable<TValue> value)
    {
        return builder.BuildWithValue<CollectionValueAsyncResult<TValue>, IAsyncEnumerable<TValue>>(value);
    }
}
