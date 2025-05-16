// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;

namespace DSE.Open.Numerics;

public static class ValueLabel
{
    /// <summary>
    /// Creates a new <see cref="ValueLabel{T}"/> from the specified value and label.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="label">The label.</param>
    /// <returns>A new <see cref="ValueLabel{T}"/> instance.</returns>
    public static ValueLabel<T> Create<T>(T value, string label)
        where T : IEquatable<T>
    {
        return new(value, label);
    }
}

/// <summary>
/// Specifies a label for a data value.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly record struct ValueLabel<T> : IValueLabel<T>
    where T : IEquatable<T>
{
    // note: we may want to support vectors of nullable types in the future
    // so T is not constrained to notnull

    [JsonConstructor]
    public ValueLabel(T value, string label)
    {
        Value = value;
        Label = label;
    }

    [JsonPropertyName("value")]
    public T Value { get; }

    [JsonPropertyName("label")]
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
