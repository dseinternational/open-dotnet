﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Language.Annotations;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<UniversalFeatureName, AsciiString>))]
public readonly partial struct UniversalFeatureName
{

    private readonly AsciiString _value;
    private readonly bool _initialized;

    private UniversalFeatureName(AsciiString value, bool skipValidation = false)
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
                $"'{value}' is not a valid {nameof(UniversalFeatureName)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<UniversalFeatureName, AsciiString>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(AsciiString value, out UniversalFeatureName result)
    {
        if (IsValidValue(value))
        {
            result = new UniversalFeatureName(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static UniversalFeatureName FromValue(AsciiString value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator UniversalFeatureName(AsciiString value)
        => FromValue(value);

    static AsciiString global::DSE.Open.IConvertibleTo<UniversalFeatureName, AsciiString>.ConvertTo(UniversalFeatureName value)
        => (AsciiString)value;

    public static explicit operator AsciiString(UniversalFeatureName value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(UniversalFeatureName other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is UniversalFeatureName other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(UniversalFeatureName left, UniversalFeatureName right) => left.Equals(right);
    
    public static bool operator !=(UniversalFeatureName left, UniversalFeatureName right) => !(left == right);

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
    /// Gets a representation of the <see cref="UniversalFeatureName"/> value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the <see cref="UniversalFeatureName"/> value.
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
    /// Gets a representation of the UniversalFeatureName value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the UniversalFeatureName value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out UniversalFeatureName result)
        => global::DSE.Open.Values.ValueParser.TryParse<UniversalFeatureName, AsciiString>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out UniversalFeatureName result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out UniversalFeatureName result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out UniversalFeatureName result)
        => global::DSE.Open.Values.ValueParser.TryParse<UniversalFeatureName, AsciiString>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out UniversalFeatureName result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out UniversalFeatureName result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static UniversalFeatureName Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<UniversalFeatureName, AsciiString>(s, provider);

    public static UniversalFeatureName Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static UniversalFeatureName ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static UniversalFeatureName Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<UniversalFeatureName, AsciiString>(s, provider);

    public static UniversalFeatureName Parse(string s)
        => Parse(s, default);

    public static UniversalFeatureName ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => _value.TryFormat(utf8Destination, out bytesWritten, format, provider);

    public bool TryFormatInvariant(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format)
        => _value.TryFormat(utf8Destination, out bytesWritten, format, System.Globalization.CultureInfo.InvariantCulture);

    public static UniversalFeatureName Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
    => global::DSE.Open.Values.ValueParser.Parse<UniversalFeatureName, AsciiString>(utf8Source, provider);

    public static UniversalFeatureName ParseInvariant(
        ReadOnlySpan<byte> utf8Source)
    => global::DSE.Open.Values.ValueParser.Parse<UniversalFeatureName, AsciiString>(utf8Source, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out UniversalFeatureName result)
        => global::DSE.Open.Values.ValueParser.TryParse<UniversalFeatureName, AsciiString>(utf8Source, provider, out result);
}
