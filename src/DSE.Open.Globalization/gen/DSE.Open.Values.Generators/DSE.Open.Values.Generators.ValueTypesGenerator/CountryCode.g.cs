﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DSE.Open.Globalization;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<CountryCode, AsciiChar2>))]
public readonly partial struct CountryCode
{
    private readonly AsciiChar2 _value;

    private CountryCode(AsciiChar2 value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(AsciiChar2 value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(CountryCode)} value");
        }
    }

    public static bool TryFromValue(AsciiChar2 value, out CountryCode result)
    {
        if (IsValidValue(value))
        {
            result = new CountryCode(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static CountryCode FromValue(AsciiChar2 value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator CountryCode(AsciiChar2 value)
        => FromValue(value);

    static AsciiChar2 global::DSE.Open.IConvertibleTo<CountryCode, AsciiChar2>.ConvertTo(CountryCode value)
        => (AsciiChar2)value;

    public static implicit operator AsciiChar2(CountryCode value)
    {
        return value._value;
    }

    // IEquatable<T>

    public override bool Equals(object? obj) => obj is CountryCode other && Equals(other);

    // IEqualityOperators<CountryCode, CountryCode, bool>

    public static bool operator ==(CountryCode left, CountryCode right) => left.Equals(right);
    
    public static bool operator !=(CountryCode left, CountryCode right) => !(left == right);

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
    /// Gets a representation of the <see cref="CountryCode"/> value as a string with formatting options.
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
    /// Gets a representation of the CountryCode value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the CountryCode value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<CountryCode>

    public static CountryCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<CountryCode, AsciiChar2>(s, provider);

    public static CountryCode ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out CountryCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<CountryCode, AsciiChar2>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out CountryCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out CountryCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<CountryCode>

    public static CountryCode Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<CountryCode, AsciiChar2>(s, provider);

    public static CountryCode Parse(string s)
        => Parse(s, default);

    public static CountryCode ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out CountryCode result)
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
        out CountryCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out CountryCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<CountryCode>

    public static CountryCode Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<CountryCode, AsciiChar2>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out CountryCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<CountryCode, AsciiChar2>(utf8Source, provider, out result);

    // IComparisonOperators<CountryCode, CountryCode, bool>

    public static bool operator <(CountryCode left, CountryCode right) => left.CompareTo(right) < 0;
    
    public static bool operator >(CountryCode left, CountryCode right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(CountryCode left, CountryCode right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(CountryCode left, CountryCode right) => left.CompareTo(right) >= 0;

}

