// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Runtime.CompilerServices;
using DSE.Open.Memory;

namespace DSE.Open.Numerics;

#pragma warning disable CA2225 // Operator overloads have named alternates

public readonly record struct MatrixIndex
{
    public static readonly MatrixIndex Zero;

    public MatrixIndex(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public int Row { get; }

    public int Column { get; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out int row, out int column)
    {
        row = Row;
        column = Column;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator MatrixIndex(Index2D value)
    {
        return Unsafe.BitCast<Index2D, MatrixIndex>(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Index2D(MatrixIndex value)
    {
        return Unsafe.BitCast<MatrixIndex, Index2D>(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator MatrixIndex(ValueTuple<int, int> value)
    {
        return Unsafe.BitCast<ValueTuple<int, int>, MatrixIndex>(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValueTuple<int, int>(MatrixIndex value)
    {
        return Unsafe.BitCast<MatrixIndex, ValueTuple<int, int>>(value);
    }
}
