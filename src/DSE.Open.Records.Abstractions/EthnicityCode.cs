// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Collections.Frozen;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Records;

[StructLayout(LayoutKind.Sequential)]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<EthnicityCode, short>))]
[EquatableValue]
public readonly partial struct EthnicityCode : IEquatableValue<EthnicityCode, short>,
    ISpanSerializableValue<EthnicityCode, short>
{
    public static int MaxSerializedCharLength => 64; // Some overhead.

    private EthnicityCode(short code)
    {
        _value = code;
    }

    public static bool IsValidValue(short value)
    {
        return value is > 100 and < 1000 && s_validCodes.ContainsKey(value);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return s_labelLookup[this];
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        var code = s_labelLookup[this];

        if (code.TryCopyTo(destination))
        {
            charsWritten = code.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out EthnicityCode result)
    {
        foreach (var (label, code) in s_valueLookup)
        {
            if (s.SequenceEqual(label))
            {
                result = code;
                return true;
            }
        }

        result = default;
        return false;
    }

    public static readonly EthnicityCode Empty;

    public static readonly EthnicityCode AmericanNativeNorthAmerica = new(105);
    public static readonly EthnicityCode AmericanNativeSouthAmerica = new(110);
    public static readonly EthnicityCode AmericanNativeOther = new(199);
    public static readonly EthnicityCode AsianBangladeshi = new(205);
    public static readonly EthnicityCode AsianChinese = new(210);
    public static readonly EthnicityCode AsianIndian = new(215);
    public static readonly EthnicityCode AsianJapanese = new(220);
    public static readonly EthnicityCode AsianMalaysia = new(225);
    public static readonly EthnicityCode AsianPakistani = new(230);
    public static readonly EthnicityCode AsianPhilippines = new(235);
    public static readonly EthnicityCode AsianThailand = new(240);
    public static readonly EthnicityCode AsianVietnamese = new(245);
    public static readonly EthnicityCode AsianOther = new(299);
    public static readonly EthnicityCode BlackAfrican = new(305);
    public static readonly EthnicityCode BlackAfricanAmerican = new(310);
    public static readonly EthnicityCode BlackCaribbean = new(315);
    public static readonly EthnicityCode BlackOther = new(399);
    public static readonly EthnicityCode HispanicCentralAmerica = new(405);
    public static readonly EthnicityCode HispanicNorthAmerica = new(410);
    public static readonly EthnicityCode HispanicSouthAmerica = new(415);
    public static readonly EthnicityCode HispanicOrLatino = new(420);
    public static readonly EthnicityCode HispanicOther = new(499);
    public static readonly EthnicityCode OceaniaAndPacificAustralianAboriginal = new(505);
    public static readonly EthnicityCode OceaniaAndPacificHawaii = new(510);
    public static readonly EthnicityCode OceaniaAndPacificNewZealandMāori = new(515);
    public static readonly EthnicityCode OceaniaAndPacificPolynesian = new(520);
    public static readonly EthnicityCode OceaniaAndPacificOther = new(599);
    public static readonly EthnicityCode WhiteMiddleEast = new(605);
    public static readonly EthnicityCode WhiteNorthAmerican = new(610);
    public static readonly EthnicityCode WhiteNorthEuropean = new(615);
    public static readonly EthnicityCode WhiteSouthEuropean = new(620);
    public static readonly EthnicityCode WhiteAustralian = new(625);
    public static readonly EthnicityCode WhiteNewZealander = new(630);
    public static readonly EthnicityCode WhiteOther = new(699);
    public static readonly EthnicityCode MixedWhiteAndAsian = new(805);
    public static readonly EthnicityCode MixedWhiteAndBlackAfrican = new(810);
    public static readonly EthnicityCode MixedWhiteAndBlackCaribbean = new(815);
    public static readonly EthnicityCode MixedAnyOtherMixedBackground = new(899);
    public static readonly EthnicityCode OtherAnyOtherEthnicBackground = new(999);

    private static readonly FrozenDictionary<short, EthnicityCode> s_validCodes = new Dictionary<short, EthnicityCode>
    {
        { AmericanNativeNorthAmerica._value, AmericanNativeNorthAmerica },
        { AmericanNativeSouthAmerica._value, AmericanNativeSouthAmerica },
        { AmericanNativeOther._value, AmericanNativeOther },
        { AsianBangladeshi._value, AsianBangladeshi },
        { AsianChinese._value, AsianChinese },
        { AsianIndian._value, AsianIndian },
        { AsianJapanese._value, AsianJapanese },
        { AsianMalaysia._value, AsianMalaysia },
        { AsianPakistani._value, AsianPakistani },
        { AsianPhilippines._value, AsianPhilippines },
        { AsianThailand._value, AsianThailand },
        { AsianVietnamese._value, AsianVietnamese },
        { AsianOther._value, AsianOther },
        { BlackAfrican._value, BlackAfrican },
        { BlackAfricanAmerican._value, BlackAfricanAmerican },
        { BlackCaribbean._value, BlackCaribbean },
        { BlackOther._value, BlackOther },
        { HispanicCentralAmerica._value, HispanicCentralAmerica },
        { HispanicNorthAmerica._value, HispanicNorthAmerica },
        { HispanicSouthAmerica._value, HispanicSouthAmerica },
        { HispanicOrLatino._value, HispanicOrLatino },
        { HispanicOther._value, HispanicOther },
        { OceaniaAndPacificAustralianAboriginal._value, OceaniaAndPacificAustralianAboriginal },
        { OceaniaAndPacificHawaii._value, OceaniaAndPacificHawaii },
        { OceaniaAndPacificNewZealandMāori._value, OceaniaAndPacificNewZealandMāori },
        { OceaniaAndPacificPolynesian._value, OceaniaAndPacificPolynesian },
        { OceaniaAndPacificOther._value, OceaniaAndPacificOther },
        { WhiteMiddleEast._value, WhiteMiddleEast },
        { WhiteNorthAmerican._value, WhiteNorthAmerican },
        { WhiteNorthEuropean._value, WhiteNorthEuropean },
        { WhiteSouthEuropean._value, WhiteSouthEuropean },
        { WhiteAustralian._value, WhiteAustralian },
        { WhiteNewZealander._value, WhiteNewZealander },
        { WhiteOther._value, WhiteOther },
        { MixedWhiteAndAsian._value, MixedWhiteAndAsian },
        { MixedWhiteAndBlackAfrican._value, MixedWhiteAndBlackAfrican },
        { MixedWhiteAndBlackCaribbean._value, MixedWhiteAndBlackCaribbean },
        { MixedAnyOtherMixedBackground._value, MixedAnyOtherMixedBackground },
        { OtherAnyOtherEthnicBackground._value, OtherAnyOtherEthnicBackground },
    }.ToFrozenDictionary();

    private static class Labels
    {
        public const string AmericanNativeNorthAmerica = "american_native_north_america";
        public const string AmericanNativeSouthAmerica = "american_native_south_america";
        public const string AmericanNativeOther = "american_native_other";
        public const string AsianBangladeshi = "asian_bangladeshi";
        public const string AsianChinese = "asian_chinese";
        public const string AsianIndian = "asian_indian";
        public const string AsianJapanese = "asian_japanese";
        public const string AsianMalaysia = "asian_malaysia";
        public const string AsianPakistani = "asian_pakistani";
        public const string AsianPhilippines = "asian_philippines";
        public const string AsianThailand = "asian_thailand";
        public const string AsianVietnamese = "asian_vietnamese";
        public const string AsianOther = "asian_other";
        public const string BlackAfrican = "black_african";
        public const string BlackAfricanAmerican = "black_african_american";
        public const string BlackCaribbean = "black_caribbean";
        public const string BlackOther = "black_other";
        public const string HispanicCentralAmerica = "hispanic_central_america";
        public const string HispanicNorthAmerica = "hispanic_north_america";
        public const string HispanicSouthAmerica = "hispanic_south_america";
        public const string HispanicOrLatino = "hispanic_or_latino";
        public const string HispanicOther = "hispanic_other";
        public const string OceaniaAndPacificAustralianAboriginal = "oceania_and_pacific_australian_aboriginal";
        public const string OceaniaAndPacificHawaii = "oceania_and_pacific_hawaii";
        public const string OceaniaAndPacificNewZealandMāori = "oceania_and_pacific_new_zealand_māori";
        public const string OceaniaAndPacificPolynesian = "oceania_and_pacific_polynesian";
        public const string OceaniaAndPacificOther = "oceania_and_pacific_other";
        public const string WhiteMiddleEast = "white_middle_east";
        public const string WhiteNorthAmerican = "white_north_american";
        public const string WhiteNorthEuropean = "white_north_european";
        public const string WhiteSouthEuropean = "white_south_european";
        public const string WhiteAustralian = "white_australian";
        public const string WhiteNewZealander = "white_new_zealander";
        public const string WhiteOther = "white_other";
        public const string MixedWhiteAndAsian = "mixed_white_and_asian";
        public const string MixedWhiteAndBlackAfrican = "mixed_white_and_black_african";
        public const string MixedWhiteAndBlackCaribbean = "mixed_white_and_black_caribbean";
        public const string MixedAnyOtherMixedBackground = "mixed_any_other_mixed_background";
        public const string OtherAnyOtherEthnicBackground = "other_any_other_ethnic_background";
    }

    private static readonly FrozenDictionary<EthnicityCode, string> s_labelLookup =
        new Dictionary<EthnicityCode, string>
        {
            { AmericanNativeNorthAmerica, Labels.AmericanNativeNorthAmerica },
            { AmericanNativeSouthAmerica, Labels.AmericanNativeSouthAmerica },
            { AmericanNativeOther, Labels.AmericanNativeOther },
            { AsianBangladeshi, Labels.AsianBangladeshi },
            { AsianChinese, Labels.AsianChinese },
            { AsianIndian, Labels.AsianIndian },
            { AsianJapanese, Labels.AsianJapanese },
            { AsianMalaysia, Labels.AsianMalaysia },
            { AsianPakistani, Labels.AsianPakistani },
            { AsianPhilippines, Labels.AsianPhilippines },
            { AsianThailand, Labels.AsianThailand },
            { AsianVietnamese, Labels.AsianVietnamese },
            { AsianOther, Labels.AsianOther },
            { BlackAfrican, Labels.BlackAfrican },
            { BlackAfricanAmerican, Labels.BlackAfricanAmerican },
            { BlackCaribbean, Labels.BlackCaribbean },
            { BlackOther, Labels.BlackOther },
            { HispanicCentralAmerica, Labels.HispanicCentralAmerica },
            { HispanicNorthAmerica, Labels.HispanicNorthAmerica },
            { HispanicSouthAmerica, Labels.HispanicSouthAmerica },
            { HispanicOrLatino, Labels.HispanicOrLatino },
            { HispanicOther, Labels.HispanicOther },
            { OceaniaAndPacificAustralianAboriginal, Labels.OceaniaAndPacificAustralianAboriginal },
            { OceaniaAndPacificHawaii, Labels.OceaniaAndPacificHawaii },
            { OceaniaAndPacificNewZealandMāori, Labels.OceaniaAndPacificNewZealandMāori },
            { OceaniaAndPacificPolynesian, Labels.OceaniaAndPacificPolynesian },
            { OceaniaAndPacificOther, Labels.OceaniaAndPacificOther },
            { WhiteMiddleEast, Labels.WhiteMiddleEast },
            { WhiteNorthAmerican, Labels.WhiteNorthAmerican },
            { WhiteNorthEuropean, Labels.WhiteNorthEuropean },
            { WhiteSouthEuropean, Labels.WhiteSouthEuropean },
            { WhiteAustralian, Labels.WhiteAustralian },
            { WhiteNewZealander, Labels.WhiteNewZealander },
            { WhiteOther, Labels.WhiteOther },
            { MixedWhiteAndAsian, Labels.MixedWhiteAndAsian },
            { MixedWhiteAndBlackAfrican, Labels.MixedWhiteAndBlackAfrican },
            { MixedWhiteAndBlackCaribbean, Labels.MixedWhiteAndBlackCaribbean },
            { MixedAnyOtherMixedBackground, Labels.MixedAnyOtherMixedBackground },
            { OtherAnyOtherEthnicBackground, Labels.OtherAnyOtherEthnicBackground },
        }.ToFrozenDictionary();

    private static readonly FrozenDictionary<string, EthnicityCode> s_valueLookup =
        new Dictionary<string, EthnicityCode>(StringComparer.Ordinal)
        {
            { Labels.AmericanNativeNorthAmerica, AmericanNativeNorthAmerica },
            { Labels.AmericanNativeSouthAmerica, AmericanNativeSouthAmerica },
            { Labels.AmericanNativeOther, AmericanNativeOther },
            { Labels.AsianBangladeshi, AsianBangladeshi },
            { Labels.AsianChinese, AsianChinese },
            { Labels.AsianIndian, AsianIndian },
            { Labels.AsianJapanese, AsianJapanese },
            { Labels.AsianMalaysia, AsianMalaysia },
            { Labels.AsianPakistani, AsianPakistani },
            { Labels.AsianPhilippines, AsianPhilippines },
            { Labels.AsianThailand, AsianThailand },
            { Labels.AsianVietnamese, AsianVietnamese },
            { Labels.AsianOther, AsianOther },
            { Labels.BlackAfrican, BlackAfrican },
            { Labels.BlackAfricanAmerican, BlackAfricanAmerican },
            { Labels.BlackCaribbean, BlackCaribbean },
            { Labels.BlackOther, BlackOther },
            { Labels.HispanicCentralAmerica, HispanicCentralAmerica },
            { Labels.HispanicNorthAmerica, HispanicNorthAmerica },
            { Labels.HispanicSouthAmerica, HispanicSouthAmerica },
            { Labels.HispanicOrLatino, HispanicOrLatino },
            { Labels.HispanicOther, HispanicOther },
            { Labels.OceaniaAndPacificAustralianAboriginal, OceaniaAndPacificAustralianAboriginal },
            { Labels.OceaniaAndPacificHawaii, OceaniaAndPacificHawaii },
            { Labels.OceaniaAndPacificNewZealandMāori, OceaniaAndPacificNewZealandMāori },
            { Labels.OceaniaAndPacificPolynesian, OceaniaAndPacificPolynesian },
            { Labels.OceaniaAndPacificOther, OceaniaAndPacificOther },
            { Labels.WhiteMiddleEast, WhiteMiddleEast },
            { Labels.WhiteNorthAmerican, WhiteNorthAmerican },
            { Labels.WhiteNorthEuropean, WhiteNorthEuropean },
            { Labels.WhiteSouthEuropean, WhiteSouthEuropean },
            { Labels.WhiteAustralian, WhiteAustralian },
            { Labels.WhiteNewZealander, WhiteNewZealander },
            { Labels.WhiteOther, WhiteOther },
            { Labels.MixedWhiteAndAsian, MixedWhiteAndAsian },
            { Labels.MixedWhiteAndBlackAfrican, MixedWhiteAndBlackAfrican },
            { Labels.MixedWhiteAndBlackCaribbean, MixedWhiteAndBlackCaribbean },
            { Labels.MixedAnyOtherMixedBackground, MixedAnyOtherMixedBackground },
            { Labels.OtherAnyOtherEthnicBackground, OtherAnyOtherEthnicBackground },
        }.ToFrozenDictionary();
}
