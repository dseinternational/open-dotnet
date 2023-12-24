// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using NodaTime;

namespace DSE.Open.Globalization;

public static class DateTimeOffsetExtensions
{
    public static ZonedDateTime ToZonedDateTime(this DateTimeOffset value, CountryCode countryCode)
    {
        return value.ToZonedDateTime(DateTimeZones.GetTimeZoneForCountry(countryCode));
    }
}
