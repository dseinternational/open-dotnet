// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="LanguageCode2"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class LanguageCode2ToStringConverter : ValueConverter<LanguageCode2, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly LanguageCode2ToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="LanguageCode2ToStringConverter"/>.
    /// </summary>
    public LanguageCode2ToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="LanguageCode2"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(LanguageCode2 code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="LanguageCode2"/>.
    /// </summary>
    /// <param name="code">The stored string value.</param>
    /// <returns>The parsed <see cref="LanguageCode2"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="code"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static LanguageCode2 ConvertFromString(string code)
    {
        if (LanguageCode2.TryParse(code, null, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(LanguageCode2)}");
        return default; // unreachable
    }
}
