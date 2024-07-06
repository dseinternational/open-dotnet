﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;

namespace DSE.Open.Values;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<Ratio, Double>))]
public readonly partial struct Ratio
{
    private readonly Double _value;

    private Ratio(Double value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(Double value)
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
            result = new Ratio(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static Ratio FromValue(Double value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator Ratio(Double value)
        => FromValue(value);

    static Double global::DSE.Open.IConvertibleTo<Ratio, Double>.ConvertTo(Ratio value)
        => (Double)value;

    public static implicit operator Double(Ratio value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(Ratio other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Ratio other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<Ratio, Ratio, bool>

    public static bool operator ==(Ratio left, Ratio right) => left.Equals(right);
    
    public static bool operator !=(Ratio left, Ratio right) => !(left == right);

    // ISpanFormattable

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return ((ISpanFormattable)_value).TryFormat(destination, out charsWritten, format, provider);
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
    /// Gets a representation of the <see cref="Ratio"/> value as a string with formatting options.
    /// </summary>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ((IFormattable)_value).ToString(format, formatProvider);
    }

    public string ToStringInvariant(string? format)
    {
        return ToString(format, System.Globalization.CultureInfo.InvariantCulture);
    }

    public string ToStringInvariant()
    {
        return ToStringInvariant(default);
    }

    /// <summary>
    /// Gets a representation of the Ratio value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Ratio value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<Ratio>

    public static Ratio Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Ratio, Double>(s, provider);

    public static Ratio ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

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

    // IParsable<Ratio>

    public static Ratio Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Ratio, Double>(s, provider);

    public static Ratio Parse(string s)
        => Parse(s, default);

    public static Ratio ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out Ratio result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }
    
        return TryParse(s.AsSpan(), provider, out result);
    }

    public static bool TryParse(
        string? s,
        out Ratio result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out Ratio result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<Ratio>

    public static Ratio Parse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Ratio, Double>(utf8Text, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out Ratio result)
        => global::DSE.Open.Values.ValueParser.TryParse<Ratio, Double>(utf8Text, provider, out result);

    public int CompareTo(Ratio other)
    {
        return _value.CompareTo(other._value);
    }

    // IComparisonOperators<Ratio, Ratio, bool>

    public static bool operator <(Ratio left, Ratio right) => left.CompareTo(right) < 0;
    
    public static bool operator >(Ratio left, Ratio right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(Ratio left, Ratio right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(Ratio left, Ratio right) => left.CompareTo(right) >= 0;

    // IAdditionOperators<Ratio, Ratio, Ratio>

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

