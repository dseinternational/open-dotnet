// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

/// <summary>
/// Extensions over <see cref="IFormattable"/>.
/// </summary>
public static class FormattableExtensions
{
    /// <summary>
    /// Formats <paramref name="formattable"/> using the specified <paramref name="format"/> and the
    /// current culture's format provider.
    /// </summary>
    public static string ToString<T>(this T formattable, string? format) where T : IFormattable
    {
        ArgumentNullException.ThrowIfNull(formattable);
        return formattable.ToString(format, null);
    }

    /// <summary>
    /// Formats <paramref name="formattable"/> using the default format and the specified
    /// <paramref name="formatProvider"/>.
    /// </summary>
    public static string ToString<T>(this T formattable, IFormatProvider formatProvider) where T : IFormattable
    {
        ArgumentNullException.ThrowIfNull(formattable);
        return formattable.ToString(null, formatProvider);
    }

    /// <summary>
    /// Formats <paramref name="formattable"/> using the default format and the invariant culture.
    /// </summary>
    public static string ToStringInvariant<T>(this T formattable) where T : IFormattable
    {
        ArgumentNullException.ThrowIfNull(formattable);
        return formattable.ToString(null, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Formats <paramref name="formattable"/> using the specified <paramref name="format"/> and the
    /// invariant culture.
    /// </summary>
    public static string ToStringInvariant<T>(this T formattable, string? format) where T : IFormattable
    {
        ArgumentNullException.ThrowIfNull(formattable);
        return formattable.ToString(format, CultureInfo.InvariantCulture);
    }
}
