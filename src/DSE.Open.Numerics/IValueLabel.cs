// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A label-and-value pair, type-erased. The non-generic interface exists so that
/// <see cref="IReadOnlyValueLabelCollection"/> can iterate without knowing the
/// element type at compile time.
/// </summary>
public interface IValueLabel
{
    /// <summary>Gets the boxed value.</summary>
    object? Value { get; }

    /// <summary>Gets the label.</summary>
    string Label { get; }
}

/// <summary>
/// A label-and-value pair where the value is strongly typed.
/// </summary>
/// <typeparam name="T">The value type.</typeparam>
public interface IValueLabel<T> : IValueLabel
    where T : IEquatable<T>
{
    /// <summary>Gets the value.</summary>
    new T Value { get; }
}
