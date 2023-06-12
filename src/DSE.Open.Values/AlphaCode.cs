// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<AlphaCode, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AlphaCode : IComparableValue<AlphaCode, AsciiString>
{
    static int ISpanSerializable<AlphaCode>.MaxSerializedCharLength { get; } = MaxLength;

    public const int MaxLength = 32;

    public AlphaCode(string code)
    {
        _value = Parse(code, CultureInfo.InvariantCulture)._value;
    }

    public AlphaCode(ReadOnlySpan<char> code)
    {
        _value = Parse(code, CultureInfo.InvariantCulture)._value;
    }

    public AlphaCode(AsciiString code) : this(code, false)
    {
    }

    public static bool IsValidValue(AsciiString value)
        => value is { IsEmpty: false, Length: <= MaxLength } && value.AsSpan().ContainsOnly(AsciiChar.IsLetter);

    public int CompareToCaseInsensitive(AlphaCode other) => _value.CompareToCaseInsensitive(other._value);

    public ReadOnlySpan<AsciiChar> AsSpan() => _value.AsSpan();

    public char[] ToCharArray() => _value.ToCharArray();

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator AlphaCode(string value) => Parse(value, CultureInfo.InvariantCulture);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public string ToStringLower() => _value.ToStringLower();

    public string ToStringUpper() => _value.ToStringUpper();
}
