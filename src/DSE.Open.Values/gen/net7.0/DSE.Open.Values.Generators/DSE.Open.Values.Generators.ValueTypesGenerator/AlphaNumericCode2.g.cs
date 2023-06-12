﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Values;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<AlphaNumericCode2, AsciiString>))]
public readonly partial struct AlphaNumericCode2
    : global::DSE.Open.Values.IComparableValue<AlphaNumericCode2, AsciiString>
{

    private readonly AsciiString _value;
    private readonly bool _initialized;

    private AlphaNumericCode2(AsciiString value, bool skipValidation = false)
    {

        if (!skipValidation)
        {
            EnsureIsValidArgumentValue(value);
        }

        _value = value;
        _initialized = true;
    }

    public bool IsInitialized => _initialized;

    private static void EnsureIsValidArgumentValue(AsciiString value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(AlphaNumericCode2)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<AlphaNumericCode2, AsciiString>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(AsciiString value, out AlphaNumericCode2 result)
    {
        if (IsValidValue(value))
        {
            result = new AlphaNumericCode2(value);
            return true;
        }
        
        result = default;
        return false;
    }

    public static AlphaNumericCode2 FromValue(AsciiString value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator AlphaNumericCode2(AsciiString value)
        => FromValue(value);

    static AsciiString global::DSE.Open.IConvertibleTo<AlphaNumericCode2, AsciiString>.ConvertTo(AlphaNumericCode2 value)
        => (AsciiString)value;

    public static explicit operator AsciiString(AlphaNumericCode2 value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(AlphaNumericCode2 other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is AlphaNumericCode2 other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(AlphaNumericCode2 left, AlphaNumericCode2 right) => left.Equals(right);
    
    public static bool operator !=(AlphaNumericCode2 left, AlphaNumericCode2 right) => !(left == right);

    // ISpanFormattable

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        {
            EnsureInitialized();
            return _value.TryFormat(destination, out charsWritten, format, provider);
        }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten)
        => TryFormat(destination, out charsWritten, default, default);

    public bool TryFormatInvariant(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format)
        => TryFormat(destination, out charsWritten, format, System.Globalization.CultureInfo.InvariantCulture);

    public bool TryFormatInvariant(
        Span<char> destination,
        out int charsWritten)
        => TryFormatInvariant(destination, out charsWritten, default);

    /// <summary>
    /// Gets a representation of the AlphaNumericCode2 value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the AlphaNumericCode2 value.
    /// </returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var maxCharLength = MaxSerializedCharLength;
    
        char[]? rented = null;
    
        try
        {
            Span<char> buffer = maxCharLength <= 128
                ? stackalloc char[maxCharLength]
                : (rented = System.Buffers.ArrayPool<char>.Shared.Rent(maxCharLength));
    
            _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
    
            ReadOnlySpan<char> returnValue = buffer[..charsWritten];
            return GetString(returnValue);
        }
        finally
        {
            if (rented is not null)
            {
                System.Buffers.ArrayPool<char>.Shared.Return(rented);
            }
        }
    
    }

    public string ToStringInvariant(string? format)
        => ToString(format, System.Globalization.CultureInfo.InvariantCulture);

    public string ToStringInvariant()
        => ToStringInvariant(default);

    /// <summary>
    /// Gets a representation of the AlphaNumericCode2 value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the AlphaNumericCode2 value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AlphaNumericCode2 result)
        => global::DSE.Open.Values.ValueParser.TryParse<AlphaNumericCode2, AsciiString>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out AlphaNumericCode2 result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out AlphaNumericCode2 result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out AlphaNumericCode2 result)
        => global::DSE.Open.Values.ValueParser.TryParse<AlphaNumericCode2, AsciiString>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out AlphaNumericCode2 result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out AlphaNumericCode2 result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static AlphaNumericCode2 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<AlphaNumericCode2, AsciiString>(s, provider);

    public static AlphaNumericCode2 Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static AlphaNumericCode2 Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<AlphaNumericCode2, AsciiString>(s, provider);

    public static AlphaNumericCode2 Parse(string s)
        => Parse(s, default);

    public int CompareTo(AlphaNumericCode2 other)
    {
        EnsureInitialized();
        return _value.CompareTo(other._value);
    }

    public static bool operator <(AlphaNumericCode2 left, AlphaNumericCode2 right) => left.CompareTo(right) < 0;
    
    public static bool operator >(AlphaNumericCode2 left, AlphaNumericCode2 right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(AlphaNumericCode2 left, AlphaNumericCode2 right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(AlphaNumericCode2 left, AlphaNumericCode2 right) => left.CompareTo(right) >= 0;
}
