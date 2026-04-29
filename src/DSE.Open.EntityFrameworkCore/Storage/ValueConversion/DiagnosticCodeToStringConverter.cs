// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Diagnostics.DiagnosticCode"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class DiagnosticCodeToStringConverter : ValueConverter<Diagnostics.DiagnosticCode, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly DiagnosticCodeToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="DiagnosticCodeToStringConverter"/>.
    /// </summary>
    public DiagnosticCodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="Diagnostics.DiagnosticCode"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(Diagnostics.DiagnosticCode code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="Diagnostics.DiagnosticCode"/>.
    /// </summary>
    /// <param name="code">The stored string value.</param>
    /// <returns>The parsed <see cref="Diagnostics.DiagnosticCode"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="code"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static Diagnostics.DiagnosticCode ConvertFromString(string code)
    {
        if (Diagnostics.DiagnosticCode.TryParse(code, out var value))
        {
            return value;
        }

        ValueConversionException.Throw($"Error converting string value '{code}' to DiagnosticCode.", code, null);
        return default;
    }
}
