// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Globalization;

/// <summary>
/// An officially-assigned ISO 3166-1 alpha-3 country code.
/// </summary>
[EquatableValue]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<CountryCode3, AsciiChar3>))]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct CountryCode3
    : IEquatableValue<CountryCode3, AsciiChar3>,
      IUtf8SpanSerializable<CountryCode3>,
      IRepeatableHash64
{
    /// <summary>The alpha-3 country code for Australia (AUS).</summary>
    public static readonly CountryCode3 Australia = new((AsciiChar3)"AUS");
    /// <summary>The alpha-3 country code for Canada (CAN).</summary>
    public static readonly CountryCode3 Canada = new((AsciiChar3)"CAN");
    /// <summary>The alpha-3 country code for Ireland (IRL).</summary>
    public static readonly CountryCode3 Ireland = new((AsciiChar3)"IRL");
    /// <summary>The alpha-3 country code for New Zealand (NZL).</summary>
    public static readonly CountryCode3 NewZealand = new((AsciiChar3)"NZL");
    /// <summary>The alpha-3 country code for the United Kingdom (GBR).</summary>
    public static readonly CountryCode3 UnitedKingdom = new((AsciiChar3)"GBR");
    /// <summary>The alpha-3 country code for the United States (USA).</summary>
    public static readonly CountryCode3 UnitedStates = new((AsciiChar3)"USA");

    /// <inheritdoc/>
    public static int MaxSerializedCharLength => 3;

    /// <inheritdoc/>
    public static int MaxSerializedByteLength => 3;

    /// <summary>
    /// Gets the set of all officially assigned ISO 3166-1 alpha-3 country codes.
    /// </summary>
    public static IEnumerable<CountryCode3> ValueSet => IsoCountryCodes.OfficiallyAssignedAlpha3;

    static int ISpanSerializable<CountryCode3>.MaxSerializedCharLength => 3;

    /// <summary>
    /// Returns a value indicating whether the specified value is a valid (officially assigned)
    /// ISO 3166-1 alpha-3 country code.
    /// </summary>
    /// <param name="value">The value to check.</param>
    public static bool IsValidValue(AsciiChar3 value)
    {
        return IsValidValue(value, true);
    }

    private static bool IsValidValue(AsciiChar3 value, bool normalize)
    {
        if (normalize)
        {
            value = value.ToUpper();
        }

        return IsoCountryCodes.OfficiallyAssignedAlpha3Ascii.Contains(value);
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return _value.GetRepeatableHashCode();
    }
}
