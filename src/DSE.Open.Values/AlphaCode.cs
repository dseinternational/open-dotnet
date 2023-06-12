// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An immutable series of (up to 32) ASCII letters used to identify something.
/// </summary>
[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<AlphaCode, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AlphaCode : IComparableValue<AlphaCode, AsciiString>
{
    public static int MaxSerializedCharLength { get; } = MaxLength;

    public const int MaxLength = 32;

    public AlphaCode(string code) : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    public AlphaCode(ReadOnlySpan<char> code) : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    public AlphaCode(AsciiString code) : this(code, false)
    {
    }

    public int Length => _value.Length;

    private static string GetString(ReadOnlySpan<char> s) => CodeStringPool.Shared.GetOrAdd(s);

    public static bool IsValidValue(AsciiString value)
        => value is { IsEmpty: false, Length: <= MaxLength } && value.AsSpan().ContainsOnly(AsciiChar.IsLetter);

    public bool Equals(ReadOnlySpan<char> other) => _value.Equals(other);

    public bool Equals(string other) => _value.Equals(other);

    public int CompareToCaseInsensitive(AlphaCode other) => _value.CompareToCaseInsensitive(other._value);

    public ReadOnlySpan<AsciiChar> AsSpan() => _value.AsSpan();

    public char[] ToCharArray() => _value.ToCharArray();

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator AlphaCode(string value) => Parse(value, CultureInfo.InvariantCulture);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public string ToStringLower() => _value.ToStringLower();

    public string ToStringUpper() => _value.ToStringUpper();
}
