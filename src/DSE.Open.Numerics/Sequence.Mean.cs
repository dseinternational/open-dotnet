// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    public static TAcc Mean<T, TAcc>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TAcc : struct, INumberBase<TAcc>
    {
        var sum = Sum<T, TAcc>(sequence);
        return sum / TAcc.CreateChecked(sequence.Length);
    }

    public static TAcc Mean<T, TAcc>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TAcc : struct, INumberBase<TAcc>
    {
        // TODO: consider accumulating method to avoid TAcc overflow
        var sum = Sum<T, TAcc>(sequence, out var size);
        return sum / TAcc.CreateChecked(size);
    }
}
