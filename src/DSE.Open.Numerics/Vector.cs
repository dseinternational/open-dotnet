// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(VectorJsonConverter))]
public abstract class Vector : IVector
{
    protected internal Vector(
        VectorDataType dataType,
        Type itemType,
        int length)
    {
        ArgumentNullException.ThrowIfNull(itemType);

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
    }

    /// <summary>
    /// Gets the number of items in the vector.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Indicates if the item type is a known numeric type.
    /// </summary>
    public bool IsNumeric { get; }

    /// <summary>
    /// Gets the type of the items in the vector.
    /// </summary>
    public Type ItemType { get; }

    /// <summary>
    /// Gets the data type of the vector.
    /// </summary>
    public VectorDataType DataType { get; }

    public virtual string ToJson(VectorJsonFormat format = default)
    {
        return "TODO";
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

    public static Vector<T> Create<T>(ReadOnlySpan<T> data)
    {
        return Create(data.ToArray());
    }

    public static Vector<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new Vector<T>(data);
    }

    public static Vector<T> Create<T>(int length)
        where T : struct, INumber<T>
    {
        return new Vector<T>(new T[length]);
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

    protected abstract ReadOnlyVector CreateReadOnly();

    public ReadOnlyVector AsReadOnly()
    {
        return CreateReadOnly();
    }
}
