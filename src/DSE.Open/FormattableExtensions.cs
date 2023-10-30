// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class FormattableExtensions
{
    public static string ToString<T>(this T formattable, string? format) where T : IFormattable
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(format, null);
    }

    public static string ToString<T>(this T formattable, IFormatProvider formatProvider) where T : IFormattable
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(null, formatProvider);
    }

    public static string ToStringInvariant<T>(this T formattable) where T : IFormattable
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(null, CultureInfo.InvariantCulture);
    }

    public static string ToStringInvariant<T>(this T formattable, string? format) where T : IFormattable
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(format, CultureInfo.InvariantCulture);
    }
}
