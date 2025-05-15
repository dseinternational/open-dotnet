// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using DSE.Open.Collections.Generic;

namespace DSE.Open.Numerics;

public static class CategoricalSeries
{
    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        string? name)
        where T : IBinaryNumber<T>
    {
        return Create(vector, name, null, null, null);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        CategorySet<T>? categories)
        where T : IBinaryNumber<T>
    {
        return Create(vector, null, null, categories, null);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        string? name,
        CategorySet<T>? categories)
        where T : IBinaryNumber<T>
    {
        return Create(vector, name, null, categories, null);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        CategorySet<T>? categories,
        ValueLabelCollection<T>? labels)
        where T : IBinaryNumber<T>
    {
        return Create(vector, null, null, categories, labels);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        string? name,
        CategorySet<T>? categories,
        ValueLabelCollection<T>? labels)
        where T : IBinaryNumber<T>
    {
        return Create(vector, name, null, categories, labels);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        string? name,
        Index? index,
        CategorySet<T>? categories,
        ValueLabelCollection<T>? labels)
        where T : IBinaryNumber<T>
    {
        return new CategoricalSeries<T>(vector, name, index, labels, categories, false);
    }
}

/// <summary>
/// TODO - a series than may only contain values from a defined set.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class CategoricalSeries<T> : Series<T>, ICategoricalSeries<T>
    where T : IBinaryNumber<T>
{
    private readonly CategorySet<T> _categories;

    internal CategoricalSeries(
        Vector<T> vector,
        string? name,
        Index? index,
        ValueLabelCollection<T>? labels,
        CategorySet<T>? categories,
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
                _categories = [.. new Set<T>(vector)];
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

    public CategorySet<T> Categories => _categories;

    ICategorySet<T> ICategoricalSeries<T>.Categories => Categories;

    IReadOnlyCategorySet<T> IReadOnlyCategoricalSeries<T>.Categories => Categories;
}
