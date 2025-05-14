// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

// todo: json converter to output as a dictionary value { "label": data }

/// <summary>
/// Specifies a label for a data data.
/// </summary>
/// <typeparam name="TData"></typeparam>
public readonly record struct DataLabel<TData>
    where TData : IEquatable<TData>
{
    // note: we may want to support vectors of nullable types in the future
    // so TData is not constrained to notnull

    public DataLabel(TData data, string label)
    {
        Data = data;
        Label = label;
    }

    public TData Data { get; }

    public string Label { get; }

    /// <summary>
    /// Deconstructs the <see cref="DataLabel{TData}"/> into its components.
    /// </summary>
    /// <param name="data">The data component.</param>
    /// <param name="label">The label component.</param>
    public void Deconstruct(out TData data, out string label)
    {
        data = Data;
        label = Label;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static implicit operator DataLabel<TData>((TData data, string label) tuple)
    {
        return new(tuple.data, tuple.label);
    }

    public static implicit operator (TData data, string label)(DataLabel<TData> dataLabel)
    {
        return (dataLabel.Data, dataLabel.Label);
    }
}
