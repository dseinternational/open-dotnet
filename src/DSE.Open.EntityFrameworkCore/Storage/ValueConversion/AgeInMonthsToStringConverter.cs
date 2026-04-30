// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="AgeInMonths"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class AgeInMonthsToStringConverter : ValueConverter<AgeInMonths, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly AgeInMonthsToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="AgeInMonthsToStringConverter"/>.
    /// </summary>
    public AgeInMonthsToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts an <see cref="AgeInMonths"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(AgeInMonths code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to an <see cref="AgeInMonths"/>.
    /// </summary>
    /// <param name="code">The stored string value.</param>
    /// <returns>The parsed <see cref="AgeInMonths"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="code"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static AgeInMonths ConvertFromString(string code)
    {
        if (AgeInMonths.TryParse(code, out var age))
        {
            return age;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(AgeInMonths)}");
        return default; // unreachable
    }
}
