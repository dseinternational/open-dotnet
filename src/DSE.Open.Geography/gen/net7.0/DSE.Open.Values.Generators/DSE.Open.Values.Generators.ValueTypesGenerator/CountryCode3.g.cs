﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;

namespace DSE.Open.Geography;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<CountryCode3, AsciiChar3>))]
public readonly partial struct CountryCode3
    : global::DSE.Open.Values.INominalValue<CountryCode3, AsciiChar3>
{

    private readonly AsciiChar3 _value;

    private CountryCode3(AsciiChar3 value)
    {
        _value = value;
    }

    private static void EnsureIsValidValue(AsciiChar3 value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(CountryCode3)} value");
        }
    }

    public static bool TryFromValue(AsciiChar3 value, out CountryCode3 result)
    {
        if (IsValidValue(value))
        {
            result = new CountryCode3(value);
            return true;
        }
        
        result = default;
        return false;
    }

    public static CountryCode3 FromValue(AsciiChar3 value)
    {
        EnsureIsValidValue(value);
        return new(value);
    }

    public static explicit operator CountryCode3(AsciiChar3 value)
        => FromValue(value);

    static AsciiChar3 global::DSE.Open.IConvertibleTo<CountryCode3, AsciiChar3>.ConvertTo(CountryCode3 value)
        => (AsciiChar3)value;

    public static explicit operator AsciiChar3(CountryCode3 value)
        => value._value;

    // IEquatable<T>

    public bool Equals(CountryCode3 other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is CountryCode3 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_value);

    public static bool operator ==(CountryCode3 left, CountryCode3 right) => left._value == right._value;
    
    public static bool operator !=(CountryCode3 left, CountryCode3 right) => left._value != right._value;

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
    /// Gets a representation of the CountryCode3 value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the CountryCode3 value.
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
    /// Gets a representation of the CountryCode3 value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the CountryCode3 value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out CountryCode3 result)
        => global::DSE.Open.Values.ValueParser.TryParse<CountryCode3, AsciiChar3>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out CountryCode3 result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out CountryCode3 result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out CountryCode3 result)
        => global::DSE.Open.Values.ValueParser.TryParse<CountryCode3, AsciiChar3>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out CountryCode3 result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out CountryCode3 result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static CountryCode3 Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<CountryCode3, AsciiChar3>(s, provider);

    public static CountryCode3 Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static CountryCode3 Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<CountryCode3, AsciiChar3>(s, provider);

    public static CountryCode3 Parse(string s)
        => Parse(s, default);
}
