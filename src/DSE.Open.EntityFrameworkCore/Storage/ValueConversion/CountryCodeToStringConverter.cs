// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="CountryCode"/> to an upper-case <see cref="string"/> for storage.
/// </summary>
public sealed class CountryCodeToStringConverter : ValueConverter<CountryCode, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly CountryCodeToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="CountryCodeToStringConverter"/>.
    /// </summary>
    public CountryCodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="CountryCode"/> to its upper-case <see cref="string"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(CountryCode code)
    {
        return code.ToStringUpper();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="CountryCode"/>.
    /// </summary>
    /// <param name="code">The stored string value.</param>
    /// <returns>The parsed <see cref="CountryCode"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="code"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static CountryCode ConvertFromString(string code)
    {
        if (CountryCode.TryParse(code, out var countryCode))
        {
            return countryCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(CountryCode)}");
        return default; // unreachable
    }
}
