// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Values;

/// <summary>
/// Encapsulates/constrains a <see cref="IComparable{TSelf}"/> value.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IComparableValue<TSelf, T>
    : IEquatableValue<TSelf, T>,
      IComparable<TSelf>,
      IComparisonOperators<TSelf, TSelf, bool>
    where T
    : IEquatable<T>,
      IComparable<T>,
      IEqualityOperators<T, T, bool>,
      ISpanFormattable,
      ISpanParsable<T>
    where TSelf : struct, IComparableValue<TSelf, T>
{
}
