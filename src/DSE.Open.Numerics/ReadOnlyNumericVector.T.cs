// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// An ordered list of numbers stored in a contiguous block of memory.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Implements value equality.
/// </remarks>
[CollectionBuilder(typeof(ReadOnlyVector), nameof(CreateNumeric))]
[JsonConverter(typeof(VectorJsonConverter))]
public class ReadOnlyNumericVector<T> : ReadOnlyVector<T>
    where T : struct, INumber<T>
{
    public static new readonly ReadOnlyNumericVector<T> Empty = new([]);

    internal ReadOnlyNumericVector(T[] data) : base(data)
    {
    }

    internal ReadOnlyNumericVector(ReadOnlyMemory<T> data) : base(data)
    {
    }

    internal ReadOnlyNumericVector(T[] data, int start, int length) : base(data, start, length)
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator ReadOnlyNumericVector<T>(T[] vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator ReadOnlyNumericVector<T>(Memory<T> vector)
    {
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "By design")]
    public static implicit operator ReadOnlyMemory<T>(ReadOnlyNumericVector<T> vector)
    {
        return vector is not null ? vector.Data : default;
    }
}
