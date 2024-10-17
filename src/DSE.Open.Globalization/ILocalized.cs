// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Globalization;

/// <summary>
/// Indicates that something indicates localization requirements.
/// </summary>
public interface ILocalized
{
    /// <summary>
    /// Identifies the language to be displayed in the user interface - for example,
    /// when looking up culture-specific resources and translations.
    /// </summary>
    /// <remarks>
    /// This is equivalent to <see cref="CultureInfo.CurrentUICulture"/> in .NET.
    /// </remarks>
    LanguageTag Language { get; }

    /// <summary>
    /// Identifies the language to be used when formatting numbers and dates.
    /// </summary>
    /// <remarks>
    /// This is equivalent to <see cref="CultureInfo.CurrentCulture"/> in .NET.
    /// </remarks>
    LanguageTag FormatLanguage { get; }
}
