// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

#pragma warning disable CA2225 // Operator overloads have named alternates

/// <summary>
/// A pair of zero-based row and column coordinates used to index into a
/// <see cref="NumericMatrix{T}"/> or <see cref="ReadOnlyNumericMatrix{T}"/>.
/// </summary>
public readonly record struct MatrixIndex
{
    /// <summary>The origin (0, 0).</summary>
    public static readonly MatrixIndex Zero;

    /// <summary>Creates a matrix index with the given <paramref name="row"/> and <paramref name="column"/>.</summary>
    public MatrixIndex(int row, int column)
    {
        Row = row;
        Column = column;
    }

    /// <summary>Gets the zero-based row.</summary>
    public int Row { get; }

    /// <summary>Gets the zero-based column.</summary>
    public int Column { get; }

    /// <summary>Deconstructs the index into its row and column components.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out int row, out int column)
    {
        row = Row;
        column = Column;
    }

    /// <summary>Implicitly converts from <see cref="Index2D"/> (binary-compatible).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator MatrixIndex(Index2D value)
    {
        return Unsafe.BitCast<Index2D, MatrixIndex>(value);
    }

    /// <summary>Implicitly converts to <see cref="Index2D"/> (binary-compatible).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Index2D(MatrixIndex value)
    {
        return Unsafe.BitCast<MatrixIndex, Index2D>(value);
    }

    /// <summary>Implicitly converts from a <c>(row, column)</c> tuple.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator MatrixIndex(ValueTuple<int, int> value)
    {
        return Unsafe.BitCast<ValueTuple<int, int>, MatrixIndex>(value);
    }

    /// <summary>Implicitly converts to a <c>(row, column)</c> tuple.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValueTuple<int, int>(MatrixIndex value)
    {
        return Unsafe.BitCast<MatrixIndex, ValueTuple<int, int>>(value);
    }
}
