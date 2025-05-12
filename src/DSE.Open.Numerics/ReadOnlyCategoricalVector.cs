// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Immutable;
using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// An ordered list of numbers, representing categorical variables, stored in a contiguous
/// block of memory, together with a set of string labels. The numbers must be integers and
/// the labels must be strings. There should be a label for each unique integer value.
/// </summary>
/// <typeparam name="T"></typeparam>
[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public sealed class ReadOnlyCategoricalVector<T> : ReadOnlyVector<T>
    where T : struct,
              IComparable<T>,
              IEquatable<T>,
              IBinaryInteger<T>,
              IMinMaxValue<T>
{
    public static new readonly ReadOnlyCategoricalVector<T> Empty = new([], Array.Empty<KeyValuePair<string, T>>());

    private ImmutableDictionary<string, T>? _categories;

    internal ReadOnlyCategoricalVector(T[] data, ReadOnlyMemory<KeyValuePair<string, T>> categories) : base(data)
    {
        if (data.Length < categories.Length)
        {
            throw new ArgumentException("There cannot be more category labels that data values.");
        }

        CategoryData = categories;
    }

    internal ReadOnlyCategoricalVector(ReadOnlyMemory<T> data, ReadOnlyMemory<KeyValuePair<string, T>> categories) : base(data)
    {
        if (data.Length < categories.Length)
        {
            throw new ArgumentException("There cannot be more category labels that data values.");
        }

        CategoryData = categories;
    }

    internal ReadOnlyCategoricalVector(T[] data, int start, int length, ReadOnlyMemory<KeyValuePair<string, T>> categories) : base(data, start, length)
    {
        if (length < categories.Length)
        {
            throw new ArgumentException("There cannot be more category labels that data values.");
        }

        CategoryData = categories;
    }

    /// <summary>
    /// Determines whether there is a label for each unique integer value.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if there is a label for each unique integer value,
    /// otherwise <see langword="false"/>.
    /// </returns>
    public bool IsValid()
    {
        if (Length == 0 && CategoryData.Length == 0)
        {
            return true;
        }

        var values = new HashSet<T>();

        foreach (var v in AsReadOnlySpan())
        {
            _ = values.Add(v);
        }

        if (values.Count != CategoryData.Length)
        {
            return false;
        }

        foreach (var (_, value) in CategoryData.Span)
        {
            if (!values.Contains(value))
            {
                return false;
            }
        }

        return true;
    }

    public ReadOnlyMemory<KeyValuePair<string, T>> CategoryData { get; }

    public ImmutableDictionary<string, T> Categories => _categories ??= CreateCategories();

    private ImmutableDictionary<string, T> CreateCategories()
    {
        if (Categories.Count == 0)
        {
            return ImmutableDictionary<string, T>.Empty;
        }

        var builder = ImmutableDictionary.CreateBuilder<string, T>(StringComparer.OrdinalIgnoreCase);

        foreach (var (key, value) in CategoryData.Span)
        {
            builder.Add(key, value);
        }

        return builder.ToImmutable();
    }
}
