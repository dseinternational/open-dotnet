// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public interface IReadOnlyDataLabelCollection<TData> : IReadOnlyCollection<DataLabel<TData>>
    where TData : IEquatable<TData>
{
    /// <summary>
    /// Gets the label for the specified data value.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    string this[TData data] { get; }

    bool TryGetLabel(TData data, out string label);

    bool TryGetData(string label, out TData data);
}
