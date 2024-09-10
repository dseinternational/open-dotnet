// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open.Globalization;

[StructLayout(LayoutKind.Sequential)]
public sealed class CountryCallingCodeInfo
{
    public static readonly CountryCallingCodeInfo? Empty;

    private CountryCallingCodeInfo(uint code, string name)
    {
        Code = code;
        Name = name;
    }

    public uint Code { get; }

    public string Name { get; }

    public static bool IsAssignedCode(uint code)
    {
        return code <= 999u && Array.BinarySearch(AssignedCodes, code) > -1;
    }

    public static IEnumerable<CountryCallingCodeInfo> GetInfo()
    {
        return CachedInfo;
    }

    public static IEnumerable<CountryCallingCodeInfo> GetInfo(uint code)
    {
        if (s_cachedLookups.TryGetValue(code, out var cached))
        {
            return cached;
        }

        lock (s_cacheLock)
        {
            if (s_cachedLookups.TryGetValue(code, out var cached2))
            {
                return cached2;
            }

            var result = CachedInfo.Where(info => info.Code == code).ToArray();

            s_cachedLookups.Add(code, result);

            return result;
        }
    }

    private static readonly Dictionary<uint, CountryCallingCodeInfo[]> s_cachedLookups = new();
    private static readonly object s_cacheLock = new();

    // internal for testing
    internal static readonly CountryCallingCodeInfo[] CachedInfo =
    [
        new(1, "American Samoa"),
        new(1, "Anguilla"),
        new(1, "Antigua and Barbuda"),
        new(1, "Bahamas"),
        new(1, "Barbados"),
        new(1, "Bermuda"),
        new(1, "British Virgin Islands"),
        new(1, "Canada"),
        new(1, "Cayman Islands"),
        new(1, "Dominica"),
        new(1, "Dominican Republic"),
        new(1, "Grenada"),
        new(1, "Guam"),
        new(1, "Jamaica"),
        new(1, "Montserrat"),
        new(1, "Northern Mariana Islands"),
        new(1, "Puerto Rico"),
        new(1, "Saint Kitts and Nevis"),
        new(1, "Saint Lucia"),
        new(1, "Saint Vincent and the Grenadines"),
        new(1, "Sint Maarten"),
        new(1, "Trinidad and Tobago"),
        new(1, "Turks and Caicos Islands"),
        new(1, "United States of America"),
        new(1, "United States Virgin Islands"),
        new(7, "Kazakhstan"),
        new(7, "Russian Federation"),
        new(20, "Egypt"),
        new(27, "South Africa"),
        new(30, "Greece"),
        new(31, "Netherlands"),
        new(32, "Belgium"),
        new(33, "France"),
        new(34, "Spain"),
        new(36, "Hungary"),
        new(39, "Italy"),
        new(39, "Vatican City State"),
        new(40, "Romania"),
        new(41, "Switzerland"),
        new(43, "Austria"),
        new(44, "United Kingdom"),
        new(45, "Denmark"),
        new(46, "Sweden"),
        new(47, "Norway"),
        new(48, "Poland"),
        new(49, "Germany"),
        new(51, "Peru"),
        new(52, "Mexico"),
        new(53, "Cuba"),
        new(54, "Argentine Republic"),
        new(55, "Brazil"),
        new(56, "Chile"),
        new(57, "Colombia"),
        new(58, "Venezuela"),
        new(60, "Malaysia"),
        new(61, "Australia"),
        new(62, "Indonesia"),
        new(63, "Philippines"),
        new(64, "New Zealand"),
        new(65, "Singapore"),
        new(66, "Thailand"),
        new(81, "Japan"),
        new(82, "Korea"),
        new(84, "Viet Nam"),
        new(86, "China"),
        new(90, "Turkey"),
        new(91, "India"),
        new(92, "Pakistan"),
        new(93, "Afghanistan"),
        new(94, "Sri Lanka"),
        new(95, "Myanmar"),
        new(98, "Iran"),
        new(211, "South Sudan"),
        new(212, "Morocco"),
        new(213, "Algeria"),
        new(216, "Tunisia"),
        new(218, "Libya"),
        new(220, "Gambia"),
        new(221, "Senegal"),
        new(222, "Mauritania"),
        new(223, "Mali"),
        new(224, "Guinea"),
        new(225, "Côte d'Ivoire"),
        new(226, "Burkina Faso"),
        new(227, "Niger"),
        new(228, "Togolese Republic"),
        new(229, "Benin"),
        new(230, "Mauritius"),
        new(231, "Liberia"),
        new(232, "Sierra Leone"),
        new(233, "Ghana"),
        new(234, "Nigeria"),
        new(235, "Chad"),
        new(236, "Central African Republic"),
        new(237, "Cameroon"),
        new(238, "Cape Verde"),
        new(239, "Sao Tome and Principe"),
        new(240, "Equatorial Guinea"),
        new(241, "Gabonese Republic"),
        new(242, "Congo"),
        new(243, "Democratic Republic of the Congo"),
        new(244, "Angola"),
        new(245, "Guinea-Bissau"),
        new(246, "Diego Garcia"),
        new(247, "Saint Helena, Ascension and Tristan da Cunha"),
        new(248, "Seychelles"),
        new(249, "Sudan"),
        new(250, "Rwanda"),
        new(251, "Ethiopia"),
        new(252, "Somali Democratic Republic"),
        new(253, "Djibouti"),
        new(254, "Kenya"),
        new(255, "Tanzania"),
        new(256, "Uganda"),
        new(257, "Burundi"),
        new(258, "Mozambique"),
        new(260, "Zambia"),
        new(261, "Madagascar"),
        new(262, "French Departments and Territories in the Indian Ocean"),
        new(263, "Zimbabwe"),
        new(264, "Namibia"),
        new(265, "Malawi"),
        new(266, "Lesotho"),
        new(267, "Botswana"),
        new(268, "Swaziland"),
        new(269, "Comoros"),
        new(290, "Saint Helena, Ascension and Tristan da Cunha"),
        new(291, "Eritrea"),
        new(297, "Aruba"),
        new(298, "Faroe Islands"),
        new(299, "Greenland"),
        new(350, "Gibraltar"),
        new(351, "Portugal"),
        new(352, "Luxembourg"),
        new(353, "Ireland"),
        new(354, "Iceland"),
        new(355, "Albania"),
        new(356, "Malta"),
        new(357, "Cyprus"),
        new(358, "Finland"),
        new(359, "Bulgaria"),
        new(370, "Lithuania"),
        new(371, "Latvia"),
        new(372, "Estonia"),
        new(373, "Moldova"),
        new(374, "Armenia"),
        new(375, "Belarus"),
        new(376, "Andorra"),
        new(377, "Monaco"),
        new(378, "San Marino"),
        new(379, "Vatican City State"),
        new(380, "Ukraine"),
        new(381, "Serbia"),
        new(382, "Montenegro"),
        new(385, "Croatia"),
        new(386, "Slovenia"),
        new(387, "Bosnia and Herzegovina"),
        new(388, "Group of countries"),
        new(389, "The Former Yugoslav Republic of Macedonia"),
        new(420, "Czech Republic"),
        new(421, "Slovak Republic"),
        new(423, "Liechtenstein"),
        new(500, "Falkland Islands"),
        new(501, "Belize"),
        new(502, "Guatemala"),
        new(503, "El Salvador"),
        new(504, "Honduras"),
        new(505, "Nicaragua"),
        new(506, "Costa Rica"),
        new(507, "Panama"),
        new(508, "Saint Pierre and Miquelon"),
        new(509, "Haiti"),
        new(590, "Guadeloupe"),
        new(591, "Bolivia"),
        new(592, "Guyana"),
        new(593, "Ecuador"),
        new(594, "French Guiana"),
        new(595, "Paraguay"),
        new(596, "Martinique"),
        new(597, "Suriname"),
        new(598, "Uruguay"),
        new(599, "Bonaire, Saint Eustatius and Saba"),
        new(599, "Curaçao"),
        new(670, "Democratic Republic of Timor-Leste"),
        new(672, "Australian External Territories"),
        new(673, "Brunei Darussalam"),
        new(674, "Nauru"),
        new(675, "Papua New Guinea"),
        new(676, "Tonga"),
        new(677, "Solomon Islands"),
        new(678, "Vanuatu"),
        new(679, "Fiji"),
        new(680, "Palau"),
        new(681, "Wallis and Futuna"),
        new(682, "Cook Islands"),
        new(683, "Niue"),
        new(685, "Samoa"),
        new(686, "Kiribati"),
        new(687, "New Caledonia"),
        new(688, "Tuvalu"),
        new(689, "French Polynesia"),
        new(690, "Tokelau"),
        new(691, "Micronesia"),
        new(692, "Marshall Islands"),
        new(800, "International Freephone Service"),
        new(808, "International Shared Cost Service (ISCS)"),
        new(850, "Democratic People's Republic of Korea"),
        new(852, "Hong Kong, China"),
        new(853, "Macao, China"),
        new(855, "Cambodia"),
        new(856, "Lao People's Democratic Republic"),
        new(870, "Inmarsat SNAC"),
        new(878, "Universal Personal Telecommunication Service (UPT)"),
        new(880, "Bangladesh"),
        new(881, "Global Mobile Satellite System (GMSS)"),
        new(886, "Taiwan, China"),
        new(888, "Telecommunications for Disaster Relief (TDR)"),
        new(960, "Maldives"),
        new(961, "Lebanon"),
        new(962, "Jordan"),
        new(963, "Syrian Arab Republic"),
        new(964, "Iraq"),
        new(965, "Kuwait"),
        new(966, "Saudi Arabia"),
        new(967, "Yemen"),
        new(968, "Oman"),
        new(971, "United Arab Emirates"),
        new(972, "Israel"),
        new(973, "Bahrain"),
        new(974, "Qatar"),
        new(975, "Bhutan"),
        new(976, "Mongolia"),
        new(977, "Nepal"),
        new(992, "Tajikistan"),
        new(993, "Turkmenistan"),
        new(994, "Azerbaijani Republic"),
        new(995, "Georgia"),
        new(996, "Kyrgyz Republic"),
        new(998, "Uzbekistan"),
    ];

    // internal for testing
    internal static readonly uint[] AssignedCodes = CachedInfo.Select(c => c.Code).Distinct().Order().ToArray();
}
