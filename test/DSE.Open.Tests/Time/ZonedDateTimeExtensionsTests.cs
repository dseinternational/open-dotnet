// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using DSE.Open.Time;
using NodaTime;

namespace DSE.Open.Tests.Time;

public class ZonedDateTimeExtensionsTests
{
    public ZonedDateTimeExtensionsTests(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    [Theory(Skip = "Results differ on Windows/Linux")]
    [InlineData("en-GB", "America/New_York", "27/10/2022 07:07 Eastern Summer Time (UTC -04:00)")]
    [InlineData("en-GB", "Asia/Tokyo", "27/10/2022 20:07 Tokyo Standard Time (UTC +09:00)")]
    [InlineData("en-GB", "Pacific/Auckland", "28/10/2022 00:07 New Zealand Summer Time (UTC +13:00)")]
    [InlineData("en-US", "America/New_York", "10/27/2022 7:07 AM Eastern Summer Time (UTC -04:00)")]
    [InlineData("en-US", "Asia/Tokyo", "10/27/2022 8:07 PM Tokyo Standard Time (UTC +09:00)")]
    [InlineData("en-US", "Pacific/Auckland", "10/28/2022 12:07 AM New Zealand Summer Time (UTC +13:00)")]
    //[InlineData("de-DE", "America/New_York", "27.10.2022 07:07 Nordamerikanische Ostküsten-Sommerzeit (UTC -04:00)")]
    //[InlineData("de-DE", "Asia/Tokyo", "27.10.2022 20:07 Japanische Normalzeit (UTC +09:00)")]
    //[InlineData("de-DE", "Pacific/Auckland", "28.10.2022 00:07 Neuseeland-Sommerzeit (UTC +13:00)")]
    //[InlineData("fr-FR", "America/New_York", "27/10/2022 07:07 heure d’été de l’Est nord-américain (UTC -04:00)")]
    //[InlineData("fr-FR", "Asia/Tokyo", "27/10/2022 20:07 heure normale du Japon (UTC +09:00)")]
    //[InlineData("fr-FR", "Pacific/Auckland", "28/10/2022 00:07 heure d’été de la Nouvelle-Zélande (UTC +13:00)")]
    public void ToShortDateTimeWithZoneInfoStringReturnsExpectedString(string language, string timneZoneId, string expected)
    {
        var dateTimeOffset = new DateTimeOffset(2022, 10, 27, 12, 07, 22, TimeSpan.FromHours(1));
        var instant = Instant.FromDateTimeOffset(dateTimeOffset);
        var timeZone = DateTimeZoneProviders.Tzdb[timneZoneId];
        var zonedNow = instant.InZone(timeZone);
        var result = zonedNow.ToShortDateTimeWithZoneInfoString(CultureInfo.GetCultureInfo(language));

        Output.WriteLine(result);

        Assert.Equal(expected, result);
    }
}
