// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open;

public static class FormattableExtensions
{
    public static string ToString(this IFormattable formattable, string? format)
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(format, null);
    }

    public static string ToString(this IFormattable formattable, IFormatProvider formatProvider)
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(null, formatProvider);
    }

    public static string ToStringInvariant(this IFormattable formattable)
    {
        Guard.IsNotNull(formattable);
        return formattable.ToStringInvariant();
    }

    public static string ToStringInvariant(this IFormattable formattable, string? format)
    {
        Guard.IsNotNull(formattable);
        return formattable.ToString(format, CultureInfo.InvariantCulture);
    }
}
