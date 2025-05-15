// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// Specifies a label for a data value.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly record struct ValueLabel<T> : IValueLabel<T>
    where T : IEquatable<T>
{
    // note: we may want to support vectors of nullable types in the future
    // so T is not constrained to notnull

    public ValueLabel(T value, string label)
    {
        Value = value;
        Label = label;
    }

    public T Value { get; }

    public string Label { get; }

    object? IValueLabel.Value => Value;

    /// <summary>
    /// Deconstructs the <see cref="ValueLabel{T}"/> into its components.
    /// </summary>
    /// <param name="value">The value component.</param>
    /// <param name="label">The label component.</param>
    public void Deconstruct(out T value, out string label)
    {
        value = Value;
        label = Label;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static implicit operator ValueLabel<T>((T value, string label) tuple)
    {
        return new(tuple.value, tuple.label);
    }

    public static implicit operator (T value, string label)(ValueLabel<T> valueLabel)
    {
        return (valueLabel.Value, valueLabel.Label);
    }
}
