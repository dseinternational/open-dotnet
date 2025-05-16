// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

public static class ReadOnlyCategoricalSeries
{
    public static ReadOnlyCategoricalSeries<T> Create<T>(
        [NotNull] ReadOnlyVector<T> vector,
        string? name)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, name, null, null, null);
    }

    public static ReadOnlyCategoricalSeries<T> Create<T>(
        [NotNull] ReadOnlyVector<T> vector,
        ReadOnlyCategorySet<T>? categories)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, null, null, categories, null);
    }

    public static ReadOnlyCategoricalSeries<T> Create<T>(
        [NotNull] ReadOnlyVector<T> vector,
        string? name,
        ReadOnlyCategorySet<T>? categories)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, name, null, categories, null);
    }

    public static ReadOnlyCategoricalSeries<T> Create<T>(
        [NotNull] ReadOnlyVector<T> vector,
        ReadOnlyCategorySet<T>? categories,
        ReadOnlyValueLabelCollection<T>? labels)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, null, null, categories, labels);
    }

    public static ReadOnlyCategoricalSeries<T> Create<T>(
        [NotNull] ReadOnlyVector<T> vector,
        string? name,
        ReadOnlyCategorySet<T>? categories,
        ReadOnlyValueLabelCollection<T>? labels)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, name, null, categories, labels);
    }

    public static ReadOnlyCategoricalSeries<T> Create<T>(
        [NotNull] ReadOnlyVector<T> vector,
        string? name,
        Index? index,
        ReadOnlyCategorySet<T>? categories,
        ReadOnlyValueLabelCollection<T>? labels)
        where T : struct, IBinaryNumber<T>
    {
        return new ReadOnlyCategoricalSeries<T>(vector, name, index, labels, categories, false);
    }
}

/// <summary>
/// TODO - a read-only series than may only contain values from a defined set.
/// </summary>
/// <typeparam name="T"></typeparam>
[JsonConverter(typeof(SeriesJsonConverter))]
public sealed class ReadOnlyCategoricalSeries<T> : ReadOnlySeries<T>, IReadOnlyCategoricalSeries<T>
    where T : struct, IBinaryNumber<T>
{
    public static new readonly ReadOnlyCategoricalSeries<T> Empty =
        new([], null, null, null, null, true);

    private readonly ReadOnlyCategorySet<T> _categories;

    internal ReadOnlyCategoricalSeries(
        [NotNull] ReadOnlyVector<T> vector,
        string? name,
        Index? index,
        ReadOnlyValueLabelCollection<T>? labels,
        ReadOnlyCategorySet<T>? categories,
        bool skipValidation)
        : base(vector, name, index, labels)
    {
        if (categories is not null)
        {
            _categories = categories;
        }
        else
        {
            if (Length == 0)
            {
                _categories = [];
            }
            else
            {
                // initialize from values in the vector
                _categories = new ReadOnlyCategorySet<T>(new ReadOnlySet<T>(vector));
                skipValidation = true;
            }
        }

        if (!skipValidation)
        {
            EnsureValid();
        }
    }

    public override bool IsCategorical { get; } = true;

    private void EnsureValid()
    {
        if (Length == 0)
        {
            return;
        }

        foreach (var value in this)
        {
            if (!_categories.Contains(value))
            {
                throw new ArgumentException($"Value {value} is not a valid category value.", nameof(value));
            }
        }
    }

    public ReadOnlyCategorySet<T> Categories => _categories;

    IReadOnlyCategorySet<T> IReadOnlyCategoricalSeries<T>.Categories => Categories;

    IReadOnlyCategorySet IReadOnlyCategoricalSeries.Categories => Categories;
}
