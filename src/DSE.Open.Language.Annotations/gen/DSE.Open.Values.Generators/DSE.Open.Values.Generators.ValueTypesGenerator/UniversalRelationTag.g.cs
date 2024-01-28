﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;

namespace DSE.Open.Language.Annotations;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<UniversalRelationTag, AsciiString>))]
public readonly partial struct UniversalRelationTag
{
    private readonly AsciiString _value;

    private UniversalRelationTag(AsciiString value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(AsciiString value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(UniversalRelationTag)} value");
        }
    }

    public static bool TryFromValue(AsciiString value, out UniversalRelationTag result)
    {
        if (IsValidValue(value))
        {
            result = new UniversalRelationTag(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static UniversalRelationTag FromValue(AsciiString value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator UniversalRelationTag(AsciiString value)
        => FromValue(value);

    static AsciiString global::DSE.Open.IConvertibleTo<UniversalRelationTag, AsciiString>.ConvertTo(UniversalRelationTag value)
        => (AsciiString)value;

    public static implicit operator AsciiString(UniversalRelationTag value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(UniversalRelationTag other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is UniversalRelationTag other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<UniversalRelationTag, UniversalRelationTag, bool>

    public static bool operator ==(UniversalRelationTag left, UniversalRelationTag right) => left.Equals(right);
    
    public static bool operator !=(UniversalRelationTag left, UniversalRelationTag right) => !(left == right);

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
    /// Gets a representation of the <see cref="UniversalRelationTag"/> value as a string with formatting options.
    /// </summary>
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
    /// Gets a representation of the UniversalRelationTag value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the UniversalRelationTag value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<UniversalRelationTag>

    public static UniversalRelationTag Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<UniversalRelationTag, AsciiString>(s, provider);

    public static UniversalRelationTag ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out UniversalRelationTag result)
        => global::DSE.Open.Values.ValueParser.TryParse<UniversalRelationTag, AsciiString>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out UniversalRelationTag result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out UniversalRelationTag result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<UniversalRelationTag>

    public static UniversalRelationTag Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<UniversalRelationTag, AsciiString>(s, provider);

    public static UniversalRelationTag Parse(string s)
        => Parse(s, default);

    public static UniversalRelationTag ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out UniversalRelationTag result)
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
        out UniversalRelationTag result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out UniversalRelationTag result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<UniversalRelationTag>

    public static UniversalRelationTag Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<UniversalRelationTag, AsciiString>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out UniversalRelationTag result)
        => global::DSE.Open.Values.ValueParser.TryParse<UniversalRelationTag, AsciiString>(utf8Source, provider, out result);

}

