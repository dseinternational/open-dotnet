// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

#pragma warning disable CA1040 // Avoid empty interfaces
public interface IReadOnlyDataLabelCollection
#pragma warning restore CA1040 // Avoid empty interfaces
{
}

public interface IReadOnlyDataLabelCollection<TData> : IReadOnlyDataLabelCollection, IReadOnlyCollection<DataLabel<TData>>
    where TData : IEquatable<TData>
{
    /// <summary>
    /// Gets the label for the specified data value.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    string this[TData data] { get; }

    bool TryGetLabel(TData value, out string label);

    bool TryGetDataValue(string label, out TData value);
}
