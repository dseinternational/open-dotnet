// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DSE.Open.Memory;

#pragma warning disable CA2225 // Operator overloads have named alternates

public readonly record struct Index2D
{
    public static readonly Index2D Zero;

    public Index2D(int height, int width)
    {
        Height = height;
        Width = width;
    }

    public int Height { get; }

    public int Width { get; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out int height, out int width)
    {
        height = Height;
        width = Width;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Index2D(ValueTuple<int, int> value)
    {
        return new(value.Item1, value.Item1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ValueTuple<int, int>(Index2D value)
    {
        return new(value.Height, value.Width);
    }
}
