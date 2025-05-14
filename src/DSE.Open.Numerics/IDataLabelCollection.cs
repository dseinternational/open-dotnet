// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A collection of data labels.
/// </summary>
/// <typeparam name="TData"></typeparam>
public interface IDataLabelCollection<TData> : IReadOnlyDataLabelCollection<TData>, ICollection<DataLabel<TData>>
    where TData : IEquatable<TData>
{
    /// <summary>
    /// Gets or sets the label for the specified data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    new string this[TData data] { get; set; }

    /// <summary>
    /// Adds a label for the specified data.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="label"></param>
    void Add(TData data, string label);
}
