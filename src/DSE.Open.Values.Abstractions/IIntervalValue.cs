// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using DSE.Open.Numerics;

namespace DSE.Open.Values;

public interface IIntervalValue<TSelf, T>
    : IOrdinalValue<TSelf, T>,
      IInterval<TSelf, T>
    where T
    : IAdditionOperators<T, T, T>,
      IDecrementOperators<T>,
      IComparable<T>,
      IComparisonOperators<T, T, bool>,
      IEqualityOperators<T, T, bool>,
      IEquatable<T>,
      IIncrementOperators<T>,
      ISpanFormattable,
      ISpanParsable<T>,
      ISubtractionOperators<T, T, T>,
      IUnaryPlusOperators<T, T>,
      IUnaryNegationOperators<T, T>
    where TSelf : struct, IIntervalValue<TSelf, T>
{
}