// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Values;

/// <summary>
/// Represents a finite set of values.
/// </summary>
/// <typeparam name="T">The type of the converted/conistrained values.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface IValueSet<TValue, T> : ISet<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>;

/// <summary>
/// Represents a finite, read-only set of values.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <typeparam name="T">The type of the converted/constrained values.</typeparam>
public interface IReadOnlyValueSet<TValue, T> : IReadOnlySet<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>;

/// <summary>
/// Represents a collection of values.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <typeparam name="T">The type of the converted/constrained values.</typeparam>
public interface IValueCollection<TValue, T> : ICollection<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>;

/// <summary>
/// Represents a read-only collection of values.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
/// <typeparam name="T">The type of the converted/constrained values.</typeparam>
public interface IReadOnlyValueCollection<TValue, T> : IReadOnlyCollection<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>;
