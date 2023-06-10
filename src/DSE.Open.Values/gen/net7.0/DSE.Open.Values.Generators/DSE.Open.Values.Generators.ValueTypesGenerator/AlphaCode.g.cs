﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;

namespace DSE.Open.Values;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<AlphaCode, AsciiCharSequence>))]
public readonly partial struct AlphaCode
    : global::DSE.Open.Values.IOrdinalValue<AlphaCode, AsciiCharSequence>
{

    private readonly AsciiCharSequence _value;

    private AlphaCode(AsciiCharSequence value, bool skipValidation = false)
    {

        if (!skipValidation)
        {
            EnsureIsValidArgumentValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidArgumentValue(AsciiCharSequence value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(AlphaCode)} value");
        }
    }

    public static bool TryFromValue(AsciiCharSequence value, out AlphaCode result)
    {
        if (IsValidValue(value))
        {
            result = new AlphaCode(value);
            return true;
        }
        
        result = default;
        return false;
    }

    public static AlphaCode FromValue(AsciiCharSequence value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator AlphaCode(AsciiCharSequence value)
        => FromValue(value);

    static AsciiCharSequence global::DSE.Open.IConvertibleTo<AlphaCode, AsciiCharSequence>.ConvertTo(AlphaCode value)
        => (AsciiCharSequence)value;

    public static explicit operator AsciiCharSequence(AlphaCode value)
        => value._value;

    // IEquatable<T>

    public bool Equals(AlphaCode other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is AlphaCode other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_value);

    public static bool operator ==(AlphaCode left, AlphaCode right) => left._value == right._value;
    
    public static bool operator !=(AlphaCode left, AlphaCode right) => left._value != right._value;

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
    /// Gets a representation of the AlphaCode value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the AlphaCode value.
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
    /// Gets a representation of the AlphaCode value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the AlphaCode value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AlphaCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<AlphaCode, AsciiCharSequence>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out AlphaCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out AlphaCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out AlphaCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<AlphaCode, AsciiCharSequence>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out AlphaCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out AlphaCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static AlphaCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<AlphaCode, AsciiCharSequence>(s, provider);

    public static AlphaCode Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static AlphaCode Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<AlphaCode, AsciiCharSequence>(s, provider);

    public static AlphaCode Parse(string s)
        => Parse(s, default);

    public int CompareTo(AlphaCode other) => _value.CompareTo(other._value);

    public static bool operator <(AlphaCode left, AlphaCode right) => left._value < right._value;
    
    public static bool operator >(AlphaCode left, AlphaCode right) => left._value > right._value;
    
    public static bool operator <=(AlphaCode left, AlphaCode right) => left._value <= right._value;
    
    public static bool operator >=(AlphaCode left, AlphaCode right) => left._value >= right._value;
}
