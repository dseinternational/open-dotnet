﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DSE.Open.Observations;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<Percent, Decimal>))]
public readonly partial struct Percent
{
    private readonly Decimal _value;

    private Percent(Decimal value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(Decimal value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(Percent)} value");
        }
    }

    public static bool TryFromValue(Decimal value, out Percent result)
    {
        if (IsValidValue(value))
        {
            result = new Percent(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static Percent FromValue(Decimal value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator Percent(Decimal value)
        => FromValue(value);

    static Decimal global::DSE.Open.IConvertibleTo<Percent, Decimal>.ConvertTo(Percent value)
        => (Decimal)value;

    public static implicit operator Decimal(Percent value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(Percent other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Percent other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<Percent, Percent, bool>

    public static bool operator ==(Percent left, Percent right) => left.Equals(right);
    
    public static bool operator !=(Percent left, Percent right) => !(left == right);

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
    /// Gets a representation of the <see cref="Percent"/> value as a string with formatting options.
    /// </summary>
    [SkipLocalsInit]
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
    /// Gets a representation of the Percent value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Percent value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<Percent>

    public static Percent Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Percent, Decimal>(s, provider);

    public static Percent ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Percent result)
        => global::DSE.Open.Values.ValueParser.TryParse<Percent, Decimal>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out Percent result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out Percent result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<Percent>

    public static Percent Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Percent, Decimal>(s, provider);

    public static Percent Parse(string s)
        => Parse(s, default);

    public static Percent ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out Percent result)
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
        out Percent result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out Percent result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<Percent>

    public static Percent Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Percent, Decimal>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out Percent result)
        => global::DSE.Open.Values.ValueParser.TryParse<Percent, Decimal>(utf8Source, provider, out result);

    public int CompareTo(Percent other)
    {
        return _value.CompareTo(other._value);
    }

    // IComparisonOperators<Percent, Percent, bool>

    public static bool operator <(Percent left, Percent right) => left.CompareTo(right) < 0;
    
    public static bool operator >(Percent left, Percent right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(Percent left, Percent right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(Percent left, Percent right) => left.CompareTo(right) >= 0;

    // IAdditionOperators<Percent, Percent, Percent>

    public static Percent operator +(Percent left, Percent right) => (Percent)(left._value + right._value);

    public static Percent operator --(Percent value) => (Percent)(value._value - 1);

    public static Percent operator ++(Percent value) => (Percent)(value._value + 1);

    public static Percent operator -(Percent left, Percent right) => (Percent)(left._value - right._value);

    public static Percent operator +(Percent value) => (Percent)(+value._value);

    public static Percent operator -(Percent value) => throw new NotImplementedException();

    public static Percent operator *(Percent left, Percent right) => (Percent)(left._value * right._value);

    public static Percent operator /(Percent left, Percent right) => (Percent)(left._value / right._value);

    public static Percent operator %(Percent left, Percent right) => (Percent)(left._value % right._value);

}

