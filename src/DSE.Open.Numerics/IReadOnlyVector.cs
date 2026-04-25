// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, fixed-length, contiguous sequence of read-only values.
/// </summary>
/// <remarks>
/// The non-generic <see cref="IReadOnlyVector"/> exposes the metadata that does
/// not depend on the element type. Strongly-typed access goes through
/// <see cref="IReadOnlyVector{T}"/>.
/// </remarks>
public interface IReadOnlyVector
{
    /// <summary>
    /// Gets the <see cref="VectorDataType"/> tag identifying the element type
    /// stored by this vector.
    /// </summary>
    VectorDataType DataType { get; }

    /// <summary>
    /// Gets <see langword="true"/> when this vector contains no elements.
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Gets <see langword="true"/> when the element type is one of the
    /// numeric <see cref="VectorDataType"/> variants (integer, floating-point,
    /// or the corresponding NA-aware wrapper).
    /// </summary>
    bool IsNumeric { get; }

    /// <summary>
    /// Gets <see langword="true"/> when the element type can carry an NA value
    /// — i.e., it is one of the <c>Na*</c> <see cref="VectorDataType"/> variants
    /// such as <see cref="VectorDataType.NaFloat64"/> or <see cref="VectorDataType.NaInt32"/>.
    /// </summary>
    bool IsNullable { get; }

    /// <summary>
    /// Gets the runtime <see cref="Type"/> of the elements in the vector. This
    /// matches the open generic argument of any concrete
    /// <see cref="IReadOnlyVector{T}"/> implementation.
    /// </summary>
    Type ItemType { get; }

    /// <summary>
    /// Gets the number of elements in the vector.
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Returns the element at <paramref name="index"/> boxed into a
    /// type-erased <see cref="VectorValue"/> for callers that do not have the
    /// concrete element type.
    /// </summary>
    /// <param name="index">Zero-based index of the element to read.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when
    /// <paramref name="index"/> is outside <c>[0, Length)</c>.</exception>
    VectorValue GetVectorValue(int index);
}

/// <summary>
/// A serializable, fixed-length, contiguous sequence of read-only values of type <typeparamref name="T"/>.
/// </summary>
public interface IReadOnlyVector<T>
    : IReadOnlyVector,
      IReadOnlyList<T>,
      IEquatable<IReadOnlyVector<T>?>
    where T : IEquatable<T>
{

    /// <summary>
    /// Gets a span over the contents of the vector.
    /// </summary>
    ReadOnlySpan<T> AsSpan();

    /// <summary>
    /// Returns a slice of this vector starting at <paramref name="start"/> with
    /// the given <paramref name="length"/>. The slice shares the underlying
    /// storage with the source — no copy is made.
    /// </summary>
    /// <param name="start">Zero-based start index of the slice.</param>
    /// <param name="length">Number of elements in the slice.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when
    /// <paramref name="start"/> or <paramref name="length"/> is negative, or
    /// when <c>start + length</c> exceeds <see cref="IReadOnlyVector.Length"/>.</exception>
    IReadOnlyVector<T> Slice(int start, int length);
}
