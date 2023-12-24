// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Values;

public interface IEquatableValue<TSelf, T>
    : IValue<TSelf, T>,
      IEquatable<TSelf>,
      IEqualityOperators<TSelf, TSelf, bool>
    where T
    : IEquatable<T>,
      IEqualityOperators<T, T, bool>,
      ISpanFormattable,
      ISpanParsable<T>
    where TSelf : struct, IEquatableValue<TSelf, T>;
