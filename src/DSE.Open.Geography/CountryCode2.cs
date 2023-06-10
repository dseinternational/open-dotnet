// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Geography;

/// <summary>
/// An offically-assigned ISO 3166-1 alpha-2 country code.
/// </summary>
[NominalValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<CountryCode2, AsciiChar2>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct CountryCode2 : INominalValue<CountryCode2, AsciiChar2>
{
    public static readonly CountryCode2 Australia = new((AsciiChar2)"AU");
    public static readonly CountryCode2 Canada = new((AsciiChar2)"CA");
    public static readonly CountryCode2 NewZealand = new((AsciiChar2)"NZ");
    public static readonly CountryCode2 UnitedKingdom = new((AsciiChar2)"GB");
    public static readonly CountryCode2 UnitedStates = new((AsciiChar2)"US");

    public static int MaxSerializedCharLength => 2;

    public static IEnumerable<CountryCode2> ValueSet => IsoCountryCodes.OfficiallyAssignedAlpha2;

    public static bool IsValidValue(AsciiChar2 value) => IsValidValue(value, true);

    private static bool IsValidValue(AsciiChar2 value, bool normalize)
    {
        if (normalize)
        {
            value = value.ToUpper();
        }

        return IsoCountryCodes.OfficiallyAssignedAlpha2Ascii.Contains(value);
    }

    public AsciiChar2 ToAsciiChar2() => _value;

    public static CountryCode2 FromAsciiChar2(AsciiChar2 value) => value.CastToValue<CountryCode2, AsciiChar2>();

}
