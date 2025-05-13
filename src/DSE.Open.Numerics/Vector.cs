// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;
using CommunityToolkit.HighPerformance;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type.
/// Optionally named, labelled or categorised for use with a <see cref="DataFrame"/>.
/// </summary>
[JsonConverter(typeof(VectorJsonConverter))]
public abstract class Vector : IVector
{
    private Memory<Variant> _labels;

    protected internal Vector(
        VectorDataType dataType,
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
        if (VectorDataTypeHelper.TryGetVectorDataType(itemType, out var expectedDataType)
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
    public VectorDataType DataType { get; }

    /// <summary>
    /// Gets or sets a name for the series (optional).
    /// </summary>
    public string? Name { get; set; }

    public virtual bool IsReadOnly { get; }

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

    protected abstract ReadOnlyVector CreateReadOnly();

    public ReadOnlyVector AsReadOnly()
    {
        return CreateReadOnly();
    }

    IReadOnlyVector IVector.AsReadOnly()
    {
        return AsReadOnly();
    }

    /// <summary>
    /// Creates a vector from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Vector<T> Create<T>(Memory<T> data)
    {
        return new Vector<T>(data);
    }

    public static Vector<T> Create<T>(Memory<T> data, Memory<Variant> labels)
    {
        return new Vector<T>(data, labels: labels);
    }

    /// <summary>
    /// Creates a vector from the given data with the specified name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Vector<T> Create<T>(string name, Memory<T> data)
    {
        return new Vector<T>(data, name);
    }

    public static Vector<T> Create<T>(string name, Memory<T> data, Memory<Variant> labels)
    {
        return new Vector<T>(data, name, labels);
    }

    public static Vector<T> Create<T>(string name, Memory<T> data, Memory<Variant> labels, Memory<KeyValuePair<string, T>> categories)
    {
        return new Vector<T>(data, name, labels, categories);
    }

    public static Vector<T> Create<T>(string name, Memory<T> data, IReadOnlyDictionary<string, T> categories)
    {
        return Create(name, data, [.. categories]);
    }

    public static Vector<T> Create<T>(string name, Memory<T> data, KeyValuePair<string, T>[] categories)
    {
        return Create(name, data, categories.AsMemory());
    }

    public static Vector<T> Create<T>(string name, Memory<T> data, Memory<KeyValuePair<string, T>> categories)
    {
        return new Vector<T>(data, name, default, categories);
    }

    /// <summary>
    /// Creates a vector from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Vector<T> Create<T>(T[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Vector<T>(data);
    }

    /// <summary>
    /// Creates a vector from the given data with the specified name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Vector<T> Create<T>(string name, T[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Vector<T>(data, name);
    }

    public static Vector<T> Create<T>(T[] data, IReadOnlyDictionary<string, T> categories)
    {
        return Create(data, [.. categories]);
    }

    public static Vector<T> Create<T>(T[] data, KeyValuePair<string, T>[] categories)
    {
        return Create(data, categories.AsMemory());
    }

    public static Vector<T> Create<T>(T[] data, Memory<KeyValuePair<string, T>> categories)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new Vector<T>(data.AsMemory(), null, default, categories);
    }

    public static Vector<T> Create<T>(string name, T[] data, IReadOnlyDictionary<string, T> categories)
    {
        return Create(name, data.AsMemory(), [.. categories]);
    }

    public static Vector<T> Create<T>(string name, T[] data, KeyValuePair<string, T>[] categories)
    {
        return new(data.AsMemory(), name, categories: categories.AsMemory());
    }

    public static Vector<T> Create<T>(string name, T[] data, Memory<KeyValuePair<string, T>> categories)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new(data.AsMemory(), name, categories: categories);
    }

    public static Vector<T> Create<T>(
        string name,
        T[] data,
        Memory<Variant> labels,
        IReadOnlyDictionary<string, T> categories)
    {
        ArgumentNullException.ThrowIfNull(data);
        return Create(name, data, labels, [.. categories]);
    }

    public static Vector<T> Create<T>(
        string name,
        T[] data,
        Memory<Variant> labels,
        KeyValuePair<string, T>[] categories)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new(data.AsMemory(), name, labels, categories.AsMemory());
    }

    public static Vector<T> Create<T>(
        string name,
        T[] data,
        Memory<Variant> labels,
        Memory<KeyValuePair<string, T>> categories)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new(data.AsMemory(), name, labels, categories);
    }

    public static Vector<T> Create<T>(ReadOnlySpan<T> data)
    {
        return new(data.ToArray());
    }

    public static Vector<T> Create<T>(string name, ReadOnlySpan<T> data)
    {
        return new(data.ToArray());
    }

    public static Vector<T> Create<T>(string name, ReadOnlySpan<T> data, ReadOnlySpan<KeyValuePair<string, T>> categories)
    {
        return new(data.ToArray(), name, categories: categories.ToArray());
    }

    public static ReadOnlyVector<T> CreateReadOnly<T>(ReadOnlySpan<T> data)
    {
        return new(data.ToArray());
    }

    public static ReadOnlyVector<T> CreateReadOnly<T>(string name, ReadOnlySpan<T> data)
    {
        return new(data.ToArray(), name);
    }

    public static ReadOnlyVector<T> CreateReadOnly<T>(
        string name,
        ReadOnlySpan<T> data,
        ReadOnlySpan<KeyValuePair<string, T>> categories)
    {
        return new(data.ToArray(), name, categories: categories.ToArray());
    }

    public static Vector<T> Create<T>(int length)
        where T : struct, INumber<T>
    {
        return new Vector<T>(new T[length]);
    }

    public static Vector<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new Vector<T>(data);
    }

    public static Vector<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero);
    }

    public static Vector<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.One);
    }

    public static Vector<T> Create<T>(string name, int length)
        where T : struct, INumber<T>
    {
        return new Vector<T>(new T[length], name);
    }
}
