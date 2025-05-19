// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;

namespace DSE.Open.Numerics;

public abstract class VectorBase
{
    protected internal VectorBase(
        VectorDataType dataType,
        Type itemType,
        int length,
        bool isReadOnly)
    {
        ArgumentNullException.ThrowIfNull(itemType);
        Ensure.EqualOrGreaterThan(length, 0);

        IsEmpty = length == 0;

#if DEBUG
        if (Vector.TryGetVectorDataType(itemType, out var expectedDataType)
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
        IsReadOnly = isReadOnly;
        IsNullable = dataType >= VectorDataType.NullableFloat64;
    }

    /// <summary>
    /// Gets the number of items in the vector.
    /// </summary>
    public int Length { get; }

    public bool IsEmpty { get; }

    /// <summary>
    /// Indicates if the item type is a known numeric type.
    /// </summary>
    public bool IsNumeric { get; }

    public bool IsNullable { get; }

    /// <summary>
    /// Gets the type of the items in the vector.
    /// </summary>
    public Type ItemType { get; }

    /// <summary>
    /// Gets the data type of the vector.
    /// </summary>
    public VectorDataType DataType { get; }

    public bool IsReadOnly { get; }
}
