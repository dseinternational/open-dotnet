// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Labels;

/// <summary>
/// Provides localized labels for values of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class LocalizedLabelProvider<T> : ILocalizedLabelProvider<T>
{
    public virtual string? GetLabel(T value)
    {
        return GetLabel(value, CultureInfo.CurrentUICulture);
    }

    public abstract string? GetLabel(T value, CultureInfo? culture);
}

