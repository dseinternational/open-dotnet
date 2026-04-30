// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using NodaTime;

namespace DSE.Open.Time;

/// <summary>
/// Extension methods for NodaTime <see cref="ZonedDateTime"/> values.
/// </summary>
public static class ZonedDateTimeExtensions
{
    /// <summary>
    /// Formats the value as short date and short time followed by the time zone's standard or daylight name and UTC offset,
    /// using <see cref="CultureInfo.CurrentCulture"/>.
    /// </summary>
    public static string ToShortDateTimeWithZoneInfoString(this ZonedDateTime zonedDateTime)
    {
        return zonedDateTime.ToShortDateTimeWithZoneInfoString(null);
    }

    /// <summary>
    /// Formats the value as short date and short time followed by the time zone's standard or daylight name and UTC offset.
    /// </summary>
    /// <param name="zonedDateTime">The zoned date/time to format.</param>
    /// <param name="formatProvider">A culture-specific format provider, or <see langword="null"/> to use <see cref="CultureInfo.CurrentCulture"/>.</param>
    public static string ToShortDateTimeWithZoneInfoString(this ZonedDateTime zonedDateTime, IFormatProvider? formatProvider)
    {
        formatProvider ??= CultureInfo.CurrentCulture;

        var info = TimeZoneInfo.FindSystemTimeZoneById(zonedDateTime.Zone.Id);

        var name = zonedDateTime.IsDaylightSavingTime()
            ? info.DaylightName
            : info.StandardName;

        return zonedDateTime.ToString($"ld<d> lt<t> '{name}' ('UTC' o<m>)", formatProvider);
    }
}
