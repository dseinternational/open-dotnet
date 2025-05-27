// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Numerics;

public static class ReadOnlyValueLabelCollectionExtensions
{
    /// <summary>
    /// Creates a mutable copy of the specified <paramref name="valueLabels"/>.
    /// </summary>
    /// <param name="valueLabels">The labels to copy.</param>
    public static ValueLabelCollection<T> Copied<T>(this ReadOnlyValueLabelCollection<T> valueLabels)
        where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(valueLabels);
        return new ValueLabelCollection<T>(valueLabels);
    }
}
