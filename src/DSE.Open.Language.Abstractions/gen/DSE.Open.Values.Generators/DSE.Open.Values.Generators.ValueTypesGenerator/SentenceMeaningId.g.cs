﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DSE.Open.Values;

namespace DSE.Open.Language;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<SentenceMeaningId, UInt64>))]
public readonly partial struct SentenceMeaningId
{
    private readonly UInt64 _value;

    private SentenceMeaningId(UInt64 value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(UInt64 value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(SentenceMeaningId)} value");
        }
    }

    private void EnsureIsNotDefault()
    {
        UninitializedValueException<SentenceMeaningId, UInt64>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(UInt64 value, out SentenceMeaningId result)
    {
        if (IsValidValue(value))
        {
            result = new SentenceMeaningId(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static SentenceMeaningId FromValue(UInt64 value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator SentenceMeaningId(UInt64 value)
        => FromValue(value);

    static UInt64 global::DSE.Open.IConvertibleTo<SentenceMeaningId, UInt64>.ConvertTo(SentenceMeaningId value)
        => (UInt64)value;

    public static implicit operator UInt64(SentenceMeaningId value)
    {
        value.EnsureIsNotDefault();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(SentenceMeaningId other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is SentenceMeaningId other && Equals(other);

    public override int GetHashCode()
    {
        EnsureIsNotDefault();
        return _value.GetHashCode();
    }

    // IEqualityOperators<SentenceMeaningId, SentenceMeaningId, bool>

    public static bool operator ==(SentenceMeaningId left, SentenceMeaningId right) => left.Equals(right);
    
    public static bool operator !=(SentenceMeaningId left, SentenceMeaningId right) => !(left == right);

    // ISpanFormattable

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        EnsureIsNotDefault();
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
    /// Gets a representation of the <see cref="SentenceMeaningId"/> value as a string with formatting options.
    /// </summary>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        EnsureIsNotDefault();

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
    /// Gets a representation of the SentenceMeaningId value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the SentenceMeaningId value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<SentenceMeaningId>

    public static SentenceMeaningId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SentenceMeaningId, UInt64>(s, provider);

    public static SentenceMeaningId ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out SentenceMeaningId result)
        => global::DSE.Open.Values.ValueParser.TryParse<SentenceMeaningId, UInt64>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out SentenceMeaningId result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out SentenceMeaningId result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<SentenceMeaningId>

    public static SentenceMeaningId Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SentenceMeaningId, UInt64>(s, provider);

    public static SentenceMeaningId Parse(string s)
        => Parse(s, default);

    public static SentenceMeaningId ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out SentenceMeaningId result)
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
        out SentenceMeaningId result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out SentenceMeaningId result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<SentenceMeaningId>

    public static SentenceMeaningId Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SentenceMeaningId, UInt64>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out SentenceMeaningId result)
        => global::DSE.Open.Values.ValueParser.TryParse<SentenceMeaningId, UInt64>(utf8Source, provider, out result);

}

