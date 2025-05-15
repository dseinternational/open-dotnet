// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A collection of data labels.
/// </summary>
/// <typeparam name="TData"></typeparam>
public interface IValueLabelCollection<TData> : IReadOnlyValueLabelCollection<TData>, ICollection<ValueLabel<TData>>
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
    /// <param name="value"></param>
    /// <param name="label"></param>
    void Add(TData value, string label);
}
