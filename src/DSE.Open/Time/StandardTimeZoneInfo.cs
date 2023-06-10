// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Time;

/// <summary>
/// Provides standard time zone information, regardless of platform.
/// </summary>
public class StandardTimeZoneInfo
{
    private StandardTimeZoneInfo(
        string windowsId,
        string displayName,
        string standardName,
        string daylightName,
        string baseUtcOffset,
        bool supportsDaylightSavingTime)
    {
        WindowsId = windowsId;
        DisplayName = displayName;
        StandardName = standardName;
        DaylightName = daylightName;
        BaseUtcOffset = baseUtcOffset;
        SupportsDaylightSavingTime = supportsDaylightSavingTime;

        if (!TimeZoneInfo.TryConvertWindowsIdToIanaId(windowsId, out var ianaId))
        {
            throw new InvalidOperationException("Could not convert Windows id to IANA id: " + windowsId);
        }

        IanaId = ianaId;
    }

    public string WindowsId { get; }

    public string IanaId { get; }

    public string DisplayName { get; }

    public string StandardName { get; }

    public string DaylightName { get; }

    public string BaseUtcOffset { get; }

    public bool SupportsDaylightSavingTime { get; }

    /// <summary>
    /// Gets the system-supplied <see cref="TimeZoneInfo"/>.
    /// </summary>
    /// <returns></returns>
    public TimeZoneInfo GetTimeZoneInfo() => TimeZoneInfo.FindSystemTimeZoneById(WindowsId);

#pragma warning disable CA1024 // Use properties where appropriate
    public static IEnumerable<StandardTimeZoneInfo> GetAll() => s_standardTimeZones;
#pragma warning restore CA1024 // Use properties where appropriate

    private static readonly StandardTimeZoneInfo[] s_standardTimeZones = new[]
    {
        new StandardTimeZoneInfo("Dateline Standard Time", "(UTC-12:00) International Date Line West", "Dateline Standard Time", "Dateline Summer Time", "-12:00:00", false),
        new StandardTimeZoneInfo("UTC-11", "(UTC-11:00) Co-ordinated Universal Time-11", "UTC-11", "UTC-11", "-11:00:00", false),
        new StandardTimeZoneInfo("Aleutian Standard Time", "(UTC-10:00) Aleutian Islands", "Aleutian Standard Time", "Aleutian Summer Time", "-10:00:00", true),
        new StandardTimeZoneInfo("Hawaiian Standard Time", "(UTC-10:00) Hawaii", "Hawaiian Standard Time", "Hawaiian Summer Time", "-10:00:00", false),
        new StandardTimeZoneInfo("Marquesas Standard Time", "(UTC-09:30) Marquesas Islands", "Marquesas Standard Time", "Marquesas Summer Time", "-09:30:00", false),
        new StandardTimeZoneInfo("Alaskan Standard Time", "(UTC-09:00) Alaska", "Alaskan Standard Time", "Alaskan Summer Time", "-09:00:00", true),
        new StandardTimeZoneInfo("UTC-09", "(UTC-09:00) Co-ordinated Universal Time-09", "UTC-09", "UTC-09", "-09:00:00", false),
        new StandardTimeZoneInfo("Pacific Standard Time (Mexico)", "(UTC-08:00) Baja California", "Pacific Standard Time (Mexico)", "Pacific Summer Time (Mexico)", "-08:00:00", true),
        new StandardTimeZoneInfo("UTC-08", "(UTC-08:00) Co-ordinated Universal Time-08", "UTC-08", "UTC-08", "-08:00:00", false),
        new StandardTimeZoneInfo("Pacific Standard Time", "(UTC-08:00) Pacific Time (US & Canada)", "Pacific Standard Time", "Pacific Summer Time", "-08:00:00", true),
        new StandardTimeZoneInfo("US Mountain Standard Time", "(UTC-07:00) Arizona", "US Mountain Standard Time", "US Mountain Summer Time", "-07:00:00", false),
        new StandardTimeZoneInfo("Mountain Standard Time (Mexico)", "(UTC-07:00) Chihuahua, La Paz, Mazatlan", "Mountain Standard Time (Mexico)", "Mountain Summer Time (Mexico)", "-07:00:00", true),
        new StandardTimeZoneInfo("Mountain Standard Time", "(UTC-07:00) Mountain Time (US & Canada)", "Mountain Standard Time", "Mountain Summer Time", "-07:00:00", true),
        new StandardTimeZoneInfo("Yukon Standard Time", "(UTC-07:00) Yukon", "Yukon Standard Time", "Yukon Daylight Time", "-07:00:00", true),
        new StandardTimeZoneInfo("Central America Standard Time", "(UTC-06:00) Central America", "Central America Standard Time", "Central America Summer Time", "-06:00:00", false),
        new StandardTimeZoneInfo("Central Standard Time", "(UTC-06:00) Central Time (US & Canada)", "Central Standard Time", "Central Summer Time", "-06:00:00", true),
        new StandardTimeZoneInfo("Easter Island Standard Time", "(UTC-06:00) Easter Island", "Easter Island Standard Time", "Easter Island Summer Time", "-06:00:00", true),
        new StandardTimeZoneInfo("Central Standard Time (Mexico)", "(UTC-06:00) Guadalajara, Mexico City, Monterrey", "Central Standard Time (Mexico)", "Central Summer Time (Mexico)", "-06:00:00", true),
        new StandardTimeZoneInfo("Canada Central Standard Time", "(UTC-06:00) Saskatchewan", "Canada Central Standard Time", "Canada Central Summer Time", "-06:00:00", false),
        new StandardTimeZoneInfo("SA Pacific Standard Time", "(UTC-05:00) Bogota, Lima, Quito, Rio Branco", "SA Pacific Standard Time", "SA Pacific Summer Time", "-05:00:00", false),
        new StandardTimeZoneInfo("Eastern Standard Time (Mexico)", "(UTC-05:00) Chetumal", "Eastern Standard Time (Mexico)", "Eastern Summer Time (Mexico)", "-05:00:00", true),
        new StandardTimeZoneInfo("Eastern Standard Time", "(UTC-05:00) Eastern Time (US & Canada)", "Eastern Standard Time", "Eastern Summer Time", "-05:00:00", true),
        new StandardTimeZoneInfo("Haiti Standard Time", "(UTC-05:00) Haiti", "Haiti Standard Time", "Haiti Summer Time", "-05:00:00", true),
        new StandardTimeZoneInfo("Cuba Standard Time", "(UTC-05:00) Havana", "Cuba Standard Time", "Cuba Summer Time", "-05:00:00", true),
        new StandardTimeZoneInfo("US Eastern Standard Time", "(UTC-05:00) Indiana (East)", "US Eastern Standard Time", "US Eastern Summer Time", "-05:00:00", true),
        new StandardTimeZoneInfo("Turks And Caicos Standard Time", "(UTC-05:00) Turks and Caicos", "Turks and Caicos Standard Time", "Turks and Caicos Summer Time", "-05:00:00", true),
        new StandardTimeZoneInfo("Paraguay Standard Time", "(UTC-04:00) Asuncion", "Paraguay Standard Time", "Paraguay Summer Time", "-04:00:00", true),
        new StandardTimeZoneInfo("Atlantic Standard Time", "(UTC-04:00) Atlantic Time (Canada)", "Atlantic Standard Time", "Atlantic Summer Time", "-04:00:00", true),
        new StandardTimeZoneInfo("Venezuela Standard Time", "(UTC-04:00) Caracas", "Venezuela Standard Time", "Venezuela Summer Time", "-04:00:00", true),
        new StandardTimeZoneInfo("Central Brazilian Standard Time", "(UTC-04:00) Cuiaba", "Central Brazilian Standard Time", "Central Brazilian Summer Time", "-04:00:00", true),
        new StandardTimeZoneInfo("SA Western Standard Time", "(UTC-04:00) Georgetown, La Paz, Manaus, San Juan", "SA Western Standard Time", "SA Western Summer Time", "-04:00:00", false),
        new StandardTimeZoneInfo("Pacific SA Standard Time", "(UTC-04:00) Santiago", "Pacific SA Standard Time", "Pacific SA Summer Time", "-04:00:00", true),
        new StandardTimeZoneInfo("Newfoundland Standard Time", "(UTC-03:30) Newfoundland", "Newfoundland Standard Time", "Newfoundland Summer Time", "-03:30:00", true),
        new StandardTimeZoneInfo("Tocantins Standard Time", "(UTC-03:00) Araguaina", "Tocantins Standard Time", "Tocantins Summer Time", "-03:00:00", true),
        new StandardTimeZoneInfo("E. South America Standard Time", "(UTC-03:00) Brasilia", "E. South America Standard Time", "E. South America Summer Time", "-03:00:00", true),
        new StandardTimeZoneInfo("SA Eastern Standard Time", "(UTC-03:00) Cayenne, Fortaleza", "SA Eastern Standard Time", "SA Eastern Summer Time", "-03:00:00", false),
        new StandardTimeZoneInfo("Argentina Standard Time", "(UTC-03:00) City of Buenos Aires", "Argentina Standard Time", "Argentina Summer Time", "-03:00:00", true),
        new StandardTimeZoneInfo("Greenland Standard Time", "(UTC-03:00) Greenland", "Greenland Standard Time", "Greenland Summer Time", "-03:00:00", true),
        new StandardTimeZoneInfo("Montevideo Standard Time", "(UTC-03:00) Montevideo", "Montevideo Standard Time", "Montevideo Summer Time", "-03:00:00", true),
        new StandardTimeZoneInfo("Magallanes Standard Time", "(UTC-03:00) Punta Arenas", "Magallanes Standard Time", "Magallanes Daylight Time", "-03:00:00", true),
        new StandardTimeZoneInfo("Saint Pierre Standard Time", "(UTC-03:00) Saint Pierre and Miquelon", "Saint Pierre Standard Time", "Saint Pierre Summer Time", "-03:00:00", true),
        new StandardTimeZoneInfo("Bahia Standard Time", "(UTC-03:00) Salvador", "Bahia Standard Time", "Bahia Summer Time", "-03:00:00", true),
        new StandardTimeZoneInfo("UTC-02", "(UTC-02:00) Co-ordinated Universal Time-02", "UTC-02", "UTC-02", "-02:00:00", false),
        // new StandardTimeZoneInfo("Mid-Atlantic Standard Time", "(UTC-02:00) Mid-Atlantic - Old", "Mid-Atlantic Standard Time", "Mid-Atlantic Summer Time", "-02:00:00", true),
        new StandardTimeZoneInfo("Azores Standard Time", "(UTC-01:00) Azores", "Azores Standard Time", "Azores SummerTime", "-01:00:00", true),
        new StandardTimeZoneInfo("Cape Verde Standard Time", "(UTC-01:00) Cabo Verde Is.", "Cabo Verde Standard Time", "Cabo Verde Summer Time", "-01:00:00", false),
        new StandardTimeZoneInfo("UTC", "(UTC) Coordinated Universal Time", "Coordinated Universal Time", "Coordinated Universal Time", "00:00:00", false),
        new StandardTimeZoneInfo("GMT Standard Time", "(UTC+00:00) Dublin, Edinburgh, Lisbon, London", "GMT Standard Time", "GMT Summer Time", "00:00:00", true),
        new StandardTimeZoneInfo("Greenwich Standard Time", "(UTC+00:00) Monrovia, Reykjavik", "Greenwich Standard Time", "Greenwich Summer Time", "00:00:00", false),
        new StandardTimeZoneInfo("Sao Tome Standard Time", "(UTC+00:00) Sao Tome", "Sao Tome Standard Time", "Sao Tome Daylight Time", "00:00:00", true),
        new StandardTimeZoneInfo("Morocco Standard Time", "(UTC+01:00) Casablanca", "Morocco Standard Time", "Morocco Summer Time", "00:00:00", true),
        new StandardTimeZoneInfo("W. Europe Standard Time", "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna", "W. Europe Standard Time", "W. Europe Summer Time", "01:00:00", true),
        new StandardTimeZoneInfo("Central Europe Standard Time", "(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague", "Central Europe Standard Time", "Central Europe Summer Time", "01:00:00", true),
        new StandardTimeZoneInfo("Romance Standard Time", "(UTC+01:00) Brussels, Copenhagen, Madrid, Paris", "Romance Standard Time", "Romance Summer Time", "01:00:00", true),
        new StandardTimeZoneInfo("Central European Standard Time", "(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb", "Central European Standard Time", "Central European Summer Time", "01:00:00", true),
        new StandardTimeZoneInfo("W. Central Africa Standard Time", "(UTC+01:00) West Central Africa", "W. Central Africa Standard Time", "W. Central Africa Summer Time", "01:00:00", false),
        new StandardTimeZoneInfo("Jordan Standard Time", "(UTC+02:00) Amman", "Jordan Standard Time", "Jordan Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("GTB Standard Time", "(UTC+02:00) Athens, Bucharest", "GTB Standard Time", "GTB Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("Middle East Standard Time", "(UTC+02:00) Beirut", "Middle East Standard Time", "Middle East Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("Egypt Standard Time", "(UTC+02:00) Cairo", "Egypt Standard Time", "Egypt Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("E. Europe Standard Time", "(UTC+02:00) Chisinau", "E. Europe Standard Time", "E. Europe Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("Syria Standard Time", "(UTC+02:00) Damascus", "Syria Standard Time", "Syria Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("West Bank Standard Time", "(UTC+02:00) Gaza, Hebron", "West Bank Gaza Standard Time", "West Bank Gaza Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("South Africa Standard Time", "(UTC+02:00) Harare, Pretoria", "South Africa Standard Time", "South Africa Summer Time", "02:00:00", false),
        new StandardTimeZoneInfo("FLE Standard Time", "(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius", "FLE Standard Time", "FLE Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("Israel Standard Time", "(UTC+02:00) Jerusalem", "Jerusalem Standard Time", "Jerusalem Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("South Sudan Standard Time", "(UTC+02:00) Juba", "South Sudan Standard Time", "South Sudan Daylight Time", "02:00:00", true),
        new StandardTimeZoneInfo("Kaliningrad Standard Time", "(UTC+02:00) Kaliningrad", "Russia TZ 1 Standard Time", "Russia TZ 1 Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("Sudan Standard Time", "(UTC+02:00) Khartoum", "Sudan Standard Time", "Sudan Daylight Time", "02:00:00", true),
        new StandardTimeZoneInfo("Libya Standard Time", "(UTC+02:00) Tripoli", "Libya Standard Time", "Libya Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("Namibia Standard Time", "(UTC+02:00) Windhoek", "Namibia Standard Time", "Namibia Summer Time", "02:00:00", true),
        new StandardTimeZoneInfo("Arabic Standard Time", "(UTC+03:00) Baghdad", "Arabic Standard Time", "Arabic Summer Time", "03:00:00", true),
        new StandardTimeZoneInfo("Turkey Standard Time", "(UTC+03:00) Istanbul", "Turkey Standard Time", "Turkey Summer Time", "03:00:00", true),
        new StandardTimeZoneInfo("Arab Standard Time", "(UTC+03:00) Kuwait, Riyadh", "Arab Standard Time", "Arab Summer Time", "03:00:00", false),
        new StandardTimeZoneInfo("Belarus Standard Time", "(UTC+03:00) Minsk", "Belarus Standard Time", "Belarus Summer Time", "03:00:00", true),
        new StandardTimeZoneInfo("Russian Standard Time", "(UTC+03:00) Moscow, St Petersburg", "Russia TZ 2 Standard Time", "Russia TZ 2 Summer Time", "03:00:00", true),
        new StandardTimeZoneInfo("E. Africa Standard Time", "(UTC+03:00) Nairobi", "E. Africa Standard Time", "E. Africa Summer Time", "03:00:00", false),
        new StandardTimeZoneInfo("Volgograd Standard Time", "(UTC+03:00) Volgograd", "Volgograd Standard Time", "Volgograd Summer Time", "03:00:00", true),
        new StandardTimeZoneInfo("Iran Standard Time", "(UTC+03:30) Tehran", "Iran Standard Time", "Iran Summer Time", "03:30:00", true),
        new StandardTimeZoneInfo("Arabian Standard Time", "(UTC+04:00) Abu Dhabi, Muscat", "Arabian Standard Time", "Arabian Summer Time", "04:00:00", false),
        new StandardTimeZoneInfo("Astrakhan Standard Time", "(UTC+04:00) Astrakhan, Ulyanovsk", "Astrakhan Standard Time", "Astrakhan Summer Time", "04:00:00", true),
        new StandardTimeZoneInfo("Azerbaijan Standard Time", "(UTC+04:00) Baku", "Azerbaijan Standard Time", "Azerbaijan Summer Time", "04:00:00", true),
        new StandardTimeZoneInfo("Russia Time Zone 3", "(UTC+04:00) Izhevsk, Samara", "Russia TZ 3 Standard Time", "Russia TZ 3 Summer Time", "04:00:00", true),
        new StandardTimeZoneInfo("Mauritius Standard Time", "(UTC+04:00) Port Louis", "Mauritius Standard Time", "Mauritius Summer Time", "04:00:00", true),
        new StandardTimeZoneInfo("Saratov Standard Time", "(UTC+04:00) Saratov", "Saratov Standard Time", "Saratov Summer Time", "04:00:00", true),
        new StandardTimeZoneInfo("Georgian Standard Time", "(UTC+04:00) Tbilisi", "Georgian Standard Time", "Georgian Summer Time", "04:00:00", false),
        new StandardTimeZoneInfo("Caucasus Standard Time", "(UTC+04:00) Yerevan", "Caucasus Standard Time", "Caucasus Summer Time", "04:00:00", true),
        new StandardTimeZoneInfo("Afghanistan Standard Time", "(UTC+04:30) Kabul", "Afghanistan Standard Time", "Afghanistan Summer Time", "04:30:00", false),
        new StandardTimeZoneInfo("West Asia Standard Time", "(UTC+05:00) Ashgabat, Tashkent", "West Asia Standard Time", "West Asia Summer Time", "05:00:00", false),
        new StandardTimeZoneInfo("Ekaterinburg Standard Time", "(UTC+05:00) Ekaterinburg", "Russia TZ 4 Standard Time", "Russia TZ 4 Summer Time", "05:00:00", true),
        new StandardTimeZoneInfo("Pakistan Standard Time", "(UTC+05:00) Islamabad, Karachi", "Pakistan Standard Time", "Pakistan Summer Time", "05:00:00", true),
        new StandardTimeZoneInfo("Qyzylorda Standard Time", "(UTC+05:00) Qyzylorda", "Qyzylorda Standard Time", "Qyzylorda Summer Time", "05:00:00", true),
        new StandardTimeZoneInfo("India Standard Time", "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi", "India Standard Time", "India Summer Time", "05:30:00", false),
        new StandardTimeZoneInfo("Sri Lanka Standard Time", "(UTC+05:30) Sri Jayawardenepura", "Sri Lanka Standard Time", "Sri Lanka Summer Time", "05:30:00", false),
        new StandardTimeZoneInfo("Nepal Standard Time", "(UTC+05:45) Kathmandu", "Nepal Standard Time", "Nepal Summer Time", "05:45:00", false),
        new StandardTimeZoneInfo("Central Asia Standard Time", "(UTC+06:00) Astana", "Central Asia Standard Time", "Central Asia Summer Time", "06:00:00", false),
        new StandardTimeZoneInfo("Bangladesh Standard Time", "(UTC+06:00) Dhaka", "Bangladesh Standard Time", "Bangladesh Summer Time", "06:00:00", true),
        new StandardTimeZoneInfo("Omsk Standard Time", "(UTC+06:00) Omsk", "Omsk Standard Time", "Omsk Summer Time", "06:00:00", true),
        new StandardTimeZoneInfo("Myanmar Standard Time", "(UTC+06:30) Yangon (Rangoon)", "Myanmar Standard Time", "Myanmar Summer Time", "06:30:00", false),
        new StandardTimeZoneInfo("SE Asia Standard Time", "(UTC+07:00) Bangkok, Hanoi, Jakarta", "SE Asia Standard Time", "SE Asia Summer Time", "07:00:00", false),
        new StandardTimeZoneInfo("Altai Standard Time", "(UTC+07:00) Barnaul, Gorno-Altaysk", "Altai Standard Time", "Altai Summer Time", "07:00:00", true),
        new StandardTimeZoneInfo("W. Mongolia Standard Time", "(UTC+07:00) Hovd", "W. Mongolia Standard Time", "W. Mongolia Summer Time", "07:00:00", true),
        new StandardTimeZoneInfo("North Asia Standard Time", "(UTC+07:00) Krasnoyarsk", "Russia TZ 6 Standard Time", "Russia TZ 6 Summer Time", "07:00:00", true),
        new StandardTimeZoneInfo("N. Central Asia Standard Time", "(UTC+07:00) Novosibirsk", "Novosibirsk Standard Time", "Novosibirsk Summer Time", "07:00:00", true),
        new StandardTimeZoneInfo("Tomsk Standard Time", "(UTC+07:00) Tomsk", "Tomsk Standard Time", "Tomsk Summer Time", "07:00:00", true),
        new StandardTimeZoneInfo("China Standard Time", "(UTC+08:00) Beijing, Chongqing, Hong Kong SAR, Urumqi", "China Standard Time", "China Summer Time", "08:00:00", false),
        new StandardTimeZoneInfo("North Asia East Standard Time", "(UTC+08:00) Irkutsk", "Russia TZ 7 Standard Time", "Russia TZ 7 Summer Time", "08:00:00", true),
        new StandardTimeZoneInfo("Singapore Standard Time", "(UTC+08:00) Kuala Lumpur, Singapore", "Malay Peninsula Standard Time", "Malay Peninsula Summer Time", "08:00:00", false),
        new StandardTimeZoneInfo("W. Australia Standard Time", "(UTC+08:00) Perth", "W. Australia Standard Time", "W. Australia Summer Time", "08:00:00", true),
        new StandardTimeZoneInfo("Taipei Standard Time", "(UTC+08:00) Taipei", "Taipei Standard Time", "Taipei Summer Time", "08:00:00", false),
        new StandardTimeZoneInfo("Ulaanbaatar Standard Time", "(UTC+08:00) Ulaanbaatar", "Ulaanbaatar Standard Time", "Ulaanbaatar Summer Time", "08:00:00", true),
        new StandardTimeZoneInfo("Aus Central W. Standard Time", "(UTC+08:45) Eucla", "Aus Central W. Standard Time", "Aus Central W. Summer Time", "08:45:00", false),
        new StandardTimeZoneInfo("Transbaikal Standard Time", "(UTC+09:00) Chita", "Transbaikal Standard Time", "Transbaikal Summer Time", "09:00:00", true),
        new StandardTimeZoneInfo("Tokyo Standard Time", "(UTC+09:00) Osaka, Sapporo, Tokyo", "Tokyo Standard Time", "Tokyo Summer Time", "09:00:00", false),
        new StandardTimeZoneInfo("North Korea Standard Time", "(UTC+09:00) Pyongyang", "North Korea Standard Time", "North Korea Summer Time", "09:00:00", true),
        new StandardTimeZoneInfo("Korea Standard Time", "(UTC+09:00) Seoul", "Korea Standard Time", "Korea Summer Time", "09:00:00", false),
        new StandardTimeZoneInfo("Yakutsk Standard Time", "(UTC+09:00) Yakutsk", "Russia TZ 8 Standard Time", "Russia TZ 8 Summer Time", "09:00:00", true),
        new StandardTimeZoneInfo("Cen. Australia Standard Time", "(UTC+09:30) Adelaide", "Cen. Australia Standard Time", "Cen. Australia Summer Time", "09:30:00", true),
        new StandardTimeZoneInfo("AUS Central Standard Time", "(UTC+09:30) Darwin", "AUS Central Standard Time", "AUS Central Summer Time", "09:30:00", false),
        new StandardTimeZoneInfo("E. Australia Standard Time", "(UTC+10:00) Brisbane", "E. Australia Standard Time", "E. Australia Summer Time", "10:00:00", false),
        new StandardTimeZoneInfo("AUS Eastern Standard Time", "(UTC+10:00) Canberra, Melbourne, Sydney", "AUS Eastern Standard Time", "AUS Eastern Summer Time", "10:00:00", true),
        new StandardTimeZoneInfo("West Pacific Standard Time", "(UTC+10:00) Guam, Port Moresby", "West Pacific Standard Time", "West Pacific Summer Time", "10:00:00", false),
        new StandardTimeZoneInfo("Tasmania Standard Time", "(UTC+10:00) Hobart", "Tasmania Standard Time", "Tasmania Summer Time", "10:00:00", true),
        new StandardTimeZoneInfo("Vladivostok Standard Time", "(UTC+10:00) Vladivostok", "Russia TZ 9 Standard Time", "Russia TZ 9 Summer Time", "10:00:00", true),
        new StandardTimeZoneInfo("Lord Howe Standard Time", "(UTC+10:30) Lord Howe Island", "Lord Howe Standard Time", "Lord Howe Summer Time", "10:30:00", true),
        new StandardTimeZoneInfo("Bougainville Standard Time", "(UTC+11:00) Bougainville Island", "Bougainville Standard Time", "Bougainville Summer Time", "11:00:00", true),
        new StandardTimeZoneInfo("Russia Time Zone 10", "(UTC+11:00) Chokurdakh", "Russia TZ 10 Standard Time", "Russia TZ 10 Summer Time", "11:00:00", true),
        new StandardTimeZoneInfo("Magadan Standard Time", "(UTC+11:00) Magadan", "Magadan Standard Time", "Magadan Summer Time", "11:00:00", true),
        new StandardTimeZoneInfo("Norfolk Standard Time", "(UTC+11:00) Norfolk Island", "Norfolk Standard Time", "Norfolk Summer Time", "11:00:00", true),
        new StandardTimeZoneInfo("Sakhalin Standard Time", "(UTC+11:00) Sakhalin", "Sakhalin Standard Time", "Sakhalin Summer Time", "11:00:00", true),
        new StandardTimeZoneInfo("Central Pacific Standard Time", "(UTC+11:00) Solomon Is., New Caledonia", "Central Pacific Standard Time", "Central Pacific Summer Time", "11:00:00", false),
        new StandardTimeZoneInfo("Russia Time Zone 11", "(UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky", "Russia TZ 11 Standard Time", "Russia TZ 11 Summer Time", "12:00:00", true),
        new StandardTimeZoneInfo("New Zealand Standard Time", "(UTC+12:00) Auckland, Wellington", "New Zealand Standard Time", "New Zealand Summer Time", "12:00:00", true),
        new StandardTimeZoneInfo("UTC+12", "(UTC+12:00) Co-ordinated Universal Time+12", "UTC+12", "UTC+12", "12:00:00", false),
        new StandardTimeZoneInfo("Fiji Standard Time", "(UTC+12:00) Fiji", "Fiji Standard Time", "Fiji Summer Time", "12:00:00", true),
        // new StandardTimeZoneInfo("Kamchatka Standard Time", "(UTC+12:00) Petropavlovsk-Kamchatsky - Old", "Kamchatka Standard Time", "Kamchatka Summer Time", "12:00:00", true),
        new StandardTimeZoneInfo("Chatham Islands Standard Time", "(UTC+12:45) Chatham Islands", "Chatham Islands Standard Time", "Chatham Islands Summer Time", "12:45:00", true),
        new StandardTimeZoneInfo("UTC+13", "(UTC+13:00) Co-ordinated Universal Time+13", "UTC+13", "UTC+13", "13:00:00", false),
        new StandardTimeZoneInfo("Tonga Standard Time", "(UTC+13:00) Nuku'alofa", "Tonga Standard Time", "Tonga Summer Time", "13:00:00", true),
        new StandardTimeZoneInfo("Samoa Standard Time", "(UTC+13:00) Samoa", "Samoa Standard Time", "Samoa Summer Time", "13:00:00", true),
        new StandardTimeZoneInfo("Line Islands Standard Time", "(UTC+14:00) Kiritimati Island", "Line Islands Standard Time", "Line Islands Summer Time", "14:00:00", false),
    };
}
