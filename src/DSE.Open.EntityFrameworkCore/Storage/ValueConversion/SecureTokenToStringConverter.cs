// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Security;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DSE.Open.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Converts a <see cref="SecureToken"/> to a <see cref="string"/> for storage.
/// </summary>
public sealed class SecureTokenToStringConverter : ValueConverter<SecureToken, string>
{
    /// <summary>
    /// A shared default instance.
    /// </summary>
    public static readonly SecureTokenToStringConverter Default = new();

    /// <summary>
    /// Initializes a new instance of <see cref="SecureTokenToStringConverter"/>.
    /// </summary>
    public SecureTokenToStringConverter()
        : base(v => ConvertToString(v), v => ConvertFromString(v))
    {
    }

    /// <summary>
    /// Converts a <see cref="SecureToken"/> to its <see cref="string"/> storage form.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation.</returns>
    // keep public for EF Core compiled models
    public static string ConvertToString(SecureToken value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Converts a <see cref="string"/> storage value back to a <see cref="SecureToken"/>,
    /// parsing using the invariant culture.
    /// </summary>
    /// <param name="value">The stored string value.</param>
    /// <returns>The parsed <see cref="SecureToken"/>.</returns>
    // keep public for EF Core compiled models
    public static SecureToken ConvertFromString(string value)
    {
        return SecureToken.Parse(value, CultureInfo.InvariantCulture);
    }
}
