// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// Represents a value that can be compared to other values of the same type
/// on the basis of equality and order, for which a degree of difference
/// can be determined between values, and which may be divided and multiplied.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IRatio<TSelf, T>
    : IInterval<TSelf, T>,
      IDivisionOperators<TSelf, TSelf, TSelf>,
      IModulusOperators<TSelf, TSelf, TSelf>,
      IMultiplyOperators<TSelf, TSelf, TSelf>
    where TSelf : IRatio<TSelf, T>;
