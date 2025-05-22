// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, contiguous sequence of values of known length and data type.
/// </summary>
[JsonConverter(typeof(ReadOnlyVectorJsonConverter))]
public abstract class ReadOnlyVector : VectorBase, IReadOnlyVector
{
    protected internal ReadOnlyVector(VectorDataType dataType, Type itemType, int length)
        : base(dataType, itemType, length, true)
    {
    }

    public abstract VectorValue GetVectorValue(int index);

    public static ReadOnlyVector<T> Create<T>(T[] array)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(array);

        if (array.Length == 0)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlyVector<T>(array);
    }

    public static ReadOnlyVector<T> Create<T>(ReadOnlyMemory<T> memory)
        where T : IEquatable<T>
    {
        if (memory.IsEmpty)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlyVector<T>(memory);
    }

    public static ReadOnlyVector<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        if (span.IsEmpty)
        {
#pragma warning disable IDE0301 // Simplify collection initialization
            return ReadOnlyVector<T>.Empty;
#pragma warning restore IDE0301 // Simplify collection initialization
        }

        return new ReadOnlyVector<T>(span.ToArray().AsMemory());
    }
}
