// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;

namespace DSE.Open.Numerics;

/// <summary>
/// Common base for <see cref="Series"/> and <see cref="ReadOnlySeries"/>:
/// carries the type-erased pass-throughs of vector metadata
/// (<see cref="DataType"/>, <see cref="ItemType"/>, <see cref="Length"/>) plus
/// the categorical flag.
/// </summary>
public abstract class SeriesBase
{
    private readonly IReadOnlyVector _vector;

    /// <summary>
    /// Initializes the series base from a backing read-only vector view.
    /// </summary>
    /// <param name="vector">The backing vector. Must not be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="vector"/> is <see langword="null"/>.</exception>
    protected internal SeriesBase(IReadOnlyVector vector)
    {
        ArgumentNullException.ThrowIfNull(vector);

        _vector = vector;

#if DEBUG
        if (Numerics.Vector.TryGetDataType(vector.ItemType, out var expectedDataType)
            && vector.DataType != expectedDataType)
        {
            Debug.Fail($"Expected data type {expectedDataType} for "
                + $"item type {vector.ItemType.Name} but given {vector.DataType}.");
        }
#endif
    }

    /// <summary>
    /// The backing read-only view of the underlying vector.
    /// </summary>
    protected IReadOnlyVector BaseVector => _vector;

    /// <summary>
    /// Gets the number of items in the series.
    /// </summary>
    public int Length => _vector.Length;

    /// <summary>
    /// Gets <see langword="true"/> when the series contains no elements.
    /// </summary>
    public bool IsEmpty => _vector.IsEmpty;

    /// <summary>
    /// Indicates if the item type is a known numeric type.
    /// </summary>
    public bool IsNumeric => _vector.IsNumeric;

    /// <summary>
    /// Gets <see langword="true"/> when this series has a non-empty
    /// <see cref="CategorySet{T}"/> attached, restricting valid element values
    /// to the members of that set.
    /// </summary>
    public virtual bool IsCategorical { get; }

    /// <summary>
    /// Gets the type of the items in the series.
    /// </summary>
    public Type ItemType => _vector.ItemType;

    /// <summary>
    /// Gets the data type of the series.
    /// </summary>
    public VectorDataType DataType => _vector.DataType;

    /// <summary>
    /// The backing read-only view of the underlying vector. Use the
    /// strongly-typed <c>Vector</c> property on <see cref="Series{T}"/> /
    /// <see cref="ReadOnlySeries{T}"/> when the element type is known.
    /// </summary>
    public IReadOnlyVector Vector => _vector;
}
