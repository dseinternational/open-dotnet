// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An immutable series of (up to 32) ASCII letters or digits used to identify something.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<AlphaNumericCode, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AlphaNumericCode : IComparableValue<AlphaNumericCode, AsciiString>
{
    public static int MaxSerializedCharLength { get; } = MaxLength;

    public const int MaxLength = 32;

    public AlphaNumericCode(string code) : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    public AlphaNumericCode(ReadOnlySpan<char> code) : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    public AlphaNumericCode(AsciiString code) : this(code, false)
    {
    }

    public int Length => _value.Length;

    private static string GetString(ReadOnlySpan<char> s) => CodeStringPool.Shared.GetOrAdd(s);

    public static bool IsValidValue(AsciiString value)
        => value is { IsEmpty: false, Length: <= MaxLength } && value.AsSpan().ContainsOnly(AsciiChar.IsLetterOrDigit);

    public bool Equals(ReadOnlySpan<char> other) => _value.Equals(other);

    public bool Equals(string other) => _value.Equals(other);

    public int CompareToCaseInsensitive(AlphaNumericCode other) => _value.CompareToCaseInsensitive(other._value);

    public ReadOnlySpan<AsciiChar> AsSpan() => _value.AsSpan();

    public char[] ToCharArray() => _value.ToCharArray();

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator AlphaNumericCode(string value) => Parse(value, CultureInfo.InvariantCulture);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public string ToStringLower() => _value.ToStringLower();

    public string ToStringUpper() => _value.ToStringUpper();
}
