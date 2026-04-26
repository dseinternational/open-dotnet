// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Linq;
using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    /// <summary>Returns the minimum value in <paramref name="values"/> using the type's natural ordering.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="values"/> is empty.</exception>
    public static T Min<T>(IEnumerable<T> values)
        where T : struct, INumber<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return VectorPrimitives.Min(span);
        }

        ArgumentNullException.ThrowIfNull(values);

        T result;

        using (var e = values.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                ThrowHelper.ThrowInvalidOperationException();
            }

            result = e.Current;

            while (e.MoveNext())
            {
                var x = e.Current;

                if (x < result)
                {
                    result = x;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Returns the minimum value in <paramref name="values"/>, propagating NaN
    /// to the result when any element is NaN. Returns <see cref="IFloatingPointIeee754{TSelf}.NaN"/>
    /// when the sequence is empty.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
    public static T MinFloatingPoint<T>(this IEnumerable<T> values)
        where T : struct, IFloatingPointIeee754<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return VectorPrimitives.Min(span);
        }

        ArgumentNullException.ThrowIfNull(values);

        var result = T.PositiveInfinity;

        using (var e = values.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                return T.NaN;
            }

            result = e.Current;

            while (e.MoveNext())
            {
                var x = e.Current;

                if (T.IsNaN(x))
                {
                    return T.NaN;
                }

                if (x < result)
                {
                    result = x;
                }
            }
        }

        return result;
    }
}
