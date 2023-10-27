﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Language;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<Word, CharSequence>))]
public readonly partial struct Word
{

    private readonly CharSequence _value;
    private readonly bool _initialized;

    private Word(CharSequence value, bool skipValidation = false)
    {

        if (!skipValidation)
        {
            EnsureIsValidArgumentValue(value);
        }

        _value = value;
        _initialized = true;
    }

    public bool IsInitialized => _initialized;

    private static void EnsureIsValidArgumentValue(CharSequence value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(Word)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<Word, CharSequence>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(CharSequence value, out Word result)
    {
        if (IsValidValue(value))
        {
            result = new Word(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static Word FromValue(CharSequence value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator Word(CharSequence value)
        => FromValue(value);

    static CharSequence global::DSE.Open.IConvertibleTo<Word, CharSequence>.ConvertTo(Word value)
        => (CharSequence)value;

    public static explicit operator CharSequence(Word value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(Word other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Word other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(Word left, Word right) => left.Equals(right);
    
    public static bool operator !=(Word left, Word right) => !(left == right);

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
    /// Gets a representation of the <see cref="Word"/> value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the <see cref="Word"/> value.
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
    /// Gets a representation of the Word value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Word value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Word result)
        => global::DSE.Open.Values.ValueParser.TryParse<Word, CharSequence>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out Word result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out Word result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out Word result)
        => global::DSE.Open.Values.ValueParser.TryParse<Word, CharSequence>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out Word result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out Word result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static Word Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Word, CharSequence>(s, provider);

    public static Word Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static Word ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static Word Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Word, CharSequence>(s, provider);

    public static Word Parse(string s)
        => Parse(s, default);

    public static Word ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public int CompareTo(Word other)
    {
        EnsureInitialized();
        return _value.CompareTo(other._value);
    }

    public static bool operator <(Word left, Word right) => left.CompareTo(right) < 0;
    
    public static bool operator >(Word left, Word right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(Word left, Word right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(Word left, Word right) => left.CompareTo(right) >= 0;
}
