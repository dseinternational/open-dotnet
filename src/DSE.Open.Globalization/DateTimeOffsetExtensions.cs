// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using NodaTime;

namespace DSE.Open.Globalization;

/// <summary>
/// Extension methods for <see cref="DateTimeOffset"/>.
/// </summary>
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Converts the specified <see cref="DateTimeOffset"/> to a <see cref="ZonedDateTime"/> in
    /// the time zone associated with the specified country.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="countryCode">The country whose time zone to use.</param>
    public static ZonedDateTime ToZonedDateTime(this DateTimeOffset value, CountryCode countryCode)
    {
        return value.ToZonedDateTime(DateTimeZones.GetTimeZoneForCountry(countryCode));
    }
}
