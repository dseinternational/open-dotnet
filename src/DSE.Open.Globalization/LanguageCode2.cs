// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Text.Json.Serialization;
using DSE.Open.Hashing;
using DSE.Open.Values;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Globalization;

/// <summary>
/// An offically-assigned ISO 639-1 language code.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<LanguageCode2, AsciiChar2>))]
public readonly partial struct LanguageCode2
    : IComparableValue<LanguageCode2, AsciiChar2>,
      IUtf8SpanSerializable<LanguageCode2>,
      IRepeatableHash64
{
    /// <summary>The language code for English (en).</summary>
    public static readonly LanguageCode2 English = new((AsciiChar2)"en");

    /// <summary>
    /// Gets the set of all officially assigned ISO 639-1 language codes.
    /// </summary>
    public static IEnumerable<LanguageCode2> ValueSet => IsoLanguageCodes.OfficiallyAssignedAlpha2;

    /// <inheritdoc/>
    public static int MaxSerializedCharLength => 2;

    /// <inheritdoc/>
    public static int MaxSerializedByteLength => 2;

    /// <summary>
    /// Returns a value indicating whether the specified value is a valid (officially assigned)
    /// ISO 639-1 alpha-2 language code.
    /// </summary>
    /// <param name="value">The value to check.</param>
    public static bool IsValidValue(AsciiChar2 value)
    {
        return IsValidValue(value, true);
    }

    private static bool IsValidValue(AsciiChar2 value, bool normalize)
    {
        if (normalize)
        {
            value = value.ToLower();
        }

        return IsoLanguageCodes.OfficiallyAssignedAlpha2Ascii.Contains(value);
    }

    /// <summary>
    /// Creates a <see cref="LanguageCode2"/> from the specified <see cref="AsciiChar2"/> value
    /// without validation.
    /// </summary>
    /// <param name="value">The two-character ASCII value.</param>
    public static LanguageCode2 FromAsciiChar2(AsciiChar2 value)
    {
        return value.CastToValue<LanguageCode2, AsciiChar2>();
    }

    /// <inheritdoc/>
    public ulong GetRepeatableHashCode()
    {
        return _value.GetRepeatableHashCode();
    }
}
