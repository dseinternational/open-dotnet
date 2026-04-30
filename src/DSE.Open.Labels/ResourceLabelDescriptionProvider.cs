// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Labels;

/// <summary>
/// Provides localized labels and descriptions for values of <typeparamref name="T"/> resolved from a
/// <see cref="System.Resources.ResourceManager"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ResourceLabelDescriptionProvider<T> : ResourceLabelProvider<T>, ILocalizedDescriptionProvider<T>
{
    /// <summary>
    /// When overridden in a derived class, returns the resource key used to look up the description for the
    /// specified <paramref name="value"/>.
    /// </summary>
    public abstract string GetDescriptionKey(T value);

    /// <summary>
    /// Returns the description for the specified <paramref name="value"/> using <see cref="CultureInfo.CurrentUICulture"/>.
    /// </summary>
    public virtual string? GetDescription(T value)
    {
        return GetDescription(value, CultureInfo.CurrentUICulture);
    }

    /// <summary>
    /// Returns the description for the specified <paramref name="value"/> from the underlying
    /// <see cref="ResourceLabelProvider{T}.ResourceManager"/> in the given <paramref name="culture"/>.
    /// </summary>
    public virtual string? GetDescription(T value, CultureInfo? culture)
    {
        return ResourceManager.GetString(GetDescriptionKey(value), culture);
    }
}

