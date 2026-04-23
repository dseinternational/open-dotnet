// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Numerics;

namespace DSE.Open.Values;

/// <summary>
/// Defines a value type that wraps an underlying equatable value of type <typeparamref name="T"/>
/// and supports equality comparison, span-based formatting and parsing.
/// </summary>
/// <typeparam name="TSelf">The type that implements the interface.</typeparam>
/// <typeparam name="T">The underlying value type being wrapped.</typeparam>
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
