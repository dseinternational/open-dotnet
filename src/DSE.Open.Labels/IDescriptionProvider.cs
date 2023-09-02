// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Labels;

/// <summary>
/// Provides descriptions for values of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDescriptionProvider<T>
{
    string? GetDescription(T value);
}

