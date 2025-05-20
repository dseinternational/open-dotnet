// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Collections.Generic;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

public static class CategoricalSeries
{
    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        string? name)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, name, null, null, null);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        CategorySet<T>? categories)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, null, null, categories, null);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        string? name,
        CategorySet<T>? categories)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, name, null, categories, null);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        CategorySet<T>? categories,
        ValueLabelCollection<T>? labels)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, null, null, categories, labels);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        string? name,
        CategorySet<T>? categories,
        ValueLabelCollection<T>? labels)
        where T : struct, IBinaryNumber<T>
    {
        return Create(vector, name, null, categories, labels);
    }

    public static CategoricalSeries<T> Create<T>(
        [NotNull] Vector<T> vector,
        string? name,
        Index? index,
        CategorySet<T>? categories,
        ValueLabelCollection<T>? labels)
        where T : struct, IBinaryNumber<T>
    {
        return new CategoricalSeries<T>(vector, name, index, labels, categories, false);
    }

    internal static Series CreateUntyped(
        [NotNull] Vector data,
        string? name,
        Index? index,
        ICategorySet? categories,
        object? labels) // todo
    {
        ArgumentNullException.ThrowIfNull(data);

        return data.DataType switch
        {
            VectorDataType.Int64 => new CategoricalSeries<long>((Vector<long>)data, name, index, labels as ValueLabelCollection<long>, categories as CategorySet<long>, false),
            VectorDataType.UInt64 => new CategoricalSeries<ulong>((Vector<ulong>)data, name, index, labels as ValueLabelCollection<ulong>, categories as CategorySet<ulong>, false),
            VectorDataType.Int32 => new CategoricalSeries<int>((Vector<int>)data, name, index, labels as ValueLabelCollection<int>, categories as CategorySet<int>, false),
            VectorDataType.UInt32 => new CategoricalSeries<uint>((Vector<uint>)data, name, index, labels as ValueLabelCollection<uint>, categories as CategorySet<uint>, false),
            VectorDataType.Int16 => new CategoricalSeries<short>((Vector<short>)data, name, index, labels as ValueLabelCollection<short>, categories as CategorySet<short>, false),
            VectorDataType.UInt16 => new CategoricalSeries<ushort>((Vector<ushort>)data, name, index, labels as ValueLabelCollection<ushort>, categories as CategorySet<ushort>, false),
            VectorDataType.Int8 => new CategoricalSeries<sbyte>((Vector<sbyte>)data, name, index, labels as ValueLabelCollection<sbyte>, categories as CategorySet<sbyte>, false),
            VectorDataType.UInt8 => new CategoricalSeries<byte>((Vector<byte>)data, name, index, labels as ValueLabelCollection<byte>, categories as CategorySet<byte>, false),
            VectorDataType.Float64 => throw new InvalidOperationException("Only binary integer values may be used for a categorical series"),
            VectorDataType.Float32 => throw new InvalidOperationException("Only binary integer values may be used for a categorical series"),
            VectorDataType.DateTime64 => throw new InvalidOperationException("Only binary integer values may be used for a categorical series"),
            VectorDataType.DateTime => throw new InvalidOperationException("Only binary integer values may be used for a categorical series"),
            VectorDataType.DateTimeOffset => throw new InvalidOperationException("Only binary integer values may be used for a categorical series"),
            VectorDataType.Bool => throw new InvalidOperationException("Only binary integer values may be used for a categorical series"),
            VectorDataType.Char => throw new InvalidOperationException("Only binary integer values may be used for a categorical series"),
            VectorDataType.String => throw new InvalidOperationException("Only binary integer values may be used for a categorical series"),
            VectorDataType.NaFloat64 => throw new NotImplementedException(),
            VectorDataType.NaFloat32 => throw new NotImplementedException(),
            VectorDataType.NaInt64 => throw new NotImplementedException(),
            VectorDataType.NaUInt64 => throw new NotImplementedException(),
            VectorDataType.NaInt32 => throw new NotImplementedException(),
            VectorDataType.NaUInt32 => throw new NotImplementedException(),
            VectorDataType.NaInt16 => throw new NotImplementedException(),
            VectorDataType.NaUInt16 => throw new NotImplementedException(),
            VectorDataType.NaInt8 => throw new NotImplementedException(),
            VectorDataType.NaUInt8 => throw new NotImplementedException(),
            VectorDataType.NaDateTime64 => throw new NotImplementedException(),
            VectorDataType.NaDateTime => throw new NotImplementedException(),
            VectorDataType.NaDateTimeOffset => throw new NotImplementedException(),
            VectorDataType.NaBool => throw new NotImplementedException(),
            VectorDataType.NaChar => throw new NotImplementedException(),
            VectorDataType.NaString => throw new NotImplementedException(),
            _ => throw new InvalidOperationException("Unsupported data type: " + data.DataType),
        };
    }
}

/// <summary>
/// TODO - a series than may only contain values from a defined set.
/// </summary>
/// <typeparam name="T"></typeparam>
[JsonConverter(typeof(SeriesJsonConverter))]
public sealed class CategoricalSeries<T> : Series<T>, ICategoricalSeries<T>
    where T : struct, IBinaryNumber<T>
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

    public CategorySet<T> Categories => _categories;

    ICategorySet<T> ICategoricalSeries<T>.Categories => Categories;

    IReadOnlyCategorySet<T> IReadOnlyCategoricalSeries<T>.Categories => Categories;

    IReadOnlyCategorySet IReadOnlyCategoricalSeries.Categories => Categories;

    public new ReadOnlyCategoricalSeries<T> AsReadOnly()
    {
        return new ReadOnlyCategoricalSeries<T>(
            Vector,
            Name,
            null,
            ValueLabels.AsReadOnly(),
            _categories.AsReadOnly(),
            true);
    }

    protected override ReadOnlySeries CreateReadOnly()
    {
        return AsReadOnly();
    }

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
}
