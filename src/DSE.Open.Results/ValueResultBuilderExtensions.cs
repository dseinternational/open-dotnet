// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Collections.Generic;

namespace DSE.Open.Results;

public static class ValueResultBuilderExtensions
{
    public static TResult BuildWithValue<TResult, TValue>(
        this ValueResultBuilder<TResult, TValue> builder,
        TValue value)
        where TResult : ValueResult<TValue>
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Value = value;
        return builder.Build();
    }

    public static ValueResult<TValue> BuildWithValue<TValue>(
        this ValueResultBuilder<TValue> builder,
        TValue value)
    {
        return builder.BuildWithValue<ValueResult<TValue>, TValue>(value);
    }

    public static CollectionValueResult<TValue> BuildWithValue<TValue>(
        this CollectionValueResultBuilder<TValue> builder,
        ReadOnlyValueCollection<TValue> value)
    {
        return builder.BuildWithValue<CollectionValueResult<TValue>, ReadOnlyValueCollection<TValue>>(value);
    }

    public static CollectionValueAsyncResult<TValue> BuildWithValue<TValue>(
        this CollectionValueAsyncResultBuilder<TValue> builder,
        IAsyncEnumerable<TValue> value)
    {
        return builder.BuildWithValue<CollectionValueAsyncResult<TValue>, IAsyncEnumerable<TValue>>(value);
    }
}
