// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;

namespace DSE.Open;

public static class FormattableExtensions
{
    public static string ToString<T>(this T formattable, string format)
        where T : IFormattable
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(format, null);
    }

    public static string ToString<T>(this T formattable, IFormatProvider formatProvider)
        where T : IFormattable
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(null, formatProvider);
    }

    public static string ToString<T>(this T? formattable, string format)
        where T : struct, IFormattable
        => formattable?.ToString(format, null) ?? string.Empty;

    public static string ToString<T>(this T? formattable, IFormatProvider formatProvider)
        where T : struct, IFormattable
        => formattable?.ToString(null, formatProvider) ?? string.Empty;

    public static string ToStringInvariant<T>(this T formattable)
        where T : IFormattable
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(null, CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant<T>(this T formattable, string? format)
        where T : IFormattable
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(format, CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant<T>(this T? formattable)
        where T : struct, IFormattable
        => formattable?.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty;

    public static string ToStringInvariant<T>(this T? formattable, string format)
        where T : struct, IFormattable
        => formattable?.ToString(format, CultureInfo.InvariantCulture) ?? string.Empty;
}
