﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Observations;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<YesNo, AsciiString>))]
public readonly partial struct YesNo
{

    private readonly AsciiString _value;
    private readonly bool _initialized;

    private YesNo(AsciiString value, bool skipValidation = false)
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
                $"'{value}' is not a valid {nameof(YesNo)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<YesNo, AsciiString>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(AsciiString value, out YesNo result)
    {
        if (IsValidValue(value))
        {
            result = new YesNo(value);
            return true;
        }
    
        result = default;
        return false;
    }

    public static YesNo FromValue(AsciiString value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator YesNo(AsciiString value)
        => FromValue(value);

    static AsciiString global::DSE.Open.IConvertibleTo<YesNo, AsciiString>.ConvertTo(YesNo value)
        => (AsciiString)value;

    public static explicit operator AsciiString(YesNo value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(YesNo other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is YesNo other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(YesNo left, YesNo right) => left.Equals(right);
    
    public static bool operator !=(YesNo left, YesNo right) => !(left == right);

    // ISpanFormattable

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        {
            EnsureInitialized();
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
    /// Gets a representation of the <see cref="YesNo"/> value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the <see cref="YesNo"/> value.
    /// </returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        EnsureInitialized();    
        return _value.ToString(format, formatProvider);
    }

    public string ToStringInvariant(string? format)
        => ToString(format, System.Globalization.CultureInfo.InvariantCulture);

    public string ToStringInvariant()
        => ToStringInvariant(default);

    /// <summary>
    /// Gets a representation of the YesNo value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the YesNo value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out YesNo result)
        => global::DSE.Open.Values.ValueParser.TryParse<YesNo, AsciiString>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out YesNo result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out YesNo result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out YesNo result)
        => global::DSE.Open.Values.ValueParser.TryParse<YesNo, AsciiString>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out YesNo result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out YesNo result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static YesNo Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<YesNo, AsciiString>(s, provider);

    public static YesNo Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static YesNo Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<YesNo, AsciiString>(s, provider);

    public static YesNo Parse(string s)
        => Parse(s, default);

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => _value.TryFormat(utf8Destination, out bytesWritten, format, provider);

    public static YesNo Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
    => global::DSE.Open.Values.ValueParser.Parse<YesNo, AsciiString>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out YesNo result)
        => global::DSE.Open.Values.ValueParser.TryParse<YesNo, AsciiString>(utf8Source, provider, out result);
}
