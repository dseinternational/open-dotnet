// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Resources;

namespace DSE.Open.Labels;

public abstract class ResourceLabelDescriptionProvider<T> : ResourceLabelProvider<T>, ILocalizedDescriptionProvider<T>
{
    public abstract string GetDescriptionKey(T value);

    public virtual string? GetDescription(T value) => GetDescription(value, CultureInfo.CurrentUICulture);

    public virtual string? GetDescription(T value, CultureInfo? culture)
    {
        return ResourceManager.GetString(GetDescriptionKey(value), culture);
    }
}

