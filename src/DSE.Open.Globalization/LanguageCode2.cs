// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Globalization;

/// <summary>
/// An offically-assigned ISO 639-1 language code.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<LanguageCode2, AsciiChar2>))]
public readonly partial struct LanguageCode2
    : IComparableValue<LanguageCode2, AsciiChar2>, IUtf8SpanSerializable<LanguageCode2>
{
    public static readonly LanguageCode2 English = new((AsciiChar2)"en");

    public static IEnumerable<LanguageCode2> ValueSet => IsoLanguageCodes.OfficiallyAssignedAlpha2;

    public static int MaxSerializedCharLength => 2;

    public static int MaxSerializedByteLength => 2;

    public static bool IsValidValue(AsciiChar2 value) => IsValidValue(value, true);

    private static bool IsValidValue(AsciiChar2 value, bool normalize)
    {
        if (normalize)
        {
            value = value.ToLower();
        }

        return IsoLanguageCodes.OfficiallyAssignedAlpha2Ascii.Contains(value);
    }

    public static LanguageCode2 FromAsciiChar2(AsciiChar2 value) => value.CastToValue<LanguageCode2, AsciiChar2>();

}
