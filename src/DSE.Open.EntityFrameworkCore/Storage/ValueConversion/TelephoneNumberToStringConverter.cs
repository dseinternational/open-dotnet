// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="TelephoneNumber"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class TelephoneNumberToStringConverter : ValueConverter<TelephoneNumber, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TelephoneNumberToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TelephoneNumberToStringConverter"/>.
    /// </summary>
    public TelephoneNumberToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="TelephoneNumber"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="number">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(TelephoneNumber number)
    {
        return number.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="TelephoneNumber"/>.
    /// </summary>
    /// <param name="value">The stored string value.</param>
    /// <returns>The parsed <see cref="TelephoneNumber"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="value"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static TelephoneNumber ConvertFromString(string value)
    {
        if (TelephoneNumber.TryParse(value, out var number))
        {
            return number;
        }

        ValueConversionException.Throw($"Could not convert string '{value}' to {nameof(TelephoneNumber)}", value, null);
        return default; // unreachable
    }
}
