// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Numerics;

/// <summary>
/// Represents a value that can be compared to other values of the same type
/// on the basis of equality. Numbers may be used to represent the values but
/// the numbers do not have numerical value or relationship.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
/// <remarks>Examples of nominal values include category labels, such as gender or
/// parts of speech.</remarks>
public interface INominal<TSelf, T>
    : IEquatable<TSelf>,
      IEqualityOperators<TSelf, TSelf, bool>
    where TSelf : INominal<TSelf, T>
{
}
