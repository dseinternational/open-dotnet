// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;

namespace DSE.Open.Linq.Expressions;

public static class Predicates
{
    public static Expression<Func<T, bool>>? CreateRange<T>(UnboundedRange<T> range)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        if (range.Start is not null)
        {
            return range.End is not null
                ? (e => range.Start.Value.CompareTo(e) <= 0 && range.End.Value.CompareTo(e) >= 0)
                : (Expression<Func<T, bool>>?)(e => range.Start.Value.CompareTo(e) <= 0);
        }
        else
        {
            if (range.End is not null)
            {
                return e => range.End.Value.CompareTo(e) >= 0;
            }
        }

        return null;
    }
}
