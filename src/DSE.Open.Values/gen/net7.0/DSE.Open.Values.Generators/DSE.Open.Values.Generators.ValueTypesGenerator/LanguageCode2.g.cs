﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;

namespace DSE.Open.Globalization;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<LanguageCode2, AsciiChar2>))]
public readonly partial struct LanguageCode2
    : global::DSE.Open.Values.IOrdinalValue<LanguageCode2, AsciiChar2>
{

    private readonly AsciiChar2 _value;

    private LanguageCode2(AsciiChar2 value)
    {
        _value = value;
    }

    private static void EnsureIsValidValue(AsciiChar2 value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(LanguageCode2)} value");
        }
    }

    public static bool TryFromValue(AsciiChar2 value, out LanguageCode2 result)
    {
        if (IsValidValue(value))
        {
            result = new LanguageCode2(value);
            return true;
        }
        
        result = default;
        return false;
    }

    public static LanguageCode2 FromValue(AsciiChar2 value)
    {
        EnsureIsValidValue(value);
        return new(value);
    }

    public static explicit operator LanguageCode2(AsciiChar2 value)
        => FromValue(value);

    static AsciiChar2 global::DSE.Open.IConvertibleTo<LanguageCode2, AsciiChar2>.ConvertTo(LanguageCode2 value)
        => (AsciiChar2)value;

    public static explicit operator AsciiChar2(LanguageCode2 value)
        => value._value;

    // IEquatable<T>

    public bool Equals(LanguageCode2 other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is LanguageCode2 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_value);

    public static bool operator ==(LanguageCode2 left, LanguageCode2 right) => left._value == right._value;
    
    public static bool operator !=(LanguageCode2 left, LanguageCode2 right) => left._value != right._value;

    // ISpanFormattable

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        {
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
    /// Gets a representation of the LanguageCode2 value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the LanguageCode2 value.
    /// </returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        string returnValue;
        returnValue = _value.ToString(format, formatProvider);
        return returnValue;
    }

    public string ToStringInvariant(string? format)
        => ToString(format, System.Globalization.CultureInfo.InvariantCulture);

    public string ToStringInvariant()
        => ToStringInvariant(default);

    /// <summary>
    /// Gets a representation of the LanguageCode2 value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the LanguageCode2 value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out LanguageCode2 result)
        => global::DSE.Open.Values.ValueParser.TryParse<LanguageCode2, AsciiChar2>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out LanguageCode2 result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out LanguageCode2 result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out LanguageCode2 result)
        => global::DSE.Open.Values.ValueParser.TryParse<LanguageCode2, AsciiChar2>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out LanguageCode2 result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out LanguageCode2 result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static LanguageCode2 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<LanguageCode2, AsciiChar2>(s, provider);

    public static LanguageCode2 Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static LanguageCode2 Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<LanguageCode2, AsciiChar2>(s, provider);

    public static LanguageCode2 Parse(string s)
        => Parse(s, default);

    public int CompareTo(LanguageCode2 other) => _value.CompareTo(other._value);

    public static bool operator <(LanguageCode2 left, LanguageCode2 right) => left._value < right._value;
    
    public static bool operator >(LanguageCode2 left, LanguageCode2 right) => left._value > right._value;
    
    public static bool operator <=(LanguageCode2 left, LanguageCode2 right) => left._value <= right._value;
    
    public static bool operator >=(LanguageCode2 left, LanguageCode2 right) => left._value >= right._value;
}
