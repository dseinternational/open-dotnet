﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Values.Text;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<ContainsPattern, CharSequence>))]
public readonly partial struct ContainsPattern
{

    private readonly CharSequence _value;
    private readonly bool _initialized;

    private ContainsPattern(CharSequence value, bool skipValidation = false)
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
                $"'{value}' is not a valid {nameof(ContainsPattern)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<ContainsPattern, CharSequence>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(CharSequence value, out ContainsPattern result)
    {
        if (IsValidValue(value))
        {
            result = new ContainsPattern(value);
            return true;
        }
    
        result = default;
        return false;
    }

    public static ContainsPattern FromValue(CharSequence value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator ContainsPattern(CharSequence value)
        => FromValue(value);

    static CharSequence global::DSE.Open.IConvertibleTo<ContainsPattern, CharSequence>.ConvertTo(ContainsPattern value)
        => (CharSequence)value;

    public static explicit operator CharSequence(ContainsPattern value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(ContainsPattern other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is ContainsPattern other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(ContainsPattern left, ContainsPattern right) => left.Equals(right);
    
    public static bool operator !=(ContainsPattern left, ContainsPattern right) => !(left == right);

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
    /// Gets a representation of the <see cref="ContainsPattern"/> value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the <see cref="ContainsPattern"/> value.
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

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out ContainsPattern result)
        => global::DSE.Open.Values.ValueParser.TryParse<ContainsPattern, CharSequence>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out ContainsPattern result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out ContainsPattern result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out ContainsPattern result)
        => global::DSE.Open.Values.ValueParser.TryParse<ContainsPattern, CharSequence>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out ContainsPattern result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out ContainsPattern result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static ContainsPattern Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<ContainsPattern, CharSequence>(s, provider);

    public static ContainsPattern Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static ContainsPattern Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<ContainsPattern, CharSequence>(s, provider);

    public static ContainsPattern Parse(string s)
        => Parse(s, default);
}
