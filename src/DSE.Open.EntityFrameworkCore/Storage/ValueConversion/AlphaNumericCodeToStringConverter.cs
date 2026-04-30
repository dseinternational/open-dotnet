// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="AlphaNumericCode"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class AlphaNumericCodeToStringConverter : ValueConverter<AlphaNumericCode, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly AlphaNumericCodeToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="AlphaNumericCodeToStringConverter"/>.
    /// </summary>
    public AlphaNumericCodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts an <see cref="AlphaNumericCode"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(AlphaNumericCode code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to an <see cref="AlphaNumericCode"/>.
    /// </summary>
    /// <param name="code">The stored string value.</param>
    /// <returns>The parsed <see cref="AlphaNumericCode"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="code"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static AlphaNumericCode ConvertFromString(string code)
    {
        if (AlphaNumericCode.TryParse(code, out var value))
        {
            return value;
        }

        ValueConversionException.Throw($"Error converting string value '{code}' to AlphaNumericCode.", code, null);
        return default;
    }
}
