// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using NodaTime;

namespace DSE.Open.Time;

public static class ZonedDateTimeExtensions
{
    public static string ToShortDateTimeWithZoneInfoString(this ZonedDateTime zonedDateTime)
        => zonedDateTime.ToShortDateTimeWithZoneInfoString(null);

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
