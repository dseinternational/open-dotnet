// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Values;

/// <summary>
/// Defines a value type that wraps an underlying value of type <typeparamref name="T"/>
/// and supports division, multiplication and modulus operations in addition to the
/// operations defined by <see cref="IAddableValue{TSelf, T}"/>.
/// </summary>
/// <typeparam name="TSelf">The type that implements the interface.</typeparam>
/// <typeparam name="T">The underlying value type being wrapped.</typeparam>
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
    /// <summary>Gets the zero value for this type.</summary>
    static abstract TSelf Zero { get; }
}
