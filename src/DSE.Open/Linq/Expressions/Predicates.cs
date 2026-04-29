// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;

namespace DSE.Open.Linq.Expressions;

/// <summary>
/// Provides factory methods for predicate expressions.
/// </summary>
public static class Predicates
{
    /// <summary>
    /// Creates a predicate expression that tests whether a value falls within the supplied
    /// <see cref="UnboundedRange{T}"/>, or returns <see langword="null"/> when the range
    /// has neither a start nor an end bound.
    /// </summary>
    public static Expression<Func<T, bool>>? CreateRange<T>(UnboundedRange<T> range)
        where T : struct, IEquatable<T>, IComparable<T>
    {
        if (range.Start is not null)
        {
            return range.End is not null
                ? (e => range.Start.Value.CompareTo(e) <= 0 && range.End.Value.CompareTo(e) >= 0)
                : (e => range.Start.Value.CompareTo(e) <= 0);
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
