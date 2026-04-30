// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Time;
using NodaTime;
using NodaTime.TimeZones;

namespace DSE.Open.Globalization;

/// <summary>
/// Helpers for working with time zones.
/// </summary>
public static class DateTimeZones
{
    /// <summary>Gets the Africa/Johannesburg time zone.</summary>
    public static DateTimeZone AfricaJohannesburg => DateTimeZoneProviders.Tzdb[TimeZoneIds.AfricaJohannesburg];

    /// <summary>Gets the America/Cancun time zone.</summary>
    public static DateTimeZone AmericaCancun => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaCancun];

    /// <summary>Gets the America/Los_Angeles time zone.</summary>
    public static DateTimeZone AmericaLosAngeles => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaLosAngeles];

    /// <summary>Gets the America/New_York time zone.</summary>
    public static DateTimeZone AmericaNewYork => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaNewYork];

    /// <summary>Gets the America/Sao_Paulo time zone.</summary>
    public static DateTimeZone AmericaSaoPaulo => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaSaoPaulo];

    /// <summary>Gets the America/Toronto time zone.</summary>
    public static DateTimeZone AmericaToronto => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaToronto];

    /// <summary>Gets the Australia/Sydney time zone.</summary>
    public static DateTimeZone AustraliaSydney => DateTimeZoneProviders.Tzdb[TimeZoneIds.AustraliaSydney];

    /// <summary>Gets the Europe/Berlin time zone.</summary>
    public static DateTimeZone EuropeBerlin => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeBerlin];

    /// <summary>Gets the Europe/Brussels time zone.</summary>
    public static DateTimeZone EuropeBrussels => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeBrussels];

    /// <summary>Gets the Europe/Dublin time zone.</summary>
    public static DateTimeZone EuropeDublin => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeDublin];

    /// <summary>Gets the Europe/London time zone.</summary>
    public static DateTimeZone EuropeLondon => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeLondon];

    /// <summary>Gets the Europe/Madrid time zone.</summary>
    public static DateTimeZone EuropeMadrid => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeMadrid];

    /// <summary>Gets the Europe/Moscow time zone.</summary>
    public static DateTimeZone EuropeMoscow => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeMoscow];

    /// <summary>Gets the Europe/Paris time zone.</summary>
    public static DateTimeZone EuropeParis => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeParis];

    /// <summary>Gets the Pacific/Auckland time zone.</summary>
    public static DateTimeZone PacificAuckland => DateTimeZoneProviders.Tzdb[TimeZoneIds.PacificAuckland];

    /// <summary>
    /// Gets the time zone for the given country. Where a country includes more than one time zone,
    /// the most common/best option is returned.
    /// </summary>
    /// <param name="countryCode">The code identifying the country.</param>
    /// <returns>A <see cref="DateTimeZone"/> for the country.</returns>
    /// <exception cref="InvalidOperationException">Time zone data is not available.</exception>
    public static DateTimeZone GetTimeZoneForCountry(CountryCode countryCode)
    {
        var zoneLocations = TzdbDateTimeZoneSource.Default.ZoneLocations;

        if (zoneLocations is null)
        {
            throw new InvalidOperationException("No ZoneLocations available.");
        }

        DateTimeZone zone;

        if (countryCode == CountryCode.UnitedStates)
        {
            zone = AmericaNewYork;
        }
        else if (countryCode == CountryCode.UnitedKingdom)
        {
            zone = EuropeLondon;
        }
        else if (countryCode == CountryCode.Ireland)
        {
            zone = EuropeDublin;
        }
        else if (countryCode == CountryCode.Canada)
        {
            zone = AmericaToronto;
        }
        else if (countryCode == CountryCode.Australia)
        {
            zone = AustraliaSydney;
        }
        else if (countryCode == CountryCode.NewZealand)
        {
            zone = PacificAuckland;
        }
        else if (countryCode == CountryCode.SouthAfrica)
        {
            zone = AfricaJohannesburg;
        }
        else if (countryCode == CountryCode.France)
        {
            zone = EuropeParis;
        }
        else if (countryCode == CountryCode.Denmark)
        {
            zone = EuropeBerlin;
        }
        else if (countryCode == CountryCode.Brazil)
        {
            zone = AmericaSaoPaulo;
        }
        else if (countryCode == CountryCode.Mexico)
        {
            zone = AmericaCancun;
        }
        else if (countryCode == CountryCode.Russia)
        {
            zone = EuropeMoscow;
        }
        else
        {
            var location = zoneLocations.FirstOrDefault(l => countryCode.Equals(l.CountryCode));

            zone = location == null ? EuropeLondon : DateTimeZoneProviders.Tzdb[location.ZoneId];
        }

        return zone;
    }

    /// <summary>
    /// Returns the specified <see cref="DateTimeOffset"/> as a <see cref="ZonedDateTime"/> in
    /// the time zone associated with the specified country.
    /// </summary>
    /// <param name="dateTime">The value to convert.</param>
    /// <param name="countryCode">The country whose time zone to use.</param>
    public static ZonedDateTime GetZonedDateTimeForCountry(DateTimeOffset dateTime, CountryCode countryCode)
    {
        var zone = GetTimeZoneForCountry(countryCode);
        return dateTime.ToInstant().InZone(zone);
    }
}
