// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Resources;

namespace DSE.Open.Localization.Resources;

/// <summary>
/// Provides access to packaged localized resources.
/// </summary>
public abstract class PackagedLocalizedResourceProvider() : ILocalizedResourceProvider
{
    protected abstract ResourceManager ResourceManager { get; }

    [AllowNull]
    [field: AllowNull, MaybeNull]
    public virtual CultureInfo PresentationCulture
    {
        get => field ?? CultureInfo.CurrentCulture;
        protected set;
    }

    [AllowNull]
    [field: AllowNull, MaybeNull]
    public virtual CultureInfo LookupCulture
    {
        get => field ?? CultureInfo.CurrentUICulture;
        protected set;
    }

    public string GetFormattedString(string name, params ReadOnlySpan<object?> args)
    {
        return GetFormattedString(name, null, args);
    }

    public string GetFormattedString(string name, CultureInfo? cultureInfo, params ReadOnlySpan<object?> args)
    {
        return string.Format(cultureInfo ?? PresentationCulture, GetString(name, cultureInfo), args);
    }

    public string GetString(string name, CultureInfo? cultureInfo = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        cultureInfo ??= LookupCulture;

        var value = ResourceManager.GetString(name, cultureInfo);

        if (value is not null)
        {
            return value;
        }

        ResourceNotFoundException.Throw(name);
        return null!;
    }

    public Stream GetStream(string name, CultureInfo? cultureInfo = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        cultureInfo ??= LookupCulture;

        var value = ResourceManager.GetStream(name, cultureInfo);

        if (value is not null)
        {
            return value;
        }

        ResourceNotFoundException.Throw(name);
        return null!;
    }
}
