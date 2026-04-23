// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Resources;

namespace DSE.Open.Localization.Resources;

/// <summary>
/// Base class for <see cref="ILocalizedResourceProvider"/> implementations that read
/// resources from a <see cref="System.Resources.ResourceManager"/> (typically the packaged
/// resources that ship with an assembly).
/// </summary>
/// <remarks>
/// Derived classes supply the <see cref="ResourceManager"/> over which lookups run.
/// The source generator in <c>DSE.Open.Localization.Generators</c> uses
/// <see cref="ResourceProviderAttribute"/> to emit concrete subclasses automatically.
/// </remarks>
public abstract class PackagedLocalizedResourceProvider() : ILocalizedResourceProvider
{
    /// <summary>
    /// The underlying <see cref="System.Resources.ResourceManager"/> that supplies resources.
    /// </summary>
    protected abstract ResourceManager ResourceManager { get; }

    /// <summary>
    /// The culture used to format interpolated values in <see cref="GetFormattedString(string, ReadOnlySpan{object?})"/>.
    /// Defaults to <see cref="CultureInfo.CurrentCulture"/> when no value has been set.
    /// </summary>
    [AllowNull]
    [field: AllowNull, MaybeNull]
    public virtual CultureInfo PresentationCulture
    {
        get => field ?? CultureInfo.CurrentCulture;
        protected set;
    }

    /// <summary>
    /// The culture used when looking up a resource key if no explicit culture is supplied.
    /// Defaults to <see cref="CultureInfo.CurrentUICulture"/> when no value has been set.
    /// </summary>
    [AllowNull]
    [field: AllowNull, MaybeNull]
    public virtual CultureInfo LookupCulture
    {
        get => field ?? CultureInfo.CurrentUICulture;
        protected set;
    }

    /// <summary>
    /// Looks up the string resource with the given <paramref name="name"/> (using
    /// <see cref="LookupCulture"/>) and formats it against <paramref name="args"/> using
    /// <see cref="PresentationCulture"/>.
    /// </summary>
    public string GetFormattedString(string name, params ReadOnlySpan<object?> args)
    {
        return GetFormattedString(name, null, args);
    }

    /// <summary>
    /// Looks up the string resource with the given <paramref name="name"/> and formats it
    /// against <paramref name="args"/> using <paramref name="cultureInfo"/>.
    /// </summary>
    /// <param name="name">The resource key.</param>
    /// <param name="cultureInfo">
    /// Culture used both for the resource lookup and for formatting the arguments.
    /// If <see langword="null"/>, the resource is looked up using <see cref="LookupCulture"/>
    /// and arguments are formatted using <see cref="PresentationCulture"/>.
    /// </param>
    /// <param name="args">The arguments to substitute into the format string.</param>
    public string GetFormattedString(string name, CultureInfo? cultureInfo, params ReadOnlySpan<object?> args)
    {
        return string.Format(cultureInfo ?? PresentationCulture, GetString(name, cultureInfo), args);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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
