// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DSE.Open.Values.Text.Json.Serialization;

namespace DSE.Open.Values.TestValues;

// Defaults to 'unknown', but you could easily have it default to 'false' or 'true' instead.

// [JsonConverter(typeof(JsonByteValueConverter<NullableBinaryByteValue>))] -- would serialize 'unknown' to 0 rather than null

[JsonConverter(typeof(JsonNullableByteValueConverter<NullableBinaryByteValue>))]
public readonly record struct NullableBinaryByteValue : INullableValue<NullableBinaryByteValue, byte>
{
    public static readonly NullableBinaryByteValue Unknown;
    public static readonly NullableBinaryByteValue False = new(1);
    public static readonly NullableBinaryByteValue True = new(2);

    private readonly byte _value;

    private NullableBinaryByteValue(byte value)
    {
        _value = value;
    }

    public static bool IsValidValue(byte value)
    {
        throw new NotImplementedException();
    }

    public byte Value => _value;

    public bool HasValue => _value != Unknown._value;

    public static int MaxSerializedCharLength => 128;

    public bool IsInitialized => true;

    public static bool TryFromValue(byte value, out NullableBinaryByteValue result)
    {
        switch (value)
        {
            case 0:
                result = Unknown;
                return true;
            case 1:
                result = False;
                return true;
            case 2:
                result = True;
                return true;
            default:
                result = default;
                return false;
        }
    }

    public static implicit operator byte(NullableBinaryByteValue value)
    {
        return value.ToByte();
    }

    public static explicit operator NullableBinaryByteValue(byte value)
    {
        return FromByte(value);
    }

    public byte ToByte()
    {
        return _value;
    }

    public static NullableBinaryByteValue FromByte(byte value)
    {
        return value.CastToValue<NullableBinaryByteValue, byte>();
    }

    static byte IConvertibleTo<NullableBinaryByteValue, byte>.ConvertTo(NullableBinaryByteValue value)
    {
        return value._value;
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return _value.TryFormat(destination, out charsWritten, format, provider);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        throw new NotImplementedException();
    }

    public static NullableBinaryByteValue Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out NullableBinaryByteValue result)
    {
        throw new NotImplementedException();
    }

    public static NullableBinaryByteValue Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out NullableBinaryByteValue result)
    {
        throw new NotImplementedException();
    }
}
