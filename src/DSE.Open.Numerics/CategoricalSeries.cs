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
            VectorDataType.Bool => throw OnlyBinaryIntegerValues(),
            VectorDataType.Char => throw OnlyBinaryIntegerValues(),
            VectorDataType.DateTime => throw OnlyBinaryIntegerValues(),
            VectorDataType.DateTime64 => throw OnlyBinaryIntegerValues(),
            VectorDataType.DateTimeOffset => throw OnlyBinaryIntegerValues(),
            VectorDataType.Float16 => throw OnlyBinaryIntegerValues(),
            VectorDataType.Float32 => throw OnlyBinaryIntegerValues(),
            VectorDataType.Float64 => throw OnlyBinaryIntegerValues(),
            VectorDataType.Int16 => Create((Vector<short>)data, name, index, categories as CategorySet<short>, labels as ValueLabelCollection<short>),
            VectorDataType.Int32 => Create((Vector<int>)data, name, index, categories as CategorySet<int>, labels as ValueLabelCollection<int>),
            VectorDataType.Int64 => Create((Vector<long>)data, name, index, categories as CategorySet<long>, labels as ValueLabelCollection<long>),
            VectorDataType.Int8 => Create((Vector<sbyte>)data, name, index, categories as CategorySet<sbyte>, labels as ValueLabelCollection<sbyte>),
            VectorDataType.String => throw OnlyBinaryIntegerValues(),
            VectorDataType.UInt16 => Create((Vector<ushort>)data, name, index, categories as CategorySet<ushort>, labels as ValueLabelCollection<ushort>),
            VectorDataType.UInt32 => Create((Vector<uint>)data, name, index, categories as CategorySet<uint>, labels as ValueLabelCollection<uint>),
            VectorDataType.UInt64 => Create((Vector<ulong>)data, name, index, categories as CategorySet<ulong>, labels as ValueLabelCollection<ulong>),
            VectorDataType.UInt8 => Create((Vector<byte>)data, name, index, categories as CategorySet<byte>, labels as ValueLabelCollection<byte>),
            VectorDataType.NaBool => throw OnlyBinaryIntegerValues(),
            VectorDataType.NaChar => throw OnlyBinaryIntegerValues(),
            VectorDataType.NaDateTime => throw OnlyBinaryIntegerValues(),
            VectorDataType.NaDateTime64 => throw OnlyBinaryIntegerValues(),
            VectorDataType.NaDateTimeOffset => throw OnlyBinaryIntegerValues(),
            VectorDataType.NaFloat16 => throw OnlyBinaryIntegerValues(),
            VectorDataType.NaFloat32 => throw OnlyBinaryIntegerValues(),
            VectorDataType.NaFloat64 => throw OnlyBinaryIntegerValues(),
            VectorDataType.NaInt16 => Create((Vector<NaInt<short>>)data, name, index, categories as CategorySet<NaInt<short>>, labels as ValueLabelCollection<NaInt<short>>),
            VectorDataType.NaInt32 => Create((Vector<NaInt<int>>)data, name, index, categories as CategorySet<NaInt<int>>, labels as ValueLabelCollection<NaInt<int>>),
            VectorDataType.NaInt64 => Create((Vector<NaInt<long>>)data, name, index, categories as CategorySet<NaInt<long>>, labels as ValueLabelCollection<NaInt<long>>),
            VectorDataType.NaInt8 => Create((Vector<NaInt<sbyte>>)data, name, index, categories as CategorySet<NaInt<sbyte>>, labels as ValueLabelCollection<NaInt<sbyte>>),
            VectorDataType.NaString => throw OnlyBinaryIntegerValues(),
            VectorDataType.NaUInt16 => Create((Vector<NaInt<ushort>>)data, name, index, categories as CategorySet<NaInt<ushort>>, labels as ValueLabelCollection<NaInt<ushort>>),
            VectorDataType.NaUInt32 => Create((Vector<NaInt<uint>>)data, name, index, categories as CategorySet<NaInt<uint>>, labels as ValueLabelCollection<NaInt<uint>>),
            VectorDataType.NaUInt64 => Create((Vector<NaInt<short>>)data, name, index, categories as CategorySet<NaInt<short>>, labels as ValueLabelCollection<NaInt<short>>),
            VectorDataType.NaUInt8 => Create((Vector<NaInt<short>>)data, name, index, categories as CategorySet<NaInt<short>>, labels as ValueLabelCollection<NaInt<short>>),
            _ => throw new InvalidOperationException("Unsupported data type: " + data.DataType),
        };

        static InvalidOperationException OnlyBinaryIntegerValues()
        {
            return new InvalidOperationException("Only binary integer values may be used for a categorical series");
        }
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
