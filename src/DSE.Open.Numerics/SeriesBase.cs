// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;

namespace DSE.Open.Numerics;

public abstract class SeriesBase
{
    private readonly IReadOnlyVector _vector;

    protected internal SeriesBase(IReadOnlyVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);

        _vector = vector;

#if DEBUG
        if (VectorDataTypeHelper.TryGetVectorDataType(vector.ItemType, out var expectedDataType)
            && vector.DataType != expectedDataType)
        {
            Debug.Fail($"Expected data type {expectedDataType} for "
                + $"item type {vector.ItemType.Name} but given {vector.DataType}.");
        }
#endif
    }

    protected IReadOnlyVector BaseVector => _vector;

    /// <summary>
    /// Gets the number of items in the series.
    /// </summary>
    public int Length => _vector.Length;

    public bool IsEmpty => _vector.IsEmpty;

    /// <summary>
    /// Indicates if the item type is a known numeric type.
    /// </summary>
    public bool IsNumeric => _vector.IsNumeric;

    /// <summary>
    /// Gets the type of the items in the series.
    /// </summary>
    public Type ItemType => _vector.ItemType;

    /// <summary>
    /// Gets the data type of the series.
    /// </summary>
    public VectorDataType DataType => _vector.DataType;
}
