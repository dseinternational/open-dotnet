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
public class NumericVector<T> : Vector<T>
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
        return vector is not null ? vector.Data : default;
    }
}

