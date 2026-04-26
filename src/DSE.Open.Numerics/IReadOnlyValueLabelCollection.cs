// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// Type-erased marker interface implemented by both
/// <see cref="ValueLabelCollection"/> and <see cref="ReadOnlyValueLabelCollection"/>.
/// Lets callers receive either kind without knowing the element type.
/// </summary>
#pragma warning disable CA1040 // Avoid empty interfaces
public interface IReadOnlyValueLabelCollection
#pragma warning restore CA1040 // Avoid empty interfaces
{
}

/// <summary>
/// A read-only collection of value-label pairs.
/// </summary>
/// <typeparam name="T">The value type.</typeparam>
public interface IReadOnlyValueLabelCollection<T> : IReadOnlyValueLabelCollection, IReadOnlyCollection<ValueLabel<T>>
    where T : IEquatable<T>
{
    /// <summary>
    /// Gets the label for the specified data value.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    string this[T data] { get; }

    /// <summary>Tries to find the label for <paramref name="value"/>.</summary>
    bool TryGetLabel(T value, out string label);

    /// <summary>Tries to find the value registered with <paramref name="label"/>.</summary>
    bool TryGetValue(string label, out T value);
}
