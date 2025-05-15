// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

#pragma warning disable CA1040 // Avoid empty interfaces
public interface IReadOnlyValueLabelCollection
#pragma warning restore CA1040 // Avoid empty interfaces
{
}

public interface IReadOnlyValueLabelCollection<T> : IReadOnlyValueLabelCollection, IReadOnlyCollection<ValueLabel<T>>
    where T : IEquatable<T>
{
    /// <summary>
    /// Gets the label for the specified data value.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    string this[T data] { get; }

    bool TryGetLabel(T value, out string label);

    bool TryGetValue(string label, out T value);
}
