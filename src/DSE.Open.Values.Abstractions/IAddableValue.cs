// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Values;

public interface IAddableValue<TSelf, T>
    : IComparableValue<TSelf, T>,
      IAdditionOperators<TSelf, TSelf, TSelf>,
      IDecrementOperators<TSelf>,
      IIncrementOperators<TSelf>,
      ISubtractionOperators<TSelf, TSelf, TSelf>,
      IUnaryPlusOperators<TSelf, TSelf>,
      IUnaryNegationOperators<TSelf, TSelf>
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
    where TSelf : struct, IAddableValue<TSelf, T>;
