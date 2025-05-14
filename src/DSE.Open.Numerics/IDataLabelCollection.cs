// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A collection of data labels.
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TLabel"></typeparam>
public interface IDataLabelCollection<TData, TLabel> : ICollection<DataLabel<TData, TLabel>>
    where TData : IEquatable<TData>
    where TLabel : IEquatable<TLabel>
{
    /// <summary>
    /// Gets or sets the label for the specified data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    TLabel this[TData data] { get; set; }

    /// <summary>
    /// Adds a label for the specified data.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="label"></param>
    void Add(TData data, TLabel label);
}
