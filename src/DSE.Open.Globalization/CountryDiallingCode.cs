// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics;

namespace DSE.Open.Globalization;

public readonly struct CountryDiallingCode : IEquatable<CountryDiallingCode>
{
    private static readonly Dictionary<short, CountryDiallingCode> s_codeLookup = CreateCodeLookup();

    public static readonly CountryDiallingCode Unknown;

    private CountryDiallingCode(short code, string name)
    {
        Debug.Assert(code > 0);
        Debug.Assert(code < 1000);

        Code = code;
        Name = name;
    }

    public short Code { get; }

    public static IEnumerable<CountryDiallingCode> Codes => s_codeLookup.Values;

    public static IReadOnlyDictionary<short, CountryDiallingCode> CodeTable => s_codeLookup;

    public string Name { get; }

    public bool Equals(CountryDiallingCode other)
    {
        return Equals(this, other);
    }

    private static Dictionary<short, CountryDiallingCode> CreateCodeLookup()
    {
        var codeLookup = new Dictionary<short, CountryDiallingCode>(207)
            {
                // ? codeLookup.Add(1, new CountryDiallingCode(0, "Unknown / Unidentified"));
                {1, new CountryDiallingCode(1, "USA, Canada, US Territories or Caribbean")},
                {7, new CountryDiallingCode(7, "Russia or Kazakhstan")},
                {20, new CountryDiallingCode(20, "Egypt")},
                {27, new CountryDiallingCode(27, "South Africa")},
                {30, new CountryDiallingCode(30, "Greece")},
                {31, new CountryDiallingCode(31, "Netherlands")},
                {32, new CountryDiallingCode(32, "Belgium")},
                {33, new CountryDiallingCode(33, "France")},
                {34, new CountryDiallingCode(34, "Spain")},
                {36, new CountryDiallingCode(36, "Hungary")},
                {39, new CountryDiallingCode(39, "Italy")},
                {40, new CountryDiallingCode(40, "Romania")},
                {41, new CountryDiallingCode(41, "Switzerland")},
                {43, new CountryDiallingCode(43, "Austria")},
                {44, new CountryDiallingCode(44, "United Kingdom")},
                {45, new CountryDiallingCode(45, "Denmark")},
                {46, new CountryDiallingCode(46, "Sweden")},
                {47, new CountryDiallingCode(47, "Norway")},
                {48, new CountryDiallingCode(48, "Poland")},
                {49, new CountryDiallingCode(49, "Germany")},
                {51, new CountryDiallingCode(51, "Peru")},
                {52, new CountryDiallingCode(52, "Mexico")},
                {53, new CountryDiallingCode(53, "Cuba")},
                {54, new CountryDiallingCode(54, "Argentina")},
                {55, new CountryDiallingCode(55, "Brazil")},
                {56, new CountryDiallingCode(56, "Chile")},
                {57, new CountryDiallingCode(57, "Colombia")},
                {58, new CountryDiallingCode(58, "Venezuela")},
                {60, new CountryDiallingCode(60, "Malaysia")},
                {61, new CountryDiallingCode(61, "Australia")},
                {62, new CountryDiallingCode(62, "Indonesia")},
                {63, new CountryDiallingCode(63, "Philippines")},
                {64, new CountryDiallingCode(64, "New Zealand")},
                {65, new CountryDiallingCode(65, "Singapore")},
                {66, new CountryDiallingCode(66, "Thailand")},
                {81, new CountryDiallingCode(81, "Japan")},
                {82, new CountryDiallingCode(82, "Korea, Republic of")},
                {84, new CountryDiallingCode(84, "Viet Nam")},
                {86, new CountryDiallingCode(86, "China, mainland")},
                {90, new CountryDiallingCode(90, "Turkey")},
                {91, new CountryDiallingCode(91, "India")},
                {92, new CountryDiallingCode(92, "Pakistan")},
                {93, new CountryDiallingCode(93, "Afghanistan")},
                {94, new CountryDiallingCode(94, "Sri Lanka")},
                {95, new CountryDiallingCode(95, "Myanmar")},
                {98, new CountryDiallingCode(98, "Iran")},
                {212, new CountryDiallingCode(212, "Morocco, Western Sahara")},
                {213, new CountryDiallingCode(213, "Algeria")},
                {216, new CountryDiallingCode(216, "Tunisia")},
                {218, new CountryDiallingCode(218, "Libyan Arab Jamahiriya")},
                {220, new CountryDiallingCode(220, "Gambia")},
                {221, new CountryDiallingCode(221, "Senegal")},
                {222, new CountryDiallingCode(222, "Mauritania")},
                {223, new CountryDiallingCode(223, "Mali")},
                {224, new CountryDiallingCode(224, "Guinea")},
                {225, new CountryDiallingCode(225, "Côte d'Ivoire")},
                {226, new CountryDiallingCode(226, "Burkina Faso")},
                {227, new CountryDiallingCode(227, "Niger")},
                {228, new CountryDiallingCode(228, "Togo")},
                {229, new CountryDiallingCode(229, "Benin")},
                {230, new CountryDiallingCode(230, "Mauritius")},
                {231, new CountryDiallingCode(231, "Liberia")},
                {232, new CountryDiallingCode(232, "Sierra Leone")},
                {233, new CountryDiallingCode(233, "Ghana")},
                {234, new CountryDiallingCode(234, "Nigeria")},
                {235, new CountryDiallingCode(235, "Chad")},
                {236, new CountryDiallingCode(236, "Central African Republic")},
                {237, new CountryDiallingCode(237, "Cameroon")},
                {238, new CountryDiallingCode(238, "Cape Verde")},
                {239, new CountryDiallingCode(239, "São Tomé and Príncipe")},
                {240, new CountryDiallingCode(240, "Equatorial Guinea")},
                {241, new CountryDiallingCode(241, "Gabon")},
                {242, new CountryDiallingCode(242, "Congo, Republic of the")},
                {243, new CountryDiallingCode(243, "Congo, The Democratic Republic Of The")},
                {244, new CountryDiallingCode(244, "Angola")},
                {245, new CountryDiallingCode(245, "Guinea-Bissau")},
                {246, new CountryDiallingCode(246, "British Indian Ocean Territory")},
                {247, new CountryDiallingCode(247, "Ascension Island")},
                {248, new CountryDiallingCode(248, "Seychelles")},
                {249, new CountryDiallingCode(249, "Sudan")},
                {250, new CountryDiallingCode(250, "Rwanda")},
                {251, new CountryDiallingCode(251, "Ethiopia")},
                {252, new CountryDiallingCode(252, "Somalia")},
                {253, new CountryDiallingCode(253, "Djibouti")},
                {254, new CountryDiallingCode(254, "Kenya")},
                {255, new CountryDiallingCode(255, "Tanzania, United Republic Of")},
                {256, new CountryDiallingCode(256, "Uganda")},
                {257, new CountryDiallingCode(257, "Burundi")},
                {258, new CountryDiallingCode(258, "Mozambique")},
                {260, new CountryDiallingCode(260, "Zambia")},
                {261, new CountryDiallingCode(261, "Madagascar")},
                {262, new CountryDiallingCode(262, "Réunion")},
                {263, new CountryDiallingCode(263, "Zimbabwe")},
                {264, new CountryDiallingCode(264, "Namibia")},
                {265, new CountryDiallingCode(265, "Malawi")},
                {266, new CountryDiallingCode(266, "Lesotho")},
                {267, new CountryDiallingCode(267, "Botswana")},
                {268, new CountryDiallingCode(268, "Swaziland")},
                {269, new CountryDiallingCode(269, "Comoros")},
                {290, new CountryDiallingCode(290, "Saint Helena")},
                {291, new CountryDiallingCode(291, "Eritrea")},
                {297, new CountryDiallingCode(297, "Aruba")},
                {298, new CountryDiallingCode(298, "Faroe Islands")},
                {299, new CountryDiallingCode(299, "Greenland")},
                {350, new CountryDiallingCode(350, "Gibraltar")},
                {351, new CountryDiallingCode(351, "Portugal")},
                {352, new CountryDiallingCode(352, "Luxembourg")},
                {353, new CountryDiallingCode(353, "Ireland")},
                {354, new CountryDiallingCode(354, "Iceland")},
                {355, new CountryDiallingCode(355, "Albania")},
                {356, new CountryDiallingCode(356, "Malta")},
                {357, new CountryDiallingCode(357, "Cyprus")},
                {358, new CountryDiallingCode(358, "Finland")},
                {359, new CountryDiallingCode(359, "Bulgaria")},
                {370, new CountryDiallingCode(370, "Lithuania")},
                {371, new CountryDiallingCode(371, "Latvia")},
                {372, new CountryDiallingCode(372, "Estonia")},
                {373, new CountryDiallingCode(373, "Moldova, Republic of")},
                {374, new CountryDiallingCode(374, "Armenia")},
                {375, new CountryDiallingCode(375, "Belarus")},
                {376, new CountryDiallingCode(376, "Andorra")},
                {377, new CountryDiallingCode(377, "Monaco")},
                {378, new CountryDiallingCode(378, "San Marino")},
                {379, new CountryDiallingCode(379, "Vatican City State")},
                {380, new CountryDiallingCode(380, "Ukraine")},
                {381, new CountryDiallingCode(381, "Serbia and Montenegro")},
                {382, new CountryDiallingCode(382, "Probably Montenegro")},
                {385, new CountryDiallingCode(385, "Croatia")},
                {386, new CountryDiallingCode(386, "Slovenia")},
                {387, new CountryDiallingCode(387, "Bosnia and Herzegovina")},
                {388, new CountryDiallingCode(388, "EU")},
                {389, new CountryDiallingCode(389, "Macedonia, The Former Yugoslav Republic of")},
                {420, new CountryDiallingCode(420, "Czech Republic")},
                {421, new CountryDiallingCode(421, "Slovakia")},
                {423, new CountryDiallingCode(423, "Liechtenstein")},
                {500, new CountryDiallingCode(500, "Falkland Islands")},
                {501, new CountryDiallingCode(501, "Belize")},
                {502, new CountryDiallingCode(502, "Guatemala")},
                {503, new CountryDiallingCode(503, "El Salvador")},
                {504, new CountryDiallingCode(504, "Honduras")},
                {505, new CountryDiallingCode(505, "Nicaragua")},
                {506, new CountryDiallingCode(506, "Costa Rica")},
                {507, new CountryDiallingCode(507, "Panama")},
                {508, new CountryDiallingCode(508, "Saint-Pierre and Miquelon")},
                {509, new CountryDiallingCode(509, "Haiti")},
                {590, new CountryDiallingCode(590, "Guadeloupe")},
                {591, new CountryDiallingCode(591, "Bolivia")},
                {592, new CountryDiallingCode(592, "Guyana")},
                {593, new CountryDiallingCode(593, "Ecuador")},
                {594, new CountryDiallingCode(594, "French Guiana")},
                {595, new CountryDiallingCode(595, "Paraguay")},
                {596, new CountryDiallingCode(596, "Martinique")},
                {597, new CountryDiallingCode(597, "Suriname")},
                {598, new CountryDiallingCode(598, "Uruguay")},
                {599, new CountryDiallingCode(599, "Netherlands Antilles")},
                {670, new CountryDiallingCode(670, "Timor-Leste")},
                {672, new CountryDiallingCode(672, "Antarctica")},
                {673, new CountryDiallingCode(673, "Brunei Darussalam")},
                {674, new CountryDiallingCode(674, "Nauru")},
                {675, new CountryDiallingCode(675, "Papua New Guinea")},
                {676, new CountryDiallingCode(676, "Tonga")},
                {677, new CountryDiallingCode(677, "Solomon Islands")},
                {678, new CountryDiallingCode(678, "Vanuatu")},
                {679, new CountryDiallingCode(679, "Fiji")},
                {680, new CountryDiallingCode(680, "Palau")},
                {681, new CountryDiallingCode(681, "Wallis and Futuna")},
                {682, new CountryDiallingCode(682, "Cook Islands")},
                {683, new CountryDiallingCode(683, "Niue")},
                {685, new CountryDiallingCode(685, "Samoa")},
                {686, new CountryDiallingCode(686, "Kiribati")},
                {687, new CountryDiallingCode(687, "New Caledonia")},
                {688, new CountryDiallingCode(688, "Tuvalu")},
                {689, new CountryDiallingCode(689, "French Polynesia")},
                {690, new CountryDiallingCode(690, "Tokelau")},
                {691, new CountryDiallingCode(691, "Micronesia, Federated States of")},
                {692, new CountryDiallingCode(692, "Marshall Islands")},
                {850, new CountryDiallingCode(850, "Korea, Democratic People's Republic of")},
                {852, new CountryDiallingCode(852, "Hong Kong")},
                {853, new CountryDiallingCode(853, "Macao")},
                {855, new CountryDiallingCode(855, "Cambodia")},
                {856, new CountryDiallingCode(856, "Lao People's Democratic Republic")},
                {880, new CountryDiallingCode(880, "Bangladesh")},
                {886, new CountryDiallingCode(886, "Taiwan (Republic of China")},
                {960, new CountryDiallingCode(960, "Maldives")},
                {961, new CountryDiallingCode(961, "Lebanon")},
                {962, new CountryDiallingCode(962, "Jordan")},
                {963, new CountryDiallingCode(963, "Syrian Arab Republic")},
                {964, new CountryDiallingCode(964, "Iraq")},
                {965, new CountryDiallingCode(965, "Kuwait")},
                {966, new CountryDiallingCode(966, "Saudi Arabia")},
                {967, new CountryDiallingCode(967, "Yemen")},
                {968, new CountryDiallingCode(968, "Oman")},
                {970, new CountryDiallingCode(970, "Palestinian Territory, Occupied")},
                {971, new CountryDiallingCode(971, "United Arab Emirates")},
                {972, new CountryDiallingCode(972, "Israel")},
                {973, new CountryDiallingCode(973, "Bahrain")},
                {974, new CountryDiallingCode(974, "Qatar")},
                {975, new CountryDiallingCode(975, "Bhutan")},
                {976, new CountryDiallingCode(976, "Mongolia")},
                {977, new CountryDiallingCode(977, "Nepal")},
                {992, new CountryDiallingCode(992, "Tajikistan")},
                {993, new CountryDiallingCode(993, "Turkmenistan")},
                {994, new CountryDiallingCode(994, "Azerbaijan")},
                {995, new CountryDiallingCode(995, "Georgia")},
                {996, new CountryDiallingCode(996, "Kyrgyzstan")},
                {998, new CountryDiallingCode(998, "Uzbekistan")},
            };
        return codeLookup;
    }

    public override bool Equals(object? obj)
    {
        return obj is CountryDiallingCode code && Equals(code);
    }

    public static bool Equals(CountryDiallingCode c1, CountryDiallingCode c2)
    {
        return c1.Code == c2.Code;
    }

    public static CountryDiallingCode FromCode(short code)
    {
        return IsValidCountryDiallingCode(code)
            ? s_codeLookup[code]
            : throw new ArgumentOutOfRangeException(nameof(code), "Invalid dialling code.");
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }

    public static bool IsValidCountryDiallingCode(short code)
    {
        return s_codeLookup.ContainsKey(code);
    }

    public override string ToString()
    {
        return "+" + Code.ToStringInvariant();
    }

    public static bool operator ==(CountryDiallingCode c1, CountryDiallingCode c2)
    {
        return Equals(c1, c2);
    }

    public static bool operator !=(CountryDiallingCode c1, CountryDiallingCode c2)
    {
        return !Equals(c1, c2);
    }
}
