// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

/// <summary>
/// A collection of data labels.
/// </summary>
/// <typeparam name="T">The type of data associated with labels in the collection.</typeparam>
public interface IValueLabelCollection<T> : IReadOnlyValueLabelCollection<T>, ICollection<ValueLabel<T>>
    where T : IEquatable<T>
{
    /// <summary>
    /// Gets or sets the label for the specified data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    new string this[T data] { get; set; }

    /// <summary>
    /// Adds a label for the specified data.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="label"></param>
    void Add(T value, string label);
}
