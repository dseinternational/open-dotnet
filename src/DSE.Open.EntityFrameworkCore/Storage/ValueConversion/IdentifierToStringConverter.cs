// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Values;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts an <see cref="Identifier"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class IdentifierToStringConverter : ValueConverter<Identifier, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly IdentifierToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="IdentifierToStringConverter"/>.
    /// </summary>
    public IdentifierToStringConverter()
        : base(v => ConvertToString(v), v => ConvertFromString(v))
    {
    }

    /// <summary>
    /// Converts an <see cref="Identifier"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(Identifier value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to an <see cref="Identifier"/>,
    /// parsing using the invariant culture.
    /// </summary>
    /// <param name="value">The stored string value.</param>
    /// <returns>The parsed <see cref="Identifier"/>.</returns>
    // keep public for EF Core compiled models
    public static Identifier ConvertFromString(string value)
    {
        return Identifier.Parse(value, CultureInfo.InvariantCulture);
    }
}
