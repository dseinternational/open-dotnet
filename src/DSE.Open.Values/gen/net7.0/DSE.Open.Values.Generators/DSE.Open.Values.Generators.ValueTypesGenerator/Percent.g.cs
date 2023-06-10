﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;

namespace DSE.Open.Values;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<Percent, Double>))]
public readonly partial struct Percent
    : global::DSE.Open.Values.IRatioValue<Percent, Double>
{

    private readonly Double _value;

    private Percent(Double value, bool skipValidation = false)
    {

        if (!skipValidation)
        {
            EnsureIsValidArgumentValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidArgumentValue(Double value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(Percent)} value");
        }
    }

    public static bool TryFromValue(Double value, out Percent result)
    {
        if (IsValidValue(value))
        {
            result = new Percent(value);
            return true;
        }
        
        result = default;
        return false;
    }

    public static Percent FromValue(Double value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator Percent(Double value)
        => FromValue(value);

    static Double global::DSE.Open.IConvertibleTo<Percent, Double>.ConvertTo(Percent value)
        => (Double)value;

    public static explicit operator Double(Percent value)
        => value._value;

    // IEquatable<T>

    public bool Equals(Percent other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Percent other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_value);

    public static bool operator ==(Percent left, Percent right) => left._value == right._value;
    
    public static bool operator !=(Percent left, Percent right) => left._value != right._value;

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
    /// Gets a representation of the Percent value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Percent value.
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
    /// Gets a representation of the Percent value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Percent value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Percent result)
        => global::DSE.Open.Values.ValueParser.TryParse<Percent, Double>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out Percent result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out Percent result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out Percent result)
        => global::DSE.Open.Values.ValueParser.TryParse<Percent, Double>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out Percent result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out Percent result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static Percent Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Percent, Double>(s, provider);

    public static Percent Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static Percent Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Percent, Double>(s, provider);

    public static Percent Parse(string s)
        => Parse(s, default);

    public int CompareTo(Percent other) => _value.CompareTo(other._value);

    public static bool operator <(Percent left, Percent right) => left._value < right._value;
    
    public static bool operator >(Percent left, Percent right) => left._value > right._value;
    
    public static bool operator <=(Percent left, Percent right) => left._value <= right._value;
    
    public static bool operator >=(Percent left, Percent right) => left._value >= right._value;

    public static Percent operator +(Percent left, Percent right) => (Percent)(left._value + right._value);

    public static Percent operator --(Percent value) => (Percent)(value._value - 1);

    public static Percent operator ++(Percent value) => (Percent)(value._value + 1);

    public static Percent operator -(Percent left, Percent right) => (Percent)(left._value - right._value);

    public static Percent operator +(Percent value) => (Percent)(+value._value);

    public static Percent operator -(Percent value) => (Percent)(-value._value);

    public static Percent operator *(Percent left, Percent right) => (Percent)(left._value * right._value);

    public static Percent operator /(Percent left, Percent right) => (Percent)(left._value / right._value);

    public static Percent operator %(Percent left, Percent right) => (Percent)(left._value % right._value);
}
