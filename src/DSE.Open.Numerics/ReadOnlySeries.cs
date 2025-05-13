// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public abstract class ReadOnlySeries : IReadOnlySeries
{
    protected ReadOnlySeries(
        SeriesDataType dataType,
        Type itemType,
        int length,
        string? name = null,
        ReadOnlyMemory<Variant> labels = default)
    {
        ArgumentNullException.ThrowIfNull(itemType);

#if DEBUG
        if (SeriesDataTypeHelper.TryGetVectorDataType(itemType, out var expectedDataType)
            && dataType != expectedDataType)
        {
            Debug.Fail($"Expected data type {expectedDataType} for " +
                $"item type {itemType.Name} but given {dataType}.");
        }
#endif

        DataType = dataType;
        IsNumeric = NumberHelper.IsKnownNumberType(itemType);
        ItemType = itemType;
        Length = length;
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
    /// Creates a series from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static ReadOnlySeries<T> Create<T>(ReadOnlyMemory<T> vector)
    {
        if (vector.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlySeries<T>(vector);
    }

    /// <summary>
    /// Creates a series from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static ReadOnlySeries<T> Create<T>(T[] array)
    {
        ArgumentNullException.ThrowIfNull(array);

        if (array.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlySeries<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlySeries<T>(array);
    }

    public static ReadOnlySeries<T> Create<T>(ReadOnlySpan<T> data)
    {
        return Create(data.ToArray());
    }

    public static ReadOnlySeries<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new(data);
    }

    public static ReadOnlySeries<T> Create<T>(int length)
        where T : struct, INumber<T>
    {
        return new(new T[length]);
    }

    public static ReadOnlySeries<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero);
    }

    public static ReadOnlySeries<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.One);
    }
}
