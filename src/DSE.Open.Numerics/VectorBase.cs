// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;

namespace DSE.Open.Numerics;

/// <summary>
/// Common base for <see cref="Vector"/> and <see cref="ReadOnlyVector"/>:
/// carries the type-erased metadata (<see cref="DataType"/>,
/// <see cref="ItemType"/>, <see cref="Length"/>) shared by every vector,
/// independent of element type or read/write capability.
/// </summary>
public abstract class VectorBase
{
    /// <summary>
    /// Initializes the vector base with the supplied dtype, runtime element
    /// type, length, and read-only flag.
    /// </summary>
    /// <param name="dataType">The <see cref="VectorDataType"/> tag for the element type.</param>
    /// <param name="itemType">The runtime <see cref="Type"/> of the element.</param>
    /// <param name="length">The number of elements in the vector. Must be non-negative.</param>
    /// <param name="isReadOnly">Whether the vector is read-only.</param>
    /// <exception cref="ArgumentNullException"><paramref name="itemType"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
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
        if (Vector.TryGetDataType(itemType, out var expectedDataType)
            && dataType != expectedDataType)
        {
            Debug.Fail($"Expected data type {expectedDataType} for "
                + $"item type {itemType.Name} but given {dataType}.");
        }
#endif

        DataType = dataType;
        IsNumeric = NumericsNumberHelper.IsKnownNumberType(itemType);
        ItemType = itemType;
        Length = length;
        IsReadOnly = isReadOnly;
        IsNullable = dataType >= VectorDataType.NaFloat64;
    }

    /// <summary>
    /// Gets the number of items in the vector.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Gets <see langword="true"/> when this vector contains no elements.
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    /// Indicates if the item type is a known numeric type.
    /// </summary>
    public bool IsNumeric { get; }

    /// <summary>
    /// Gets <see langword="true"/> when the element type can carry an NA value
    /// (i.e., it is one of the <c>Na*</c> <see cref="VectorDataType"/> variants
    /// such as <see cref="VectorDataType.NaFloat64"/> or
    /// <see cref="VectorDataType.NaInt32"/>).
    /// </summary>
    public bool IsNullable { get; }

    /// <summary>
    /// Gets the type of the items in the vector.
    /// </summary>
    public Type ItemType { get; }

    /// <summary>
    /// Gets the data type of the vector.
    /// </summary>
    public VectorDataType DataType { get; }

    /// <summary>
    /// Gets <see langword="true"/> when this vector exposes only read access
    /// (the underlying storage may itself still be mutable through other views).
    /// </summary>
    public bool IsReadOnly { get; }
}
