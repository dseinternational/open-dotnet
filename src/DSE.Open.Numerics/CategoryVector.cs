// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

// todo

[JsonConverter(typeof(VectorJsonConverter))]
public sealed class CategoryVector<TKey, TValue> : NumericVector<TValue>
    where TKey : notnull
    where TValue : struct, System.Numerics.INumber<TValue>
{
    public CategoryVector(TValue[] data, Dictionary<TKey, TValue> categories) : base(data)
    {
        ArgumentNullException.ThrowIfNull(categories);
        Categories = categories;
    }

    public CategoryVector(Memory<TValue> data, Dictionary<TKey, TValue> categories) : base(data)
    {
        ArgumentNullException.ThrowIfNull(categories);
        Categories = categories;
    }

    public CategoryVector(TValue[] data, int start, int length, Dictionary<TKey, TValue> categories) : base(data, start, length)
    {
        ArgumentNullException.ThrowIfNull(categories);
        Categories = categories;
    }

    public Dictionary<TKey, TValue> Categories { get; }
}
