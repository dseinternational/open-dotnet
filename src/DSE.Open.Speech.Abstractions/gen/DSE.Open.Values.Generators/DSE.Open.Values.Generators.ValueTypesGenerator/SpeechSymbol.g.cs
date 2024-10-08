﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DSE.Open.Speech;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<SpeechSymbol, Char>))]
public readonly partial struct SpeechSymbol
{
    private readonly Char _value;

    private SpeechSymbol(Char value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(Char value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(SpeechSymbol)} value");
        }
    }

    public static bool TryFromValue(Char value, out SpeechSymbol result)
    {
        if (IsValidValue(value))
        {
            result = new SpeechSymbol(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static SpeechSymbol FromValue(Char value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator SpeechSymbol(Char value)
        => FromValue(value);

    static Char global::DSE.Open.IConvertibleTo<SpeechSymbol, Char>.ConvertTo(SpeechSymbol value)
        => (Char)value;

    public static implicit operator Char(SpeechSymbol value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(SpeechSymbol other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is SpeechSymbol other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<SpeechSymbol, SpeechSymbol, bool>

    public static bool operator ==(SpeechSymbol left, SpeechSymbol right) => left.Equals(right);
    
    public static bool operator !=(SpeechSymbol left, SpeechSymbol right) => !(left == right);

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
    /// Gets a representation of the <see cref="SpeechSymbol"/> value as a string with formatting options.
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
    /// Gets a representation of the SpeechSymbol value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the SpeechSymbol value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<SpeechSymbol>

    public static SpeechSymbol Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SpeechSymbol, Char>(s, provider);

    public static SpeechSymbol ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out SpeechSymbol result)
        => global::DSE.Open.Values.ValueParser.TryParse<SpeechSymbol, Char>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out SpeechSymbol result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out SpeechSymbol result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<SpeechSymbol>

    public static SpeechSymbol Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SpeechSymbol, Char>(s, provider);

    public static SpeechSymbol Parse(string s)
        => Parse(s, default);

    public static SpeechSymbol ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out SpeechSymbol result)
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
        out SpeechSymbol result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out SpeechSymbol result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<SpeechSymbol>

    public static SpeechSymbol Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SpeechSymbol, Char>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out SpeechSymbol result)
        => global::DSE.Open.Values.ValueParser.TryParse<SpeechSymbol, Char>(utf8Source, provider, out result);

    public int CompareTo(SpeechSymbol other)
    {
        return _value.CompareTo(other._value);
    }

    // IComparisonOperators<SpeechSymbol, SpeechSymbol, bool>

    public static bool operator <(SpeechSymbol left, SpeechSymbol right) => left.CompareTo(right) < 0;
    
    public static bool operator >(SpeechSymbol left, SpeechSymbol right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(SpeechSymbol left, SpeechSymbol right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(SpeechSymbol left, SpeechSymbol right) => left.CompareTo(right) >= 0;

}

