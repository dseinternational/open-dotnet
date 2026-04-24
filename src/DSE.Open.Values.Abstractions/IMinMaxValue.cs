// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Values;

/// <summary>
/// Defines a value type that has well-defined minimum and maximum values.
/// </summary>
/// <typeparam name="TSelf">The type that implements the interface.</typeparam>
/// <typeparam name="T">The underlying value type, which itself must define min/max values.</typeparam>
public interface IMinMaxValue<TSelf, T> : IEquatable<TSelf>, IEqualityOperators<TSelf, TSelf, bool>
    where T : IEquatable<T>, IMinMaxValue<T>
    where TSelf : struct, IMinMaxValue<TSelf, T>
{
    /// <summary>Gets the minimum representable value of this type.</summary>
    static abstract TSelf MinValue { get; }

    /// <summary>Gets the maximum representable value of this type.</summary>
    static abstract TSelf MaxValue { get; }
}
