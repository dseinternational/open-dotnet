// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, fixed-length, contiguous sequence of values.
/// </summary>
[JsonConverter(typeof(VectorJsonConverter))]
public abstract partial class Vector : VectorBase, IVector
{
    protected internal Vector(VectorDataType dataType, Type itemType, int length)
        : base(dataType, itemType, length, false)
    {
    }

    protected abstract ReadOnlyVector CreateReadOnly();

    public ReadOnlyVector AsReadOnly()
    {
        return CreateReadOnly();
    }

    // -------- Factory methods --------

    public static Vector<T> Create<T>(Memory<T> memory)
        where T : IEquatable<T>
    {
        if (memory.IsEmpty)
        {
            return [];
        }

        return new Vector<T>(memory);
    }

    public static Vector<T> Create<T>(T[] array)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(array);

        if (array.Length == 0)
        {
            return [];
        }

        return new Vector<T>(array.AsMemory());
    }

    public static Vector<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        if (span.IsEmpty)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return Vector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new Vector<T>(span.ToArray().AsMemory());
    }

    public static Vector<T> Create<T>(int length)
        where T : IEquatable<T>
    {
        return new Vector<T>(new T[length]);
    }

    public static Vector<T> Create<T>(int length, T scalar)
        where T : struct, INumber<T>
    {
        var array = new T[length];
        array.AsSpan().Fill(scalar);
        return new Vector<T>(array);
    }

    public static Vector<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero);
    }

    public static Vector<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.One);
    }
}
