// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;

namespace DSE.Open.Numerics;

/// <summary>
/// An ordered list of numbers stored in a contiguous block of memory.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Implements value equality.
/// </remarks>
[CollectionBuilder(typeof(NumericVector), nameof(NumericVector.Create))]
public class NumericVector<T> : Vector<T>
    where T : struct, INumber<T>
{
    public NumericVector(T[] data) : base(data)
    {
    }

    public NumericVector(Memory<T> data) : base(data)
    {
    }

    public NumericVector(T[] data, int start, int length) : base(data, start, length)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator NumericVector<T>(T[] vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator NumericVector<T>(Memory<T> vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator ReadOnlyNumericVector<T>(NumericVector<T> vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        return new(vector.Memory);
    }
}
