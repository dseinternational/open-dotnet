﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;

namespace DSE.Open.Values;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<Ratio, Double>))]
public readonly partial struct Ratio
    : global::DSE.Open.Values.IDivisibleValue<Ratio, Double>
{

    private readonly Double _value;

    private Ratio(Double value, bool skipValidation = false)
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
                $"'{value}' is not a valid {nameof(Ratio)} value");
        }
    }

    public static bool TryFromValue(Double value, out Ratio result)
    {
        if (IsValidValue(value))
        {
            result = new Ratio(value);
            return true;
        }
        
        result = default;
        return false;
    }

    public static Ratio FromValue(Double value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator Ratio(Double value)
        => FromValue(value);

    static Double global::DSE.Open.IConvertibleTo<Ratio, Double>.ConvertTo(Ratio value)
        => (Double)value;

    public static explicit operator Double(Ratio value)
        => value._value;

    // IEquatable<T>

    public bool Equals(Ratio other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Ratio other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(_value);

    public static bool operator ==(Ratio left, Ratio right) => left.Equals(right);
    
    public static bool operator !=(Ratio left, Ratio right) => !(left == right);

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
    /// Gets a representation of the Ratio value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Ratio value.
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
    /// Gets a representation of the Ratio value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Ratio value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Ratio result)
        => global::DSE.Open.Values.ValueParser.TryParse<Ratio, Double>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out Ratio result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out Ratio result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out Ratio result)
        => global::DSE.Open.Values.ValueParser.TryParse<Ratio, Double>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out Ratio result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out Ratio result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static Ratio Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Ratio, Double>(s, provider);

    public static Ratio Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static Ratio Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Ratio, Double>(s, provider);

    public static Ratio Parse(string s)
        => Parse(s, default);

    public int CompareTo(Ratio other) => _value.CompareTo(other._value);

    public static bool operator <(Ratio left, Ratio right) => left._value < right._value;
    
    public static bool operator >(Ratio left, Ratio right) => left._value > right._value;
    
    public static bool operator <=(Ratio left, Ratio right) => left._value <= right._value;
    
    public static bool operator >=(Ratio left, Ratio right) => left._value >= right._value;

    public static Ratio operator +(Ratio left, Ratio right) => (Ratio)(left._value + right._value);

    public static Ratio operator --(Ratio value) => (Ratio)(value._value - 1);

    public static Ratio operator ++(Ratio value) => (Ratio)(value._value + 1);

    public static Ratio operator -(Ratio left, Ratio right) => (Ratio)(left._value - right._value);

    public static Ratio operator +(Ratio value) => (Ratio)(+value._value);

    public static Ratio operator -(Ratio value) => (Ratio)(-value._value);

    public static Ratio operator *(Ratio left, Ratio right) => (Ratio)(left._value * right._value);

    public static Ratio operator /(Ratio left, Ratio right) => (Ratio)(left._value / right._value);

    public static Ratio operator %(Ratio left, Ratio right) => (Ratio)(left._value % right._value);
}
