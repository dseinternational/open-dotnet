// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;
using DSE.Open.Numerics;

namespace DSE.Open.Values;

/// <summary>
/// Encapsulates/constrains a <see cref="IOrdinal{TSelf, T}"/> value.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IOrdinalValue<TSelf, T>
    : INominalValue<TSelf, T>,
      IOrdinal<TSelf, T>
    where T
    : IEquatable<T>,
      IComparable<T>,
      IEqualityOperators<T, T, bool>,
      ISpanFormattable,
      ISpanParsable<T>
    where TSelf : struct, IOrdinalValue<TSelf, T>
{
}