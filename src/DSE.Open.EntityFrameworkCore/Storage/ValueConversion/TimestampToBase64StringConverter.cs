// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Timestamp"/> to a Base64-encoded <see cref="string"/> for storage.
/// </summary>
public sealed class TimestampToBase64StringConverter : ValueConverter<Timestamp, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly TimestampToBase64StringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="TimestampToBase64StringConverter"/>.
    /// </summary>
    public TimestampToBase64StringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="Timestamp"/> to its Base64-encoded <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The Base64 string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(Timestamp value)
    {
        return value.ToBase64String();
    }

    /// <summary>
    /// Converts a Base64-encoded <see cref="string"/> storage value back to a <see cref="Timestamp"/>.
    /// </summary>
    /// <param name="value">The stored Base64 string value.</param>
    /// <returns>The parsed <see cref="Timestamp"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="value"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static Timestamp ConvertFromString(string value)
    {
        if (Timestamp.TryParse(value, null, out var timestamp))
        {
            return timestamp;
        }

        ValueConversionException.Throw($"Could not convert string '{value}' to {nameof(Timestamp)}");
        return default; // unreachable
    }
}
