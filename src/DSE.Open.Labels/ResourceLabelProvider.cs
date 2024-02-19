// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Resources;
using Microsoft.Extensions.Localization;

namespace DSE.Open.Labels;

/// <summary>
/// Provides localized labels for values of <typeparamref name="T"/> from a <see cref="ResourceManager"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ResourceLabelProvider<T> : LocalizedLabelProvider<T>, IStringLocalizer
{
    public abstract ResourceManager ResourceManager { get; }

    public abstract string GetLabelKey(T value);

    public override string? GetLabel(T value, CultureInfo? culture)
    {
        return ResourceManager.GetString(GetLabelKey(value), culture);
    }

    LocalizedString IStringLocalizer.this[string name]
    {
#pragma warning disable CA1033 // Interface methods should be callable by child types
        get
#pragma warning restore CA1033 // Interface methods should be callable by child types
        {
            Guard.IsNotNull(name);
            var value = ResourceManager.GetString(name, null);
            return new LocalizedString(name, value ?? name, resourceNotFound: value is null, searchedLocation: ResourceManager.BaseName);
        }
    }

    LocalizedString IStringLocalizer.this[string name, params object[] arguments]
    {
#pragma warning disable CA1033 // Interface methods should be callable by child types
        get
#pragma warning restore CA1033 // Interface methods should be callable by child types
        {
            Guard.IsNotNull(name);
            var format = ResourceManager.GetString(name, null);
            var value = string.Format(CultureInfo.CurrentCulture, format ?? name, arguments);
            return new LocalizedString(name, value, resourceNotFound: format is null, searchedLocation: ResourceManager.BaseName);
        }
    }

    // TODO
    // See: https://github.com/dotnet/aspnetcore/blob/main/src/Localization/Localization/src/ResourceManagerStringLocalizer.cs
    IEnumerable<LocalizedString> IStringLocalizer.GetAllStrings(bool includeParentCultures)
    {
        throw new NotImplementedException();
    }
}

