// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using DSE.Open.Linq;

namespace DSE.Open.Numerics;

public static partial class Sequence
{
    public static T Max<T>(IEnumerable<T> values)
        where T : struct, INumber<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return Vector.Max(span);
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

                if (x > result)
                {
                    result = x;
                }
            }
        }

        return result;
    }

    public static T MaxFloatingPoint<T>(this IEnumerable<T> values)
        where T : struct, IFloatingPointIeee754<T>
    {
        if (values.TryGetSpan(out var span))
        {
            return Vector.MaxFloatingPoint(span);
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
