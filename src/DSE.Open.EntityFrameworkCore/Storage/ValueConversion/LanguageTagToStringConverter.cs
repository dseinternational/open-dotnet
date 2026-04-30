// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="LanguageTag"/> to a formatted <see cref="string"/> for storage.
/// </summary>
public sealed class LanguageTagToStringConverter : ValueConverter<LanguageTag, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly LanguageTagToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="LanguageTagToStringConverter"/>.
    /// </summary>
    public LanguageTagToStringConverter() : base(c => ConvertTo(c), s => ConvertFrom(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="LanguageTag"/> to its formatted <see cref="string"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The formatted string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertTo(LanguageTag code)
    {
        return code.ToStringFormatted();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="LanguageTag"/>.
    /// </summary>
    /// <param name="code">The stored string value.</param>
    /// <returns>The parsed <see cref="LanguageTag"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="code"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static LanguageTag ConvertFrom(string code)
    {
        if (LanguageTag.TryParse(code, out var languageCode))
        {
            return languageCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(LanguageTag)}");
        return default; // unreachable
    }
}
