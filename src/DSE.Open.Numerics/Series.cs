// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable sequence of values of known length and data type with value equality semantics.
/// Optionally named, labelled or categorised for use with a <see cref="DataFrame"/>.
/// </summary>
[JsonConverter(typeof(SeriesJsonConverter))]
public abstract class Series : ISeries
{
    private Memory<Variant> _labels;

    protected internal Series(
        SeriesDataType dataType,
        Type itemType,
        int length,
        string? name = null,
        Memory<Variant> labels = default)
    {
        ArgumentNullException.ThrowIfNull(itemType);
        Ensure.EqualOrGreaterThan(length, 0);

        if (!labels.IsEmpty && length != labels.Length)
        {
            throw new ArgumentException($"Labels length {labels.Length} does not match "
                + $"series length {length}.");
        }

#if DEBUG
        if (SeriesDataTypeHelper.TryGetVectorDataType(itemType, out var expectedDataType)
            && dataType != expectedDataType)
        {
            Debug.Fail($"Expected data type {expectedDataType} for "
                + $"item type {itemType.Name} but given {dataType}.");
        }
#endif

        DataType = dataType;
        IsNumeric = NumberHelper.IsKnownNumberType(itemType);
        ItemType = itemType;
        Length = length;
        Name = name;

        // if empty, leave empty until accessed
        _labels = labels;
    }

    /// <summary>
    /// Gets the number of items in the series.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Indicates if the item type is a known numeric type.
    /// </summary>
    public bool IsNumeric { get; }

    /// <summary>
    /// Gets the type of the items in the series.
    /// </summary>
    public Type ItemType { get; }

    /// <summary>
    /// Gets the data type of the series.
    /// </summary>
    public SeriesDataType DataType { get; }

    /// <summary>
    /// Gets or sets a name for the series (optional).
    /// </summary>
    public string? Name { get; set; }

    public Memory<Variant> Labels
    {
        get
        {
            if (_labels.Length == Length)
            {
                return _labels;
            }

            var labels = new Variant[Length];

            for (var i = 0; i < Length; i++)
            {
                labels[i] = new Variant(i);
            }

            _labels = labels;

            return _labels;
        }
    }

    /// <summary>
    /// Creates a series from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Series<T> Create<T>(string name, T[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Series<T>(data, name);
    }

    /// <summary>
    /// Creates a series from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Series<T> Create<T>(T[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Series<T>(data);
    }

    public static Series<T> Create<T>(T[] data, IReadOnlyDictionary<string, T> categories)
    {
        return Create(data, [.. categories]);
    }

    public static Series<T> Create<T>(T[] data, KeyValuePair<string, T>[] categories)
    {
        return Create(data, categories.AsMemory());
    }

    public static Series<T> Create<T>(T[] data, Memory<KeyValuePair<string, T>> categories)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Series<T>(data, null, default, categories);
    }

    public static Series<T> Create<T>(ReadOnlySpan<T> data)
    {
        return Create(data.ToArray());
    }

    public static Series<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new Series<T>(data);
    }

    public static Series<T> Create<T>(int length)
        where T : struct, INumber<T>
    {
        return new Series<T>(new T[length]);
    }

    public static Series<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero);
    }

    public static Series<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.One);
    }

    protected abstract ReadOnlySeries CreateReadOnly();

    public ReadOnlySeries AsReadOnly()
    {
        return CreateReadOnly();
    }

    IReadOnlySeries ISeries.AsReadOnly()
    {
        return AsReadOnly();
    }
}
