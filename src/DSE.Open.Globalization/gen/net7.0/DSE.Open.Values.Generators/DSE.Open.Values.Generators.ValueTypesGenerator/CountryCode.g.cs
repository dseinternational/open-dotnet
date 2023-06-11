﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Globalization;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<CountryCode, AsciiChar2>))]
public readonly partial struct CountryCode
    : global::DSE.Open.Values.IComparableValue<CountryCode, AsciiChar2>
{

    private readonly AsciiChar2 _value;
    private readonly bool _initialized;

    private CountryCode(AsciiChar2 value, bool skipValidation = false)
    {

        if (!skipValidation)
        {
            EnsureIsValidArgumentValue(value);
        }

        _value = value;
        _initialized = true;
    }

    public bool IsInitialized => _initialized;

    private static void EnsureIsValidArgumentValue(AsciiChar2 value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(CountryCode)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<CountryCode, AsciiChar2>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(AsciiChar2 value, out CountryCode result)
    {
        if (IsValidValue(value))
        {
            result = new CountryCode(value);
            return true;
        }
        
        result = default;
        return false;
    }

    public static CountryCode FromValue(AsciiChar2 value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator CountryCode(AsciiChar2 value)
        => FromValue(value);

    static AsciiChar2 global::DSE.Open.IConvertibleTo<CountryCode, AsciiChar2>.ConvertTo(CountryCode value)
        => (AsciiChar2)value;

    public static explicit operator AsciiChar2(CountryCode value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(CountryCode other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is CountryCode other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(CountryCode left, CountryCode right) => left.Equals(right);
    
    public static bool operator !=(CountryCode left, CountryCode right) => !(left == right);

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
    /// Gets a representation of the CountryCode value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the CountryCode value.
    /// </returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        EnsureInitialized();
        string returnValue;
        returnValue = _value.ToString(format, formatProvider);
        return returnValue;
    }

    public string ToStringInvariant(string? format)
        => ToString(format, System.Globalization.CultureInfo.InvariantCulture);

    public string ToStringInvariant()
        => ToStringInvariant(default);

    /// <summary>
    /// Gets a representation of the CountryCode value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the CountryCode value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

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

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out CountryCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<CountryCode, AsciiChar2>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out CountryCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out CountryCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static CountryCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<CountryCode, AsciiChar2>(s, provider);

    public static CountryCode Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static CountryCode Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<CountryCode, AsciiChar2>(s, provider);

    public static CountryCode Parse(string s)
        => Parse(s, default);

    public int CompareTo(CountryCode other)
    {
        EnsureInitialized();
        return _value.CompareTo(other._value);
    }

    public static bool operator <(CountryCode left, CountryCode right) => left.CompareTo(right) < 0;
    
    public static bool operator >(CountryCode left, CountryCode right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(CountryCode left, CountryCode right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(CountryCode left, CountryCode right) => left.CompareTo(right) >= 0;
}