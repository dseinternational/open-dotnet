// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Localization.Resources;

/// <summary>
/// Provides localized string and stream resources, selected by culture.
/// </summary>
public interface ILocalizedResourceProvider
{
    /// <summary>
    /// The default culture used when looking up a resource if no culture is supplied
    /// to <see cref="GetString"/> or <see cref="GetStream"/>.
    /// </summary>
    CultureInfo LookupCulture { get; }

    /// <summary>
    /// Returns the localized string resource with the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The resource key.</param>
    /// <param name="cultureInfo">
    /// The culture to use for the lookup; if <see langword="null"/>, <see cref="LookupCulture"/>
    /// is used.
    /// </param>
    /// <returns>The localized string value.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null, empty, or whitespace.</exception>
    /// <exception cref="ResourceNotFoundException">No resource with the given name exists for the resolved culture.</exception>
    string GetString(string name, CultureInfo? cultureInfo = null);

    /// <summary>
    /// Returns the localized binary stream resource with the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The resource key.</param>
    /// <param name="cultureInfo">
    /// The culture to use for the lookup; if <see langword="null"/>, <see cref="LookupCulture"/>
    /// is used.
    /// </param>
    /// <returns>A stream over the resource bytes. The caller owns the stream.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null, empty, or whitespace.</exception>
    /// <exception cref="ResourceNotFoundException">No resource with the given name exists for the resolved culture.</exception>
    Stream GetStream(string name, CultureInfo? cultureInfo = null);
}
