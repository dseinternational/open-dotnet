// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Labels;

/// <summary>
/// Provides labels for values of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ILabelProvider<T>
{
    /// <summary>
    /// Returns the label for the specified <paramref name="value"/>, or <see langword="null"/> if none is available.
    /// </summary>
    string? GetLabel(T value);
}

