// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Text.Json.Serialization;
using DSE.Open.Numerics.Serialization;

namespace DSE.Open.Numerics;

/// <summary>
/// A serializable, fixed-length, contiguous sequence of values.
/// </summary>
/// <remarks>
/// <see cref="Vector"/> is the type-erased base for the strongly-typed
/// <see cref="Vector{T}"/>. Most callers should work with the generic form;
/// the non-generic base exists primarily for serialization and runtime
/// dispatch through <see cref="VectorDataType"/>. Type and dtype tables are
/// provided by <see cref="GetDataType{T}()"/>, <see cref="TryGetDataType(Type, out VectorDataType)"/>,
/// and friends.
/// </remarks>
[JsonConverter(typeof(VectorJsonConverter))]
public abstract partial class Vector : VectorBase, IVector
{
    /// <summary>
    /// Initializes a new vector base with the supplied dtype, runtime element
    /// type and length. Used by derived <see cref="Vector{T}"/>.
    /// </summary>
    /// <param name="dataType">The <see cref="VectorDataType"/> tag for the element type.</param>
    /// <param name="itemType">The runtime <see cref="Type"/> of the element.</param>
    /// <param name="length">The number of elements in the vector.</param>
    protected internal Vector(VectorDataType dataType, Type itemType, int length)
        : base(dataType, itemType, length, false)
    {
    }

    /// <summary>
    /// Returns a read-only view of this vector. Implemented by derived types to
    /// produce the corresponding <see cref="ReadOnlyVector{T}"/>.
    /// </summary>
    protected abstract ReadOnlyVector CreateReadOnly();

    /// <summary>
    /// Returns the element at <paramref name="index"/> boxed into a
    /// type-erased <see cref="VectorValue"/>.
    /// </summary>
    /// <param name="index">Zero-based index of the element to read.</param>
    public abstract VectorValue GetVectorValue(int index);

    /// <summary>
    /// Returns a read-only view of this vector. The view shares the underlying
    /// storage; mutations of the source vector are observable through it.
    /// </summary>
    public ReadOnlyVector AsReadOnly()
    {
        return CreateReadOnly();
    }

    // -------- Factory methods --------

    /// <summary>
    /// Creates a vector that wraps the supplied <see cref="Memory{T}"/>. The
    /// memory is taken by reference — no copy is made.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="memory">The backing memory.</param>
    /// <returns>A <see cref="Vector{T}"/> over <paramref name="memory"/>, or the
    /// shared empty instance when the memory is empty.</returns>
    public static Vector<T> Create<T>(Memory<T> memory)
        where T : IEquatable<T>
    {
        if (memory.IsEmpty)
        {
            return [];
        }

        return new Vector<T>(memory);
    }

    /// <summary>
    /// Creates a vector that wraps the supplied <paramref name="array"/>. The
    /// array is taken by reference — no copy is made.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="array">The backing array.</param>
    /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    /// Creates a vector by copying the contents of <paramref name="span"/> into
    /// a freshly-allocated array.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="span">The span whose contents will be copied.</param>
    public static Vector<T> Create<T>(ReadOnlySpan<T> span)
        where T : IEquatable<T>
    {
        if (span.IsEmpty)
        {
            return Vector<T>.Empty;
        }

        return new Vector<T>(span.ToArray().AsMemory());
    }

    /// <summary>
    /// Creates a vector of the given <paramref name="length"/> with all
    /// elements initialised to <c>default(T)</c>.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="length">The number of elements in the new vector.</param>
    public static Vector<T> Create<T>(int length)
        where T : IEquatable<T>
    {
        return [.. new T[length]];
    }

    /// <summary>
    /// Creates a vector of the given <paramref name="length"/> backed by an
    /// uninitialised array — contents are unspecified until written. Useful as
    /// a destination for primitive operations that overwrite every element.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="length">The number of elements in the new vector.</param>
    public static Vector<T> CreateUninitialized<T>(int length)
        where T : IEquatable<T>
    {
        return [.. GC.AllocateUninitializedArray<T>(length)];
    }

    /// <summary>
    /// Creates a vector of the given length, with all elements set to the given scalar value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="length"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public static Vector<T> Create<T>(int length, T scalar)
        where T : IEquatable<T>
    {
        var array = GC.AllocateUninitializedArray<T>(length);
        array.AsSpan().Fill(scalar);
        return [.. array];
    }

    /// <summary>
    /// Creates a vector of the given length, with all elements set to <see cref="INumberBase{TSelf}.Zero"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector<T> CreateZeroes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.Zero);
    }

    /// <summary>
    /// Creates a vector of the given length, with all elements set to <see cref="INumberBase{TSelf}.One"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector<T> CreateOnes<T>(int length)
        where T : struct, INumber<T>
    {
        return Create(length, T.One);
    }
}
