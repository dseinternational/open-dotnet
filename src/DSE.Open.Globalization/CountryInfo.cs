// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

namespace DSE.Open.Globalization;

public sealed record CountryInfo
{
    private CountryInfo(CountryCode code, int numericCode, string threeLetterCode,
        string name, bool isEuMemberState, bool isEuSpecialTerritory, bool isEuOutermostRegion,
        bool isUsOverseasTerritory, bool isGdprTerritory)
    {
        Code = code;
        NumericCode = numericCode;
        ThreeLetterCode = threeLetterCode;
        Name = name;
        IsEuMemberState = isEuMemberState;
        IsEuSpecialTerritory = isEuSpecialTerritory;
        IsEuOutermostRegion = isEuOutermostRegion;
        IsUsOverseasTerritory = isUsOverseasTerritory;
        IsGdprTerritory = isGdprTerritory;
    }

    private static CountryInfo FromData(string code, string threeLetterCode, int numericCode,
        string name, byte isEuMemberState, byte isEuSpecialTerritory, byte isEuOutermostRegion,
        byte isUsOverseasTerritory, byte isGdprTerritory)
    {
        return new CountryInfo(CountryCode.Parse(code, default), numericCode, threeLetterCode, name,
            isEuMemberState == 1, isEuSpecialTerritory == 1, isEuOutermostRegion == 1,
            isUsOverseasTerritory == 1, isGdprTerritory == 1);
    }

    public CountryCode Code { get; }

    public int NumericCode { get; }

    public string ThreeLetterCode { get; }

    public string Name { get; }

    public bool IsEuMemberState { get; }

    public bool IsEuSpecialTerritory { get; }

    public bool IsEuOutermostRegion { get; }

    public bool IsUsOverseasTerritory { get; }

    public bool IsGdprTerritory { get; }

    public static CountryInfo? FromCountryCode(CountryCode code)
    {
        return s_twoLetterCodeLookup.Value.GetValueOrDefault(code.ToStringInvariant().ToUpperInvariant());
    }

    public static CountryInfo? FromTwoLetterCode(string code)
    {
        return s_twoLetterCodeLookup.Value.GetValueOrDefault(code);
    }

    public static CountryInfo? FromThreeLetterCode(string code)
    {
        return s_threeLetterCodeLookup.Value.GetValueOrDefault(code);
    }

    public static CountryInfo? FromNumericCode(int code)
    {
        return s_numericCodeLookup.Value.GetValueOrDefault(code);
    }

    private static readonly Lazy<Dictionary<string, CountryInfo>> s_twoLetterCodeLookup = new(InitTwoLetterCodeLookup);
    private static readonly Lazy<Dictionary<string, CountryInfo>> s_threeLetterCodeLookup = new(InitThreeLetterCodeLookup);
    private static readonly Lazy<Dictionary<int, CountryInfo>> s_numericCodeLookup = new(InitNumericCodeLookup);

    private static Dictionary<string, CountryInfo> InitThreeLetterCodeLookup()
    {
        return s_countryData.ToDictionary(ci => ci.ThreeLetterCode, StringComparer.OrdinalIgnoreCase);
    }

    private static Dictionary<int, CountryInfo> InitNumericCodeLookup()
    {
        return s_countryData.ToDictionary(ci => ci.NumericCode);
    }

    public static IReadOnlyList<CountryInfo> GetAllCountries()
    {
        return s_countryData.ToArray();
    }

    public static IReadOnlyList<CountryInfo> GetEuMemberCountries()
    {
        return s_countryData.Where(ci => ci.IsEuMemberState).ToArray();
    }

    private static Dictionary<string, CountryInfo> InitTwoLetterCodeLookup()
    {
        return s_countryData.ToDictionary(ci => ci.Code.ToString(), StringComparer.OrdinalIgnoreCase);
    }

    private static readonly List<CountryInfo> s_countryData =
    [
        FromData("AD", "AND", 20, "Andorra", 0, 0, 0, 0, 0),
        FromData("AE", "ARE", 784, "United Arab Emirates", 0, 0, 0, 0, 0),
        FromData("AF", "AFG", 4, "Afghanistan", 0, 0, 0, 0, 0),
        FromData("AG", "ATG", 28, "Antigua and Barbuda", 0, 0, 0, 0, 0),
        FromData("AI", "AIA", 660, "Anguilla", 0, 1, 0, 1, 1),
        FromData("AL", "ALB", 8, "Albania", 0, 0, 0, 0, 0),
        FromData("AM", "ARM", 51, "Armenia", 0, 0, 0, 0, 0),
        FromData("AO", "AGO", 24, "Angola", 0, 0, 0, 0, 0),
        FromData("AQ", "ATA", 10, "Antarctica", 0, 0, 0, 0, 0),
        FromData("AR", "ARG", 32, "Argentina", 0, 0, 0, 0, 0),
        FromData("AS", "ASM", 16, "American Samoa", 0, 0, 0, 0, 0),
        FromData("AT", "AUT", 40, "Austria", 1, 0, 0, 0, 1),
        FromData("AU", "AUS", 36, "Australia", 0, 0, 0, 0, 0),
        FromData("AW", "ABW", 533, "Aruba", 0, 1, 0, 1, 1),
        FromData("AX", "ALA", 248, "France, Metropolitan", 0, 0, 0, 0, 0),
        FromData("AZ", "AZE", 31, "Azerbaijan", 0, 0, 0, 0, 0),
        FromData("BA", "BIH", 70, "Bosnia and Herzegovina", 0, 0, 0, 0, 0),
        FromData("BB", "BRB", 52, "Barbados", 0, 0, 0, 0, 0),
        FromData("BD", "BGD", 50, "Bangladesh", 0, 0, 0, 0, 0),
        FromData("BE", "BEL", 56, "Belgium", 1, 0, 0, 0, 1),
        FromData("BF", "BFA", 854, "Burkina Faso", 0, 0, 0, 0, 0),
        FromData("BG", "BGR", 100, "Bulgaria", 1, 0, 0, 0, 1),
        FromData("BH", "BHR", 48, "Bahrain", 0, 0, 0, 0, 0),
        FromData("BI", "BDI", 108, "Burundi", 0, 0, 0, 0, 0),
        FromData("BJ", "BEN", 204, "Benin", 0, 0, 0, 0, 0),
        FromData("BL", "BLM", 652, "Saint Barthélemy", 0, 1, 0, 1, 1),
        FromData("BM", "BMU", 60, "Bermuda", 0, 1, 0, 1, 1),
        FromData("BN", "BRN", 96, "Brunei", 0, 0, 0, 0, 0),
        FromData("BO", "BOL", 68, "Bolivia", 0, 0, 0, 0, 0),
        FromData("BQ", "BES", 535, "Bonaire, Sint Eustatius and Saba", 0, 0, 0, 0, 0),
        FromData("BR", "BRA", 76, "Brazil", 0, 0, 0, 0, 0),
        FromData("BS", "BHS", 44, "Bahamas, The", 0, 0, 0, 0, 0),
        FromData("BT", "BTN", 64, "Bhutan", 0, 0, 0, 0, 0),
        FromData("BV", "BVT", 74, "Bouvet Island", 0, 0, 0, 0, 0),
        FromData("BW", "BWA", 72, "Botswana", 0, 0, 0, 0, 0),
        FromData("BY", "BLR", 112, "Belarus", 0, 0, 0, 0, 0),
        FromData("BZ", "BLZ", 84, "Belize", 0, 0, 0, 0, 0),
        FromData("CA", "CAN", 124, "Canada", 0, 0, 0, 0, 0),
        FromData("CC", "CCK", 166, "Cocos (Keeling) Islands", 0, 0, 0, 0, 0),
        FromData("CD", "COD", 180, "Congo (DRC)", 0, 0, 0, 0, 0),
        FromData("CF", "CAF", 140, "Central African Republic", 0, 0, 0, 0, 0),
        FromData("CG", "COG", 178, "Congo", 0, 0, 0, 0, 0),
        FromData("CH", "CHE", 756, "Switzerland", 0, 0, 0, 0, 0),
        FromData("CI", "CIV", 384, "Côte d'Ivoire", 0, 0, 0, 0, 0),
        FromData("CK", "COK", 184, "Cook Islands", 0, 0, 0, 0, 0),
        FromData("CL", "CHL", 152, "Chile", 0, 0, 0, 0, 0),
        FromData("CM", "CMR", 120, "Cameroon", 0, 0, 0, 0, 0),
        FromData("CN", "CHN", 156, "China", 0, 0, 0, 0, 0),
        FromData("CO", "COL", 170, "Colombia", 0, 0, 0, 0, 0),
        FromData("CR", "CRI", 188, "Costa Rica", 0, 0, 0, 0, 0),
        FromData("CU", "CUB", 192, "Cuba", 0, 0, 0, 0, 0),
        FromData("CV", "CPV", 132, "Cape Verde", 0, 0, 0, 0, 0),
        FromData("CW", "CUW", 531, "Netherlands Antilles", 0, 1, 0, 1, 1),
        FromData("CX", "CXR", 162, "Christmas Island", 0, 0, 0, 0, 0),
        FromData("CY", "CYP", 196, "Cyprus", 1, 0, 0, 0, 1),
        FromData("CZ", "CZE", 203, "Czech Republic", 1, 0, 0, 0, 1),
        FromData("DE", "DEU", 276, "Germany", 1, 0, 0, 0, 1),
        FromData("DJ", "DJI", 262, "Djibouti", 0, 0, 0, 0, 0),
        FromData("DK", "DNK", 208, "Denmark", 1, 0, 0, 0, 1),
        FromData("DM", "DMA", 212, "Dominica", 0, 0, 0, 0, 0),
        FromData("DO", "DOM", 214, "Dominican Republic", 0, 0, 0, 0, 0),
        FromData("DZ", "DZA", 12, "Algeria", 0, 0, 0, 0, 0),
        FromData("EC", "ECU", 218, "Ecuador", 0, 0, 0, 0, 0),
        FromData("EE", "EST", 233, "Estonia", 1, 0, 0, 0, 1),
        FromData("EG", "EGY", 818, "Egypt", 0, 0, 0, 0, 0),
        FromData("EH", "ESH", 732, "Western Sahara", 0, 0, 0, 0, 0),
        FromData("ER", "ERI", 232, "Eritrea", 0, 0, 0, 0, 0),
        FromData("ES", "ESP", 724, "Spain", 1, 0, 0, 0, 1),
        FromData("ET", "ETH", 231, "Ethiopia", 0, 0, 0, 0, 0),
        FromData("FI", "FIN", 246, "Finland", 1, 0, 0, 0, 1),
        FromData("FJ", "FJI", 242, "Fiji Islands", 0, 0, 0, 0, 0),
        FromData("FK", "FLK", 238, "Falkland Islands (Islas Malvinas)", 0, 1, 0, 1, 1),
        FromData("FM", "FSM", 583, "Micronesia", 0, 0, 0, 0, 0),
        FromData("FO", "FRO", 234, "Faroe Islands", 0, 0, 0, 0, 0),
        FromData("FR", "FRA", 250, "France", 1, 0, 0, 0, 1),
        FromData("GA", "GAB", 266, "Gabon", 0, 0, 0, 0, 0),
        FromData("GB", "GBR", 826, "United Kingdom", 0, 0, 0, 0, 1),
        FromData("GD", "GRD", 308, "Grenada", 0, 0, 0, 0, 0),
        FromData("GE", "GEO", 268, "Georgia", 0, 0, 0, 0, 0),
        FromData("GF", "GUF", 254, "French Guiana", 0, 1, 1, 0, 1),
        FromData("GG", "GGY", 831, "Tanzania", 0, 0, 0, 0, 1),
        FromData("GH", "GHA", 288, "Ghana", 0, 0, 0, 0, 0),
        FromData("GI", "GIB", 292, "Gibraltar", 0, 0, 0, 0, 1),
        FromData("GL", "GRL", 304, "Greenland", 0, 1, 0, 1, 1),
        FromData("GM", "GMB", 270, "Gambia, The", 0, 0, 0, 0, 0),
        FromData("GN", "GIN", 324, "Guinea", 0, 0, 0, 0, 0),
        FromData("GP", "GLP", 312, "Guadeloupe", 0, 1, 1, 0, 1),
        FromData("GQ", "GNQ", 226, "Equatorial Guinea", 0, 0, 0, 0, 0),
        FromData("GR", "GRC", 300, "Greece", 1, 0, 0, 0, 1),
        FromData("GS", "SGS", 239, "South Georgia and the South Sandwich Islands", 0, 1, 0, 1, 1),
        FromData("GT", "GTM", 320, "Guatemala", 0, 0, 0, 0, 0),
        FromData("GU", "GUM", 316, "Guam", 0, 0, 0, 0, 0),
        FromData("GW", "GNB", 624, "Guinea-Bissau", 0, 0, 0, 0, 0),
        FromData("GY", "GUY", 328, "Guyana", 0, 0, 0, 0, 0),
        FromData("HK", "HKG", 344, "Hong Kong SAR", 0, 0, 0, 0, 0),
        FromData("HM", "HMD", 334, "Heard Island and McDonald Islands", 0, 0, 0, 0, 0),
        FromData("HN", "HND", 340, "Honduras", 0, 0, 0, 0, 0),
        FromData("HR", "HRV", 191, "Croatia", 1, 0, 0, 0, 1),
        FromData("HT", "HTI", 332, "Haiti", 0, 0, 0, 0, 0),
        FromData("HU", "HUN", 348, "Hungary", 1, 0, 0, 0, 1),
        FromData("ID", "IDN", 360, "Indonesia", 0, 0, 0, 0, 0),
        FromData("IE", "IRL", 372, "Ireland", 1, 0, 0, 0, 1),
        FromData("IL", "ISR", 376, "Israel", 0, 0, 0, 0, 0),
        FromData("IM", "IMN", 833, "Isle of Man", 0, 0, 0, 0, 1),
        FromData("IN", "IND", 356, "India", 0, 0, 0, 0, 0),
        FromData("IO", "IOT", 86, "British Indian Ocean Territory", 0, 1, 0, 1, 1),
        FromData("IQ", "IRQ", 368, "Iraq", 0, 0, 0, 0, 0),
        FromData("IR", "IRN", 364, "Iran", 0, 0, 0, 0, 0),
        FromData("IS", "ISL", 352, "Iceland", 0, 0, 0, 0, 0),
        FromData("IT", "ITA", 380, "Italy", 1, 0, 0, 0, 1),
        FromData("JE", "JEY", 832, "Jersey", 0, 0, 0, 0, 1),
        FromData("JM", "JAM", 388, "Jamaica", 0, 0, 0, 0, 0),
        FromData("JO", "JOR", 400, "Jordan", 0, 0, 0, 0, 0),
        FromData("JP", "JPN", 392, "Japan", 0, 0, 0, 0, 0),
        FromData("KE", "KEN", 404, "Kenya", 0, 0, 0, 0, 0),
        FromData("KG", "KGZ", 417, "Kyrgyzstan", 0, 0, 0, 0, 0),
        FromData("KH", "KHM", 116, "Cambodia", 0, 0, 0, 0, 0),
        FromData("KI", "KIR", 296, "Kiribati", 0, 0, 0, 0, 0),
        FromData("KM", "COM", 174, "Comoros", 0, 0, 0, 0, 0),
        FromData("KN", "KNA", 659, "St. Kitts and Nevis", 0, 0, 0, 0, 0),
        FromData("KP", "PRK", 408, "North Korea", 0, 0, 0, 0, 0),
        FromData("KR", "KOR", 410, "Korea", 0, 0, 0, 0, 0),
        FromData("KW", "KWT", 414, "Kuwait", 0, 0, 0, 0, 0),
        FromData("KY", "CYM", 136, "Cayman Islands", 0, 1, 0, 1, 1),
        FromData("KZ", "KAZ", 398, "Kazakhstan", 0, 0, 0, 0, 0),
        FromData("LA", "LAO", 418, "Laos", 0, 0, 0, 0, 0),
        FromData("LB", "LBN", 422, "Lebanon", 0, 0, 0, 0, 0),
        FromData("LC", "LCA", 662, "St. Lucia", 0, 0, 0, 0, 0),
        FromData("LI", "LIE", 438, "Liechtenstein", 0, 0, 0, 0, 0),
        FromData("LK", "LKA", 144, "Sri Lanka", 0, 0, 0, 0, 0),
        FromData("LR", "LBR", 430, "Liberia", 0, 0, 0, 0, 0),
        FromData("LS", "LSO", 426, "Lesotho", 0, 0, 0, 0, 0),
        FromData("LT", "LTU", 440, "Lithuania", 1, 0, 0, 0, 1),
        FromData("LU", "LUX", 442, "Luxembourg", 1, 0, 0, 0, 1),
        FromData("LV", "LVA", 428, "Latvia", 1, 0, 0, 0, 1),
        FromData("LY", "LBY", 434, "Libya", 0, 0, 0, 0, 0),
        FromData("MA", "MAR", 504, "Morocco", 0, 0, 0, 0, 0),
        FromData("MC", "MCO", 492, "Monaco", 0, 0, 0, 0, 0),
        FromData("MD", "MDA", 498, "Moldova", 0, 0, 0, 0, 0),
        FromData("ME", "MNE", 499, "Montenegro", 0, 1, 1, 0, 1),
        FromData("MF", "MAF", 663, "Saint Martin", 0, 1, 1, 0, 1),
        FromData("MG", "MDG", 450, "Madagascar", 0, 0, 0, 0, 0),
        FromData("MH", "MHL", 584, "Marshall Islands", 0, 0, 0, 0, 0),
        FromData("MK", "MKD", 807, "Macedonia, Former Yugoslav Republic of", 0, 0, 0, 0, 0),
        FromData("ML", "MLI", 466, "Mali", 0, 0, 0, 0, 0),
        FromData("MM", "MMR", 104, "Myanmar", 0, 0, 0, 0, 0),
        FromData("MN", "MNG", 496, "Mongolia", 0, 0, 0, 0, 0),
        FromData("MO", "MAC", 446, "Macau SAR", 0, 0, 0, 0, 0),
        FromData("MP", "MNP", 580, "Northern Mariana Islands", 0, 0, 0, 0, 0),
        FromData("MQ", "MTQ", 474, "Martinique", 0, 1, 1, 0, 1),
        FromData("MR", "MRT", 478, "Mauritania", 0, 0, 0, 0, 0),
        FromData("MS", "MSR", 500, "Montserrat", 0, 1, 0, 1, 1),
        FromData("MT", "MLT", 470, "Malta", 1, 0, 0, 0, 1),
        FromData("MU", "MUS", 480, "Mauritius", 0, 0, 0, 0, 0),
        FromData("MV", "MDV", 462, "Maldives", 0, 0, 0, 0, 0),
        FromData("MW", "MWI", 454, "Malawi", 0, 0, 0, 0, 0),
        FromData("MX", "MEX", 484, "Mexico", 0, 0, 0, 0, 0),
        FromData("MY", "MYS", 458, "Malaysia", 0, 0, 0, 0, 0),
        FromData("MZ", "MOZ", 508, "Mozambique", 0, 0, 0, 0, 0),
        FromData("NA", "NAM", 516, "Namibia", 0, 0, 0, 0, 0),
        FromData("NC", "NCL", 540, "New Caledonia", 0, 1, 0, 1, 1),
        FromData("NE", "NER", 562, "Niger", 0, 0, 0, 0, 0),
        FromData("NF", "NFK", 574, "Norfolk Island", 0, 0, 0, 0, 0),
        FromData("NG", "NGA", 566, "Nigeria", 0, 0, 0, 0, 0),
        FromData("NI", "NIC", 558, "Nicaragua", 0, 0, 0, 0, 0),
        FromData("NL", "NLD", 528, "Netherlands, The", 1, 0, 0, 0, 1),
        FromData("NO", "NOR", 578, "Norway", 0, 0, 0, 0, 0),
        FromData("NP", "NPL", 524, "Nepal", 0, 0, 0, 0, 0),
        FromData("NR", "NRU", 520, "Nauru", 0, 0, 0, 0, 0),
        FromData("NU", "NIU", 570, "Niue", 0, 0, 0, 0, 0),
        FromData("NZ", "NZL", 554, "New Zealand", 0, 0, 0, 0, 0),
        FromData("OM", "OMN", 512, "Oman", 0, 0, 0, 0, 0),
        FromData("PA", "PAN", 591, "Panama", 0, 0, 0, 0, 0),
        FromData("PE", "PER", 604, "Peru", 0, 0, 0, 0, 0),
        FromData("PF", "PYF", 258, "French Polynesia", 0, 1, 0, 1, 1),
        FromData("PG", "PNG", 598, "Papua New Guinea", 0, 0, 0, 0, 0),
        FromData("PH", "PHL", 608, "Philippines", 0, 0, 0, 0, 0),
        FromData("PK", "PAK", 586, "Pakistan", 0, 0, 0, 0, 0),
        FromData("PL", "POL", 616, "Poland", 1, 0, 0, 0, 1),
        FromData("PM", "SPM", 666, "St. Pierre and Miquelon", 0, 1, 0, 1, 1),
        FromData("PN", "PCN", 612, "Pitcairn Islands", 0, 1, 0, 1, 1),
        FromData("PR", "PRI", 630, "Puerto Rico", 0, 0, 0, 0, 0),
        FromData("PS", "PSE", 275, "State of Palestine", 0, 0, 0, 0, 0),
        FromData("PT", "PRT", 620, "Portugal", 1, 0, 0, 0, 1),
        FromData("PW", "PLW", 585, "Palau", 0, 0, 0, 0, 0),
        FromData("PY", "PRY", 600, "Paraguay", 0, 0, 0, 0, 0),
        FromData("QA", "QAT", 634, "Qatar", 0, 0, 0, 0, 0),
        FromData("RE", "REU", 638, "Reunion", 0, 1, 1, 0, 1),
        FromData("RO", "ROU", 642, "Romania", 1, 0, 0, 0, 1),
        FromData("RS", "SRB", 688, "Serbia", 0, 0, 0, 0, 0),
        FromData("RU", "RUS", 643, "Russia", 0, 0, 0, 0, 0),
        FromData("RW", "RWA", 646, "Rwanda", 0, 0, 0, 0, 0),
        FromData("SA", "SAU", 682, "Saudi Arabia", 0, 0, 0, 0, 0),
        FromData("SB", "SLB", 90, "Solomon Islands", 0, 0, 0, 0, 0),
        FromData("SC", "SYC", 690, "Seychelles", 0, 0, 0, 0, 0),
        FromData("SD", "SDN", 729, "Sudan", 0, 0, 0, 0, 0),
        FromData("SE", "SWE", 752, "Sweden", 1, 0, 0, 0, 1),
        FromData("SG", "SGP", 702, "Singapore", 0, 0, 0, 0, 0),
        FromData("SH", "SHN", 654, "St. Helena", 0, 1, 0, 1, 1),
        FromData("SI", "SVN", 705, "Slovenia", 1, 0, 0, 0, 1),
        FromData("SJ", "SJM", 744, "Svalbard and Jan Mayen", 0, 0, 0, 0, 0),
        FromData("SK", "SVK", 703, "Slovakia", 1, 0, 0, 0, 1),
        FromData("SL", "SLE", 694, "Sierra Leone", 0, 0, 0, 0, 0),
        FromData("SM", "SMR", 674, "San Marino", 0, 0, 0, 0, 0),
        FromData("SN", "SEN", 686, "Senegal", 0, 0, 0, 0, 0),
        FromData("SO", "SOM", 706, "Somalia", 0, 0, 0, 0, 0),
        FromData("SR", "SUR", 740, "Suriname", 0, 0, 0, 0, 0),
        FromData("SS", "SSD", 728, "Sudan", 0, 0, 0, 0, 0),
        FromData("ST", "STP", 678, "São Tomé and Príncipe", 0, 0, 0, 0, 0),
        FromData("SV", "SLV", 222, "El Salvador", 0, 0, 0, 0, 0),
        FromData("SX", "SXM", 534, "Sint Maarten", 0, 1, 0, 1, 1),
        FromData("SY", "SYR", 760, "Syria", 0, 0, 0, 0, 0),
        FromData("SZ", "SWZ", 748, "Swaziland", 0, 0, 0, 0, 0),
        FromData("TC", "TCA", 796, "Turks and Caicos Islands", 0, 1, 0, 1, 1),
        FromData("TD", "TCD", 148, "Chad", 0, 0, 0, 0, 0),
        FromData("TF", "ATF", 260, "French Southern and Antarctic Lands", 0, 1, 0, 1, 1),
        FromData("TG", "TGO", 768, "Togo", 0, 0, 0, 0, 0),
        FromData("TH", "THA", 764, "Thailand", 0, 0, 0, 0, 0),
        FromData("TJ", "TJK", 762, "Tajikistan", 0, 0, 0, 0, 0),
        FromData("TK", "TKL", 772, "Tokelau", 0, 0, 0, 0, 0),
        FromData("TL", "TLS", 626, "Timor-Leste", 0, 0, 0, 0, 0),
        FromData("TM", "TKM", 795, "Turkmenistan", 0, 0, 0, 0, 0),
        FromData("TN", "TUN", 788, "Tunisia", 0, 0, 0, 0, 0),
        FromData("TO", "TON", 776, "Tonga", 0, 0, 0, 0, 0),
        FromData("TR", "TUR", 792, "Turkey", 0, 0, 0, 0, 0),
        FromData("TT", "TTO", 780, "Trinidad and Tobago", 0, 0, 0, 0, 0),
        FromData("TV", "TUV", 798, "Tuvalu", 0, 0, 0, 0, 0),
        FromData("TW", "TWN", 158, "Taiwan", 0, 0, 0, 0, 0),
        FromData("TZ", "TZA", 834, "United Republic of Tanzania", 0, 0, 0, 0, 0),
        FromData("UA", "UKR", 804, "Ukraine", 0, 0, 0, 0, 0),
        FromData("UG", "UGA", 800, "Uganda", 0, 0, 0, 0, 0),
        FromData("UM", "UMI", 581, "U.S. Minor Outlying Islands", 0, 0, 0, 0, 0),
        FromData("US", "USA", 840, "United States", 0, 0, 0, 0, 0),
        FromData("UY", "URY", 858, "Uruguay", 0, 0, 0, 0, 0),
        FromData("UZ", "UZB", 860, "Uzbekistan", 0, 0, 0, 0, 0),
        FromData("VA", "VAT", 336, "Vatican City", 0, 0, 0, 0, 0),
        FromData("VC", "VCT", 670, "St. Vincent and the Grenadines", 0, 0, 0, 0, 0),
        FromData("VE", "VEN", 862, "Venezuela", 0, 0, 0, 0, 0),
        FromData("VG", "VGB", 92, "Virgin Islands, British", 0, 1, 0, 1, 1),
        FromData("VI", "VIR", 850, "Virgin Islands", 0, 0, 0, 0, 0),
        FromData("VN", "VNM", 704, "Viet Nam", 0, 0, 0, 0, 0),
        FromData("VU", "VUT", 548, "Vanuatu", 0, 0, 0, 0, 0),
        FromData("WF", "WLF", 876, "Wallis and Futuna", 0, 1, 0, 1, 1),
        FromData("WS", "WSM", 882, "Samoa", 0, 0, 0, 0, 0),
        FromData("XK", "XKX", 0, "Kosovo", 0, 0, 0, 0, 0),
        FromData("YE", "YEM", 887, "Yemen", 0, 0, 0, 0, 0),
        FromData("YT", "MYT", 175, "Mayotte", 0, 1, 1, 0, 1),
        FromData("ZA", "ZAF", 710, "South Africa", 0, 0, 0, 0, 0),
        FromData("ZM", "ZMB", 894, "Zambia", 0, 0, 0, 0, 0),
        FromData("ZW", "ZWE", 716, "Zimbabwe", 0, 0, 0, 0, 0)
    ];
}
