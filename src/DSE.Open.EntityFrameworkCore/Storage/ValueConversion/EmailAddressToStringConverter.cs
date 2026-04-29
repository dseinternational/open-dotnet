// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="EmailAddress"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class EmailAddressToStringConverter : ValueConverter<EmailAddress, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly EmailAddressToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="EmailAddressToStringConverter"/>.
    /// </summary>
    public EmailAddressToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts an <see cref="EmailAddress"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(EmailAddress value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to an <see cref="EmailAddress"/>.
    /// </summary>
    /// <param name="value">The stored string value.</param>
    /// <returns>The parsed <see cref="EmailAddress"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="value"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static EmailAddress ConvertFromString(string value)
    {
        if (EmailAddress.TryParse(value, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Error converting string value '{value}' to EmailAddress.", value, null);
        return default;
    }
}
