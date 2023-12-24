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
public interface IReadOnlyValueSet<TValue, T> : IReadOnlySet<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>;
public interface IValueCollection<TValue, T> : ICollection<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>;
public interface IReadOnlyValueCollection<TValue, T> : IReadOnlyCollection<TValue>
    where T : IEquatable<T>
    where TValue : struct, IValue<TValue, T>;
