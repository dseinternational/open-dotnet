// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Geography;

/// <summary>
/// An offically-assigned ISO 3166-1 alpha-3 country code.
/// </summary>
[NominalValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<CountryCode3, AsciiChar3>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct CountryCode3 : INominalValue<CountryCode3, AsciiChar3>
{
    public static readonly CountryCode3 Australia = new((AsciiChar3)"AUS");
    public static readonly CountryCode3 Canada = new((AsciiChar3)"CAN");
    public static readonly CountryCode3 Ireland = new((AsciiChar3)"IRL");
    public static readonly CountryCode3 NewZealand = new((AsciiChar3)"NZL");
    public static readonly CountryCode3 UnitedKingdom = new((AsciiChar3)"GBR");
    public static readonly CountryCode3 UnitedStates = new((AsciiChar3)"USA");

    public static int MaxSerializedCharLength => 3;

    public static IEnumerable<CountryCode3> ValueSet => IsoCountryCodes.OfficiallyAssignedAlpha3;

    public static bool IsValidValue(AsciiChar3 value) => IsValidValue(value, true);

    private static bool IsValidValue(AsciiChar3 value, bool normalize)
    {
        if (normalize)
        {
            value = value.ToUpper();
        }

        return IsoCountryCodes.OfficiallyAssignedAlpha3Ascii.Contains(value);
    }
}
