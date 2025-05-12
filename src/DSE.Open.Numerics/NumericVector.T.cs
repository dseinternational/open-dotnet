// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using DSE.Open.Numerics.Serialization;
using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// An ordered list of numbers stored in a contiguous block of memory.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Implements value equality.
/// </remarks>
[CollectionBuilder(typeof(Vector), nameof(CreateNumeric))]
[JsonConverter(typeof(VectorJsonConverter))]
public class NumericVector<T>
    : Vector<T>,
      INumericVector<T>,
      IAdditionOperators<NumericVector<T>, NumericVector<T>, NumericVector<T>>,
      IAdditionOperators<NumericVector<T>, T, NumericVector<T>>,
      ISubtractionOperators<NumericVector<T>, NumericVector<T>, NumericVector<T>>,
      ISubtractionOperators<NumericVector<T>, T, NumericVector<T>>
    where T : struct, INumber<T>
{
    public static new readonly NumericVector<T> Empty = new([]);

    internal NumericVector(T[] data) : base(data)
    {
    }

    internal NumericVector(Memory<T> data) : base(data)
    {
    }

    internal NumericVector(T[] data, int start, int length) : base(data, start, length)
    {
    }

    public new ReadOnlyNumericVector<T> AsReadOnly()
    {
        return new ReadOnlyNumericVector<T>(Data);
    }

    protected override ReadOnlyVector CreateReadOnly()
    {
        return AsReadOnly();
    }

    public NumericVector<T> Add(IReadOnlyNumericVector<T> vector)
    {
        var destination = CreateNumeric<T>(Length);
        VectorPrimitives.Add(this, vector, destination);
        return destination;
    }

    public NumericVector<T> Add(T scalar)
    {
        var destination = CreateNumeric<T>(Length);
        VectorPrimitives.Add(this, scalar, destination);
        return destination;
    }

    public void AddInPlace(IReadOnlyNumericVector<T> vector)
    {
        VectorPrimitives.AddInPlace(this, vector);
    }

    public void AddInPlace(T scalar)
    {
        VectorPrimitives.AddInPlace(this, scalar);
    }

    public NumericVector<T> Subtract(IReadOnlyNumericVector<T> vector)
    {
        var destination = CreateNumeric<T>(Length);
        VectorPrimitives.Subtract(this, vector, destination);
        return destination;
    }

    public NumericVector<T> Subtract(T scalar)
    {
        var destination = CreateNumeric<T>(Length);
        VectorPrimitives.Subtract(this, scalar, destination);
        return destination;
    }

    public void SubtractInPlace(IReadOnlyNumericVector<T> vector)
    {
        VectorPrimitives.SubtractInPlace(this, vector);
    }

    public void SubtractInPlace(T scalar)
    {
        VectorPrimitives.SubtractInPlace(this, scalar);
    }

    public static NumericVector<T> operator +(NumericVector<T> left, NumericVector<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Add(right);
    }

    public static NumericVector<T> operator +(NumericVector<T> left, T right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Add(right);
    }

    public static NumericVector<T> operator -(NumericVector<T> left, NumericVector<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Subtract(right);
    }

    public static NumericVector<T> operator -(NumericVector<T> left, T right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return left.Subtract(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator NumericVector<T>(T[] vector)
    {
        return CreateNumeric(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator NumericVector<T>(Memory<T> vector)
    {
        return CreateNumeric(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator ReadOnlyNumericVector<T>(NumericVector<T> vector)
    {
        return vector is not null ? vector.Data : default;
    }
}
