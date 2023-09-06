// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Labels;

/// <summary>
/// Provides labels for values of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ILabelProvider<T>
{
    string? GetLabel(T value);
}
