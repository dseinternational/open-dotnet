// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace DSE.Open.Values;

/// <summary>
/// Defines a value that can be converted to a corresponding value of type <typeparamref name="T"/>
/// and may be convertible from some or all values of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="TSelf">The type that implements the interface.</typeparam>
/// <typeparam name="T">The type that the value that can be converted to.</typeparam>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Required for static interface methods")]
public interface IValue<TSelf, T>
    : IConvertibleTo<TSelf, T>,
        ITryConvertibleFrom<TSelf, T>,
        IEquatable<TSelf>,
        IEqualityOperators<TSelf, TSelf, bool>,
        ISpanSerializable<TSelf>
    where T : IEquatable<T>
    where TSelf : struct, IValue<TSelf, T>
{
    static abstract bool IsValidValue(T value);
}
