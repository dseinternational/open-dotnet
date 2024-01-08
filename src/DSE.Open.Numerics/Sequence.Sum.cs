// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using System.Numerics.Tensors;
using DSE.Open.Linq;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>Computes the sum of all elements in the specified tensor of numbers.</summary>
    /// <param name="sequence">The sequence, represented as a span.</param>
    /// <returns>The result of adding all elements in <paramref name="sequence"/>, or zero if
    /// <paramref name="sequence"/> is empty.</returns>
    /// <remarks>
    /// <para>
    /// If any of the values in the input is equal to <see cref="IFloatingPointIeee754{TSelf}.NaN"/>,
    /// the result is also NaN.
    /// </para>
    /// <para>
    /// This method may call into the underlying C runtime or employ instructions specific to the current
    /// architecture. Exact results may differ between different operating systems or architectures.
    /// </para>
    /// </remarks>
    public static T Sum<T>(ReadOnlySpan<T> sequence)
        where T : struct, INumberBase<T>
    {
        return TensorPrimitives.Sum(sequence);
    }

    public static T Sum<T>(IEnumerable<T> sequence, SummationCompensation compensation = default)
        where T : struct, INumberBase<T>
    {
        if (sequence.TryGetSpan(out var span))
        {
            return Sum(span);
        }

        ArgumentNullException.ThrowIfNull(sequence);

        if (sequence is IEnumerable<double> doubleSeq)
        {
            return T.CreateChecked(SumFloatingPointIeee754(doubleSeq, compensation));
        }

        var result = T.AdditiveIdentity;

        foreach (var value in sequence)
        {
            result += value;
        }

        return result;
    }

    private static T SumFloatingPointIeee754<T>(
        IEnumerable<T> sequence,
        SummationCompensation summation = SummationCompensation.None)
        where T : struct, IFloatingPointIeee754<T>
    {
        if (summation == SummationCompensation.KahanBabushkaNeumaier)
        {
            return SumFloatingPointIeee754KahanBabushkaNeumaier(sequence);
        }

        var result = T.AdditiveIdentity;

        foreach (var value in sequence)
        {
            if (T.IsNaN(value))
            {
                return T.NaN;
            }

            result += value;
        }

        return result;
    }

    /*
function KahanBabushkaNeumaierSum(input)
    var sum = 0.0
    var c = 0.0                       // A running compensation for lost low-order bits.

    for i = 1 to input.length do
        var t = sum + input[i]
        if |sum| >= |input[i]| then
            c += (sum - t) + input[i] // If sum is bigger, low-order digits of input[i] are lost.
        else
            c += (input[i] - t) + sum // Else low-order digits of sum are lost.
        endif
        sum = t
    next i

    return sum + c                    // Correction only applied once in the very end.
     */

    private static T SumFloatingPointIeee754KahanBabushkaNeumaier<T>(IEnumerable<T> sequence)
        where T : struct, IFloatingPointIeee754<T>
    {
        var result = T.AdditiveIdentity;

        var c = T.AdditiveIdentity;

        foreach (var value in sequence)
        {
            if (T.IsNaN(value))
            {
                return T.NaN;
            }

            var t = result + value;

            if (T.Abs(result) >= T.Abs(value))
            {
                c += result - t + value;
            }
            else
            {
                c += value - t + result;
            }

            result = t;
        }

        return result + c;

    }
    public static TAcc Sum<T, TAcc>(ReadOnlySpan<T> values)
        where T : struct, INumber<T>
        where TAcc : struct, INumber<TAcc>
    {
        var acc = TAcc.AdditiveIdentity;

        foreach (var value in values)
        {
            checked
            {
                acc += TAcc.CreateChecked(value);
            }
        }

        return acc;
    }
}
