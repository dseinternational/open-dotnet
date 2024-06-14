// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

/// <summary>
/// An immutable series of (up to 32) ASCII letters used to identify something.
/// </summary>
[ComparableValue(AllowDefaultValue = false)]
[JsonConverter(typeof(JsonUtf8SpanSerializableValueConverter<AlphaCode, AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AlphaCode
    : IComparableValue<AlphaCode, AsciiString>,
      IUtf8SpanSerializable<AlphaCode>
{
    public static int MaxSerializedCharLength => MaxLength;

    public static int MaxSerializedByteLength => MaxLength;

    public const int MaxLength = 32;

    public AlphaCode(string code)
        : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    public AlphaCode(ReadOnlySpan<char> code)
        : this(AsciiString.Parse(code, CultureInfo.InvariantCulture))
    {
    }

    public AlphaCode(AsciiString code)
        : this(code, false)
    {
    }

    public int Length => _value.Length;

    private static string GetString(ReadOnlySpan<char> s)
    {
        return CodeStringPool.Shared.GetOrAdd(s);
    }

    public static bool IsValidValue(AsciiString value)
    {
        return value is { IsEmpty: false, Length: <= MaxLength } && value.AsSpan().ContainsOnlyAsciiLetters();
    }

    public bool Equals(ReadOnlySpan<char> other)
    {
        return _value.Equals(other);
    }

    public bool EqualsIgnoreCase(ReadOnlySpan<char> other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    public bool Equals(string other)
    {
        return _value.Equals(other);
    }

    public bool EqualsIgnoreCase(string other)
    {
        return _value.EqualsIgnoreCase(other);
    }

    public int CompareToIgnoreCase(AlphaCode other)
    {
        return _value.CompareToIgnoreCase(other._value);
    }

    public ReadOnlySpan<AsciiChar> AsSpan()
    {
        return _value.AsSpan();
    }

    public char[] ToCharArray()
    {
        return _value.ToCharArray();
    }

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator AlphaCode(string value)
    {
        return Parse(value, CultureInfo.InvariantCulture);
    }

#pragma warning restore CA2225 // Operator overloads have named alternates

    public string ToStringLower()
    {
        return _value.ToStringLower();
    }

    public string ToStringUpper()
    {
        return _value.ToStringUpper();
    }
}
