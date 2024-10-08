﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DSE.Open.Observations;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<SpeechClarity, UInt32>))]
public readonly partial struct SpeechClarity
{
    private readonly UInt32 _value;

    private SpeechClarity(UInt32 value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(UInt32 value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(SpeechClarity)} value");
        }
    }

    public static bool TryFromValue(UInt32 value, out SpeechClarity result)
    {
        if (IsValidValue(value))
        {
            result = new SpeechClarity(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static SpeechClarity FromValue(UInt32 value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator SpeechClarity(UInt32 value)
        => FromValue(value);

    static UInt32 global::DSE.Open.IConvertibleTo<SpeechClarity, UInt32>.ConvertTo(SpeechClarity value)
        => (UInt32)value;

    public static implicit operator UInt32(SpeechClarity value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(SpeechClarity other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is SpeechClarity other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<SpeechClarity, SpeechClarity, bool>

    public static bool operator ==(SpeechClarity left, SpeechClarity right) => left.Equals(right);
    
    public static bool operator !=(SpeechClarity left, SpeechClarity right) => !(left == right);

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
    /// Gets a representation of the <see cref="SpeechClarity"/> value as a string with formatting options.
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
    /// Gets a representation of the SpeechClarity value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the SpeechClarity value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<SpeechClarity>

    public static SpeechClarity Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SpeechClarity, UInt32>(s, provider);

    public static SpeechClarity ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out SpeechClarity result)
        => global::DSE.Open.Values.ValueParser.TryParse<SpeechClarity, UInt32>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out SpeechClarity result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out SpeechClarity result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<SpeechClarity>

    public static SpeechClarity Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SpeechClarity, UInt32>(s, provider);

    public static SpeechClarity Parse(string s)
        => Parse(s, default);

    public static SpeechClarity ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out SpeechClarity result)
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
        out SpeechClarity result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out SpeechClarity result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<SpeechClarity>

    public static SpeechClarity Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SpeechClarity, UInt32>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out SpeechClarity result)
        => global::DSE.Open.Values.ValueParser.TryParse<SpeechClarity, UInt32>(utf8Source, provider, out result);

    public int CompareTo(SpeechClarity other)
    {
        return _value.CompareTo(other._value);
    }

    // IComparisonOperators<SpeechClarity, SpeechClarity, bool>

    public static bool operator <(SpeechClarity left, SpeechClarity right) => left.CompareTo(right) < 0;
    
    public static bool operator >(SpeechClarity left, SpeechClarity right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(SpeechClarity left, SpeechClarity right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(SpeechClarity left, SpeechClarity right) => left.CompareTo(right) >= 0;

}

