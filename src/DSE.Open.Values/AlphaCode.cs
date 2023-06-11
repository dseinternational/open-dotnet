// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values;

[ComparableValue]
[JsonConverter(typeof(JsonSpanSerializableValueConverter<AlphaCode,AsciiString>))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AlphaCode : IComparableValue<AlphaCode, AsciiString>
{
    public static readonly AlphaCode Empty;

    static int ISpanSerializable<AlphaCode>.MaxSerializedCharLength { get; } = MaxLength;

    public const int MaxLength = 32;

    public static bool IsValidValue(AsciiString value)
        => value is { IsEmpty: false, Length: <= MaxLength } && value.AsSpan().ContainsOnly(AsciiChar.IsLetter);

    public int CompareToCaseInsensitive(AlphaCode other) => _value.CompareToCaseInsensitive(other._value);

    public ReadOnlySpan<AsciiChar> AsSpan() => _value.AsSpan();

    public char[] ToCharArray() => _value.ToCharArray();

#pragma warning disable CA2225 // Operator overloads have named alternates

    public static explicit operator AlphaCode(string value) => Parse(value, CultureInfo.InvariantCulture);

#pragma warning restore CA2225 // Operator overloads have named alternates
}
