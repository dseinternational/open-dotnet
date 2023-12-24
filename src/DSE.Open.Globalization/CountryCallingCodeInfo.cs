// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace DSE.Open.Globalization;

[StructLayout(LayoutKind.Auto)]
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
        new CountryCallingCodeInfo(1, "American Samoa"),
        new CountryCallingCodeInfo(1, "Anguilla"),
        new CountryCallingCodeInfo(1, "Antigua and Barbuda"),
        new CountryCallingCodeInfo(1, "Bahamas"),
        new CountryCallingCodeInfo(1, "Barbados"),
        new CountryCallingCodeInfo(1, "Bermuda"),
        new CountryCallingCodeInfo(1, "British Virgin Islands"),
        new CountryCallingCodeInfo(1, "Canada"),
        new CountryCallingCodeInfo(1, "Cayman Islands"),
        new CountryCallingCodeInfo(1, "Dominica"),
        new CountryCallingCodeInfo(1, "Dominican Republic"),
        new CountryCallingCodeInfo(1, "Grenada"),
        new CountryCallingCodeInfo(1, "Guam"),
        new CountryCallingCodeInfo(1, "Jamaica"),
        new CountryCallingCodeInfo(1, "Montserrat"),
        new CountryCallingCodeInfo(1, "Northern Mariana Islands"),
        new CountryCallingCodeInfo(1, "Puerto Rico"),
        new CountryCallingCodeInfo(1, "Saint Kitts and Nevis"),
        new CountryCallingCodeInfo(1, "Saint Lucia"),
        new CountryCallingCodeInfo(1, "Saint Vincent and the Grenadines"),
        new CountryCallingCodeInfo(1, "Sint Maarten"),
        new CountryCallingCodeInfo(1, "Trinidad and Tobago"),
        new CountryCallingCodeInfo(1, "Turks and Caicos Islands"),
        new CountryCallingCodeInfo(1, "United States of America"),
        new CountryCallingCodeInfo(1, "United States Virgin Islands"),
        new CountryCallingCodeInfo(7, "Kazakhstan"),
        new CountryCallingCodeInfo(7, "Russian Federation"),
        new CountryCallingCodeInfo(20, "Egypt"),
        new CountryCallingCodeInfo(27, "South Africa"),
        new CountryCallingCodeInfo(30, "Greece"),
        new CountryCallingCodeInfo(31, "Netherlands"),
        new CountryCallingCodeInfo(32, "Belgium"),
        new CountryCallingCodeInfo(33, "France"),
        new CountryCallingCodeInfo(34, "Spain"),
        new CountryCallingCodeInfo(36, "Hungary"),
        new CountryCallingCodeInfo(39, "Italy"),
        new CountryCallingCodeInfo(39, "Vatican City State"),
        new CountryCallingCodeInfo(40, "Romania"),
        new CountryCallingCodeInfo(41, "Switzerland"),
        new CountryCallingCodeInfo(43, "Austria"),
        new CountryCallingCodeInfo(44, "United Kingdom"),
        new CountryCallingCodeInfo(45, "Denmark"),
        new CountryCallingCodeInfo(46, "Sweden"),
        new CountryCallingCodeInfo(47, "Norway"),
        new CountryCallingCodeInfo(48, "Poland"),
        new CountryCallingCodeInfo(49, "Germany"),
        new CountryCallingCodeInfo(51, "Peru"),
        new CountryCallingCodeInfo(52, "Mexico"),
        new CountryCallingCodeInfo(53, "Cuba"),
        new CountryCallingCodeInfo(54, "Argentine Republic"),
        new CountryCallingCodeInfo(55, "Brazil"),
        new CountryCallingCodeInfo(56, "Chile"),
        new CountryCallingCodeInfo(57, "Colombia"),
        new CountryCallingCodeInfo(58, "Venezuela"),
        new CountryCallingCodeInfo(60, "Malaysia"),
        new CountryCallingCodeInfo(61, "Australia"),
        new CountryCallingCodeInfo(62, "Indonesia"),
        new CountryCallingCodeInfo(63, "Philippines"),
        new CountryCallingCodeInfo(64, "New Zealand"),
        new CountryCallingCodeInfo(65, "Singapore"),
        new CountryCallingCodeInfo(66, "Thailand"),
        new CountryCallingCodeInfo(81, "Japan"),
        new CountryCallingCodeInfo(82, "Korea"),
        new CountryCallingCodeInfo(84, "Viet Nam"),
        new CountryCallingCodeInfo(86, "China"),
        new CountryCallingCodeInfo(90, "Turkey"),
        new CountryCallingCodeInfo(91, "India"),
        new CountryCallingCodeInfo(92, "Pakistan"),
        new CountryCallingCodeInfo(93, "Afghanistan"),
        new CountryCallingCodeInfo(94, "Sri Lanka"),
        new CountryCallingCodeInfo(95, "Myanmar"),
        new CountryCallingCodeInfo(98, "Iran"),
        new CountryCallingCodeInfo(211, "South Sudan"),
        new CountryCallingCodeInfo(212, "Morocco"),
        new CountryCallingCodeInfo(213, "Algeria"),
        new CountryCallingCodeInfo(216, "Tunisia"),
        new CountryCallingCodeInfo(218, "Libya"),
        new CountryCallingCodeInfo(220, "Gambia"),
        new CountryCallingCodeInfo(221, "Senegal"),
        new CountryCallingCodeInfo(222, "Mauritania"),
        new CountryCallingCodeInfo(223, "Mali"),
        new CountryCallingCodeInfo(224, "Guinea"),
        new CountryCallingCodeInfo(225, "Côte d'Ivoire"),
        new CountryCallingCodeInfo(226, "Burkina Faso"),
        new CountryCallingCodeInfo(227, "Niger"),
        new CountryCallingCodeInfo(228, "Togolese Republic"),
        new CountryCallingCodeInfo(229, "Benin"),
        new CountryCallingCodeInfo(230, "Mauritius"),
        new CountryCallingCodeInfo(231, "Liberia"),
        new CountryCallingCodeInfo(232, "Sierra Leone"),
        new CountryCallingCodeInfo(233, "Ghana"),
        new CountryCallingCodeInfo(234, "Nigeria"),
        new CountryCallingCodeInfo(235, "Chad"),
        new CountryCallingCodeInfo(236, "Central African Republic"),
        new CountryCallingCodeInfo(237, "Cameroon"),
        new CountryCallingCodeInfo(238, "Cape Verde"),
        new CountryCallingCodeInfo(239, "Sao Tome and Principe"),
        new CountryCallingCodeInfo(240, "Equatorial Guinea"),
        new CountryCallingCodeInfo(241, "Gabonese Republic"),
        new CountryCallingCodeInfo(242, "Congo"),
        new CountryCallingCodeInfo(243, "Democratic Republic of the Congo"),
        new CountryCallingCodeInfo(244, "Angola"),
        new CountryCallingCodeInfo(245, "Guinea-Bissau"),
        new CountryCallingCodeInfo(246, "Diego Garcia"),
        new CountryCallingCodeInfo(247, "Saint Helena, Ascension and Tristan da Cunha"),
        new CountryCallingCodeInfo(248, "Seychelles"),
        new CountryCallingCodeInfo(249, "Sudan"),
        new CountryCallingCodeInfo(250, "Rwanda"),
        new CountryCallingCodeInfo(251, "Ethiopia"),
        new CountryCallingCodeInfo(252, "Somali Democratic Republic"),
        new CountryCallingCodeInfo(253, "Djibouti"),
        new CountryCallingCodeInfo(254, "Kenya"),
        new CountryCallingCodeInfo(255, "Tanzania"),
        new CountryCallingCodeInfo(256, "Uganda"),
        new CountryCallingCodeInfo(257, "Burundi"),
        new CountryCallingCodeInfo(258, "Mozambique"),
        new CountryCallingCodeInfo(260, "Zambia"),
        new CountryCallingCodeInfo(261, "Madagascar"),
        new CountryCallingCodeInfo(262, "French Departments and Territories in the Indian Ocean"),
        new CountryCallingCodeInfo(263, "Zimbabwe"),
        new CountryCallingCodeInfo(264, "Namibia"),
        new CountryCallingCodeInfo(265, "Malawi"),
        new CountryCallingCodeInfo(266, "Lesotho"),
        new CountryCallingCodeInfo(267, "Botswana"),
        new CountryCallingCodeInfo(268, "Swaziland"),
        new CountryCallingCodeInfo(269, "Comoros"),
        new CountryCallingCodeInfo(290, "Saint Helena, Ascension and Tristan da Cunha"),
        new CountryCallingCodeInfo(291, "Eritrea"),
        new CountryCallingCodeInfo(297, "Aruba"),
        new CountryCallingCodeInfo(298, "Faroe Islands"),
        new CountryCallingCodeInfo(299, "Greenland"),
        new CountryCallingCodeInfo(350, "Gibraltar"),
        new CountryCallingCodeInfo(351, "Portugal"),
        new CountryCallingCodeInfo(352, "Luxembourg"),
        new CountryCallingCodeInfo(353, "Ireland"),
        new CountryCallingCodeInfo(354, "Iceland"),
        new CountryCallingCodeInfo(355, "Albania"),
        new CountryCallingCodeInfo(356, "Malta"),
        new CountryCallingCodeInfo(357, "Cyprus"),
        new CountryCallingCodeInfo(358, "Finland"),
        new CountryCallingCodeInfo(359, "Bulgaria"),
        new CountryCallingCodeInfo(370, "Lithuania"),
        new CountryCallingCodeInfo(371, "Latvia"),
        new CountryCallingCodeInfo(372, "Estonia"),
        new CountryCallingCodeInfo(373, "Moldova"),
        new CountryCallingCodeInfo(374, "Armenia"),
        new CountryCallingCodeInfo(375, "Belarus"),
        new CountryCallingCodeInfo(376, "Andorra"),
        new CountryCallingCodeInfo(377, "Monaco"),
        new CountryCallingCodeInfo(378, "San Marino"),
        new CountryCallingCodeInfo(379, "Vatican City State"),
        new CountryCallingCodeInfo(380, "Ukraine"),
        new CountryCallingCodeInfo(381, "Serbia"),
        new CountryCallingCodeInfo(382, "Montenegro"),
        new CountryCallingCodeInfo(385, "Croatia"),
        new CountryCallingCodeInfo(386, "Slovenia"),
        new CountryCallingCodeInfo(387, "Bosnia and Herzegovina"),
        new CountryCallingCodeInfo(388, "Group of countries"),
        new CountryCallingCodeInfo(389, "The Former Yugoslav Republic of Macedonia"),
        new CountryCallingCodeInfo(420, "Czech Republic"),
        new CountryCallingCodeInfo(421, "Slovak Republic"),
        new CountryCallingCodeInfo(423, "Liechtenstein"),
        new CountryCallingCodeInfo(500, "Falkland Islands"),
        new CountryCallingCodeInfo(501, "Belize"),
        new CountryCallingCodeInfo(502, "Guatemala"),
        new CountryCallingCodeInfo(503, "El Salvador"),
        new CountryCallingCodeInfo(504, "Honduras"),
        new CountryCallingCodeInfo(505, "Nicaragua"),
        new CountryCallingCodeInfo(506, "Costa Rica"),
        new CountryCallingCodeInfo(507, "Panama"),
        new CountryCallingCodeInfo(508, "Saint Pierre and Miquelon"),
        new CountryCallingCodeInfo(509, "Haiti"),
        new CountryCallingCodeInfo(590, "Guadeloupe"),
        new CountryCallingCodeInfo(591, "Bolivia"),
        new CountryCallingCodeInfo(592, "Guyana"),
        new CountryCallingCodeInfo(593, "Ecuador"),
        new CountryCallingCodeInfo(594, "French Guiana"),
        new CountryCallingCodeInfo(595, "Paraguay"),
        new CountryCallingCodeInfo(596, "Martinique"),
        new CountryCallingCodeInfo(597, "Suriname"),
        new CountryCallingCodeInfo(598, "Uruguay"),
        new CountryCallingCodeInfo(599, "Bonaire, Saint Eustatius and Saba"),
        new CountryCallingCodeInfo(599, "Curaçao"),
        new CountryCallingCodeInfo(670, "Democratic Republic of Timor-Leste"),
        new CountryCallingCodeInfo(672, "Australian External Territories"),
        new CountryCallingCodeInfo(673, "Brunei Darussalam"),
        new CountryCallingCodeInfo(674, "Nauru"),
        new CountryCallingCodeInfo(675, "Papua New Guinea"),
        new CountryCallingCodeInfo(676, "Tonga"),
        new CountryCallingCodeInfo(677, "Solomon Islands"),
        new CountryCallingCodeInfo(678, "Vanuatu"),
        new CountryCallingCodeInfo(679, "Fiji"),
        new CountryCallingCodeInfo(680, "Palau"),
        new CountryCallingCodeInfo(681, "Wallis and Futuna"),
        new CountryCallingCodeInfo(682, "Cook Islands"),
        new CountryCallingCodeInfo(683, "Niue"),
        new CountryCallingCodeInfo(685, "Samoa"),
        new CountryCallingCodeInfo(686, "Kiribati"),
        new CountryCallingCodeInfo(687, "New Caledonia"),
        new CountryCallingCodeInfo(688, "Tuvalu"),
        new CountryCallingCodeInfo(689, "French Polynesia"),
        new CountryCallingCodeInfo(690, "Tokelau"),
        new CountryCallingCodeInfo(691, "Micronesia"),
        new CountryCallingCodeInfo(692, "Marshall Islands"),
        new CountryCallingCodeInfo(800, "International Freephone Service"),
        new CountryCallingCodeInfo(808, "International Shared Cost Service (ISCS)"),
        new CountryCallingCodeInfo(850, "Democratic People's Republic of Korea"),
        new CountryCallingCodeInfo(852, "Hong Kong, China"),
        new CountryCallingCodeInfo(853, "Macao, China"),
        new CountryCallingCodeInfo(855, "Cambodia"),
        new CountryCallingCodeInfo(856, "Lao People's Democratic Republic"),
        new CountryCallingCodeInfo(870, "Inmarsat SNAC"),
        new CountryCallingCodeInfo(878, "Universal Personal Telecommunication Service (UPT)"),
        new CountryCallingCodeInfo(880, "Bangladesh"),
        new CountryCallingCodeInfo(881, "Global Mobile Satellite System (GMSS)"),
        new CountryCallingCodeInfo(886, "Taiwan, China"),
        new CountryCallingCodeInfo(888, "Telecommunications for Disaster Relief (TDR)"),
        new CountryCallingCodeInfo(960, "Maldives"),
        new CountryCallingCodeInfo(961, "Lebanon"),
        new CountryCallingCodeInfo(962, "Jordan"),
        new CountryCallingCodeInfo(963, "Syrian Arab Republic"),
        new CountryCallingCodeInfo(964, "Iraq"),
        new CountryCallingCodeInfo(965, "Kuwait"),
        new CountryCallingCodeInfo(966, "Saudi Arabia"),
        new CountryCallingCodeInfo(967, "Yemen"),
        new CountryCallingCodeInfo(968, "Oman"),
        new CountryCallingCodeInfo(971, "United Arab Emirates"),
        new CountryCallingCodeInfo(972, "Israel"),
        new CountryCallingCodeInfo(973, "Bahrain"),
        new CountryCallingCodeInfo(974, "Qatar"),
        new CountryCallingCodeInfo(975, "Bhutan"),
        new CountryCallingCodeInfo(976, "Mongolia"),
        new CountryCallingCodeInfo(977, "Nepal"),
        new CountryCallingCodeInfo(992, "Tajikistan"),
        new CountryCallingCodeInfo(993, "Turkmenistan"),
        new CountryCallingCodeInfo(994, "Azerbaijani Republic"),
        new CountryCallingCodeInfo(995, "Georgia"),
        new CountryCallingCodeInfo(996, "Kyrgyz Republic"),
        new CountryCallingCodeInfo(998, "Uzbekistan"),
    ];

    // internal for testing
    internal static readonly uint[] AssignedCodes = CachedInfo.Select(c => c.Code).Distinct().Order().ToArray();
}
