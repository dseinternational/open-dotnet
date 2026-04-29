// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="Code"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class CodeToStringConverter : ValueConverter<Code, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly CodeToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="CodeToStringConverter"/>.
    /// </summary>
    public CodeToStringConverter()
        : base(c => ConvertToString(c), s => ConvertFromString(s))
    {
    }

    /// <summary>
    /// Converts a <see cref="Code"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="code">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(Code code)
    {
        return code.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="Code"/>.
    /// </summary>
    /// <param name="code">The stored string value.</param>
    /// <returns>The parsed <see cref="Code"/>.</returns>
    /// <exception cref="ValueConversionException">Thrown when <paramref name="code"/> cannot be parsed.</exception>
    // keep public for EF Core compiled models
    public static Code ConvertFromString(string code)
    {
        if (Code.TryParse(code, null, out var result))
        {
            return result;
        }

        ValueConversionException.Throw($"Could not convert string '{code}' to {nameof(Code)}");
        return default; // unreachable
    }
}
