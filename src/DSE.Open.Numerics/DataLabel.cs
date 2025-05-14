// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// Specifies a label for a data data.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly record struct DataLabel<T> : IDataLabel<T>
    where T : IEquatable<T>
{
    // note: we may want to support vectors of nullable types in the future
    // so T is not constrained to notnull

    public DataLabel(T data, string label)
    {
        Data = data;
        Label = label;
    }

    public T Data { get; }

    public string Label { get; }

    object? IDataLabel.Data => Data;

    /// <summary>
    /// Deconstructs the <see cref="DataLabel{T}"/> into its components.
    /// </summary>
    /// <param name="data">The data component.</param>
    /// <param name="label">The label component.</param>
    public void Deconstruct(out T data, out string label)
    {
        data = Data;
        label = Label;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static implicit operator DataLabel<T>((T data, string label) tuple)
    {
        return new(tuple.data, tuple.label);
    }

    public static implicit operator (T data, string label)(DataLabel<T> dataLabel)
    {
        return (dataLabel.Data, dataLabel.Label);
    }
}
