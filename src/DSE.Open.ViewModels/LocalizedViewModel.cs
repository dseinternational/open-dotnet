// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;

namespace DSE.Open.ViewModels;

/// <summary>
/// Base class for a view model that has an identified UI culture.
/// </summary>
/// <remarks>
/// By default, <see cref="Language"/> is initialised to <see cref="CultureInfo.CurrentUICulture"/>
/// and <see cref="FormatLanguage"/> is initialised to <see cref="CultureInfo.CurrentCulture"/>.
/// </remarks>
public class LocalizedViewModel : ViewModel, ILocalizedViewModel
{
    /// <summary>
    /// Initialises a new instance.
    /// </summary>
    /// <remarks>
    /// By default, <see cref="Language"/> is initialised to <see cref="CultureInfo.CurrentUICulture"/>
    /// and <see cref="FormatLanguage"/> is initialised to <see cref="CultureInfo.CurrentCulture"/>.
    /// </remarks>
    public LocalizedViewModel()
    {
        Language = LanguageTag.FromCultureInfo(CultureInfo.CurrentUICulture);
        FormatLanguage = LanguageTag.FromCultureInfo(CultureInfo.CurrentCulture);
    }

    /// <inheritdoc />
    public virtual LanguageTag Language { get; set; }

    /// <inheritdoc />
    public virtual LanguageTag FormatLanguage { get; set; }
}
