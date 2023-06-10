// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Time;
using NodaTime;

namespace DSE.Open.Globalization.Tests;

public class DateTimeZonesTests
{
    [Theory]
    [InlineData("GB", TimeZoneIds.EuropeLondon)]
    [InlineData("US", TimeZoneIds.AmericaNewYork)]
    [InlineData("CA", TimeZoneIds.AmericaToronto)]
    [InlineData("AU", TimeZoneIds.AustraliaSydney)]
    [InlineData("NZ", TimeZoneIds.PacificAuckland)]
    [InlineData("IE", TimeZoneIds.EuropeDublin)]
    [InlineData("DK", TimeZoneIds.EuropeBerlin)]
    [InlineData("BE", TimeZoneIds.EuropeBrussels)]
    [InlineData("HK", TimeZoneIds.AsiaHongKong)]
    public void GetTimeZoneForCountryReturnsExpectedTimeZone(string countryCode, string timeZoneId)
        => Assert.Equal(DateTimeZoneProviders.Tzdb[timeZoneId], DateTimeZones.GetTimeZoneForCountry(CountryCode.Parse(countryCode)));

    [Theory]
    [InlineData("GB", "en-GB", "United Kingdom Time")]
    public void GetTimeZoneNamesForCountryReturnsExpectedNames(string countryCode, string languageCode, string generic)
    {
        var names = DateTimeZones.GetTimeZoneNamesForCountry(CountryCode.Parse(countryCode), languageCode);
        Assert.Equal(generic, names.Generic);
    }
}
