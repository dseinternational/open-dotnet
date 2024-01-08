// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Linq;
using System.Numerics.Tensors;
using System.Numerics;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    public static T Maximum<T>(ReadOnlySpan<T> values)
        where T : INumber<T>
    {
        return TensorPrimitives.Max(values);
    }

    public static T Maximum<T>(IEnumerable<T> values)
        where T : struct, INumber<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return Maximum(span);
        }

        ArgumentNullException.ThrowIfNull(values);

        if (values is IEnumerable<double> doubleSeq)
        {
            return T.CreateChecked(MaximumFloatingPointIeee754(doubleSeq));
        }

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

                if (x > result)
                {
                    result = x;
                }
            }
        }

        return result;
    }

    private static T MaximumFloatingPointIeee754<T>(this IEnumerable<T> values)
        where T : struct, IFloatingPointIeee754<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return Maximum(span);
        }

        ArgumentNullException.ThrowIfNull(values);

        var result = T.NegativeInfinity;

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

                if (x > result)
                {
                    result = x;
                }
            }
        }

        return result;
    }
}
