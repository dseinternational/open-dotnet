// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>
    /// Return the sample arithmetic mean of a sequence. If the sequence is empty, an
    /// <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequence"></param>
    /// <returns></returns>
    public static T Mean<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Mean<T, T>(sequence);
    }

    public static TAcc Mean<T, TAcc>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
        where TAcc : struct, INumberBase<TAcc>
    {
        EmptySequenceException.ThrowIfEmpty(sequence);
        var sum = Sum<T, TAcc>(sequence);
        return sum / TAcc.CreateChecked(sequence.Length);
    }

    /// <summary>
    /// Return the sample arithmetic mean of a sequence. If the sequence is empty, an
    /// <see cref="EmptySequenceException"/> is thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequence"></param>
    /// <returns></returns>
    public static T Mean<T>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
    {
        return Mean<T, T>(sequence);
    }

    public static TAcc Mean<T, TAcc>(IEnumerable<T> sequence)
        where T : struct, INumberBase<T>
        where TAcc : struct, INumberBase<TAcc>
    {
        // TODO: consider accumulating method to avoid TAcc overflow
        var sum = Sum<T, TAcc>(sequence, out var size);

        if (size == 0)
        {
            EmptySequenceException.Throw();
        }

        return sum / TAcc.CreateChecked(size);
    }
}
