// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Time;
using NodaTime;
using NodaTime.TimeZones;
using TimeZoneNames;

namespace DSE.Open.Globalization;

/// <summary>
/// Helpers for working with time zones.
/// </summary>
public static class DateTimeZones
{
    public static DateTimeZone AfricaJohannesburg => DateTimeZoneProviders.Tzdb[TimeZoneIds.AfricaJohannesburg];

    public static DateTimeZone AmericaCancun => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaCancun];

    public static DateTimeZone AmericaLosAngeles => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaLosAngeles];

    public static DateTimeZone AmericaNewYork => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaNewYork];

    public static DateTimeZone AmericaSaoPaulo => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaSaoPaulo];

    public static DateTimeZone AmericaToronto => DateTimeZoneProviders.Tzdb[TimeZoneIds.AmericaToronto];

    public static DateTimeZone AustraliaSydney => DateTimeZoneProviders.Tzdb[TimeZoneIds.AustraliaSydney];

    public static DateTimeZone EuropeBerlin => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeBerlin];

    public static DateTimeZone EuropeBrussels => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeBrussels];

    public static DateTimeZone EuropeDublin => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeDublin];

    public static DateTimeZone EuropeLondon => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeLondon];

    public static DateTimeZone EuropeMadrid => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeMadrid];

    public static DateTimeZone EuropeMoscow => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeMoscow];

    public static DateTimeZone EuropeParis => DateTimeZoneProviders.Tzdb[TimeZoneIds.EuropeParis];

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

    public static ZonedDateTime GetZonedDateTimeForCountry(DateTimeOffset dateTime, CountryCode countryCode)
    {
        var zone = GetTimeZoneForCountry(countryCode);
        return dateTime.ToInstant().InZone(zone);
    }

    public static TimeZoneValues GetTimeZoneNamesForCountry(CountryCode countryCode, LanguageTag languageTag)
        => GetTimeZoneNamesForCountry(countryCode, languageTag.ToString());

    public static TimeZoneValues GetTimeZoneNamesForCountry(CountryCode countryCode, string languageTag)
    {
        var zone = GetTimeZoneForCountry(countryCode);
        return TZNames.GetNamesForTimeZone(zone.Id, languageTag);
    }
}
