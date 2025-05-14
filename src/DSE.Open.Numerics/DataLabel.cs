// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// Specifies a label for a data data.
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TLabel"></typeparam>
public readonly record struct DataLabel<TData, TLabel>
    where TData : IEquatable<TData>
    where TLabel : IEquatable<TLabel>
{
    // note: we may want to support vectors of nullable types in the future
    // so TData is not constrained to notnull

    public DataLabel(TData data, TLabel label)
    {
        Data = data;
        Label = label;
    }

    public TData Data { get; }

    public TLabel Label { get; }

    /// <summary>
    /// Deconstructs the <see cref="DataLabel{TData, TLabel}"/> into its components.
    /// </summary>
    /// <param name="data">The data component.</param>
    /// <param name="label">The label component.</param>
    public void Deconstruct(out TData data, out TLabel label)
    {
        data = Data;
        label = Label;
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static implicit operator DataLabel<TData, TLabel>((TData data, TLabel label) tuple)
    {
        return new(tuple.data, tuple.label);
    }

    public static implicit operator (TData data, TLabel label)(DataLabel<TData, TLabel> dataLabel)
    {
        return (dataLabel.Data, dataLabel.Label);
    }
}
