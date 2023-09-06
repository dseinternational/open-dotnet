﻿// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;

namespace DSE.Open.Labels;

/// <summary>
/// Provides localized labels for values of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class LocalizedLabelDescriptionProvider<T> : LocalizedLabelProvider<T>, ILocalizedDescriptionProvider<T>
{
    public virtual string? GetDescription(T value) => GetDescription(value, CultureInfo.CurrentUICulture);

    public abstract string? GetDescription(T value, CultureInfo? culture);
}
