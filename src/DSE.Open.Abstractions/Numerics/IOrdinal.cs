// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// Represents a value that can be compared to other values of the same type
/// on the basis of equality and order, but where the distances between values
/// are not known.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IOrdinal<TSelf, T>
    : INominal<TSelf, T>,
      IComparable<TSelf>,
      IComparisonOperators<TSelf, TSelf, bool>
    where TSelf : IOrdinal<TSelf, T>
{
}
