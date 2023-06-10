// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// Represents a value that can be compared to other values of the same type
/// on the basis of equality and order, and for which a degree of difference
/// can be determined between values.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IInterval<TSelf, T>
    : IOrdinal<TSelf, T>,
      IAdditionOperators<TSelf, TSelf, TSelf>,
      IDecrementOperators<TSelf>,
      IIncrementOperators<TSelf>,
      ISubtractionOperators<TSelf, TSelf, TSelf>,
      IUnaryPlusOperators<TSelf, TSelf>,
      IUnaryNegationOperators<TSelf, TSelf>
    where TSelf : IInterval<TSelf, T>
{
}
