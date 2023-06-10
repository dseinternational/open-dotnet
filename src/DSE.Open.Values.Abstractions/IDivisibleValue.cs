// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Values;

[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface IDivisibleValue<TSelf, T>
    : IAddableValue<TSelf, T>,
      IDivisionOperators<TSelf, TSelf, TSelf>,
      IModulusOperators<TSelf, TSelf, TSelf>,
      IMultiplyOperators<TSelf, TSelf, TSelf>
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
    where TSelf : struct, IDivisibleValue<TSelf, T>
{
    static abstract TSelf Zero { get; }
}
