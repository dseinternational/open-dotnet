// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public abstract class ReadOnlyVector : IReadOnlyVector
{
    protected ReadOnlyVector(
        VectorDataType dataType,
        Type itemType,
        int length,
        string? name,
        IReadOnlyDictionary<string, Variant>? annotations)
    {
        ArgumentNullException.ThrowIfNull(itemType);

#if DEBUG
        if (VectorDataTypeHelper.TryGetVectorDataType(itemType, out var expectedDataType)
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
        Name = name;
        Annotations = annotations;
    }

    public string? Name { get; }

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

    public IReadOnlyDictionary<string, Variant>? Annotations { get; }

    /// <summary>
    /// Creates a vector from the given data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
        Justification = "Type T will not be trimmed and the use of Vector<> can be statically determined.")]
    public static ReadOnlyVector<T> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(T[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (data.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlyVector<T>(data);
    }

    public static ReadOnlyVector<T> Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(ReadOnlySpan<T> data)
    {
        return Create(data.ToArray());
    }

    public static ReadOnlyVector<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var data = new T[length];
        data.AsSpan().Fill(scalar);
        return new(data);
    }

    public static ReadOnlyVector<T> Create<T>(int length)
        where T : struct, INumber<T>
    {
        return new(new T[length]);
    }

    public static ReadOnlyVector<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero);
    }

    public static ReadOnlyVector<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.One);
    }
}
