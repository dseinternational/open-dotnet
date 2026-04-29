// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DSE.Open.Memory;

#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// Represents a two-dimensional index expressed as a height (row) and width (column).
/// </summary>
public readonly record struct Index2D
{
    /// <summary>
    /// The default <see cref="Index2D"/> with both <see cref="Height"/> and <see cref="Width"/> equal to zero.
    /// </summary>
    public static readonly Index2D Zero;

    /// <summary>
    /// Initializes a new <see cref="Index2D"/> with the specified height (row) and width (column) components.
    /// </summary>
    public Index2D(int height, int width)
    {
        Height = height;
        Width = width;
    }

    /// <summary>
    /// Gets the height (row) component of the index.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the width (column) component of the index.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Deconstructs the index into its <paramref name="height"/> and <paramref name="width"/> components.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out int height, out int width)
    {
        height = Height;
        width = Width;
    }

    /// <summary>
    /// Implicitly converts a <see cref="ValueTuple{Int32, Int32}"/> of (height, width) to an <see cref="Index2D"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Index2D(ValueTuple<int, int> value)
    {
        return new(value.Item1, value.Item2);
    }

    /// <summary>
    /// Implicitly converts an <see cref="Index2D"/> to a <see cref="ValueTuple{Int32, Int32}"/> of (height, width).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValueTuple<int, int>(Index2D value)
    {
        return new(value.Height, value.Width);
    }
}
