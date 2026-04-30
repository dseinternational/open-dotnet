// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="AlphaCode"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class AlphaCodeToStringConverter : ValueConverter<AlphaCode, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly AlphaCodeToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="AlphaCodeToStringConverter"/>.
    /// </summary>
    public AlphaCodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts an <see cref="AlphaCode"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(AlphaCode code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to an <see cref="AlphaCode"/>.
    /// </summary>
    /// <param name="code">The stored string value.</param>
    /// <returns>The parsed <see cref="AlphaCode"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="code"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static AlphaCode ConvertFromString(string code)
    {
        if (AlphaCode.TryParse(code, out var alphaCode))
        {
            return alphaCode;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(AlphaCode)}");
        return default; // unreachable
    }
}
