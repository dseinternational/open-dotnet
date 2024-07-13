﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;

namespace DSE.Open.Records;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<MeasurementLevel, AsciiString>))]
public readonly partial struct MeasurementLevel
{
    private readonly AsciiString _value;

    private MeasurementLevel(AsciiString value, bool skipValidation = false)
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
                $"'{value}' is not a valid {nameof(MeasurementLevel)} value");
        }
    }

    public static bool TryFromValue(AsciiString value, out MeasurementLevel result)
    {
        if (IsValidValue(value))
        {
            result = new MeasurementLevel(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static MeasurementLevel FromValue(AsciiString value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator MeasurementLevel(AsciiString value)
        => FromValue(value);

    static AsciiString global::DSE.Open.IConvertibleTo<MeasurementLevel, AsciiString>.ConvertTo(MeasurementLevel value)
        => (AsciiString)value;

    public static implicit operator AsciiString(MeasurementLevel value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(MeasurementLevel other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is MeasurementLevel other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<MeasurementLevel, MeasurementLevel, bool>

    public static bool operator ==(MeasurementLevel left, MeasurementLevel right) => left.Equals(right);
    
    public static bool operator !=(MeasurementLevel left, MeasurementLevel right) => !(left == right);

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
    /// Gets a representation of the <see cref="MeasurementLevel"/> value as a string with formatting options.
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
    /// Gets a representation of the MeasurementLevel value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the MeasurementLevel value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<MeasurementLevel>

    public static MeasurementLevel Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<MeasurementLevel, AsciiString>(s, provider);

    public static MeasurementLevel ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out MeasurementLevel result)
        => global::DSE.Open.Values.ValueParser.TryParse<MeasurementLevel, AsciiString>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out MeasurementLevel result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out MeasurementLevel result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<MeasurementLevel>

    public static MeasurementLevel Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<MeasurementLevel, AsciiString>(s, provider);

    public static MeasurementLevel Parse(string s)
        => Parse(s, default);

    public static MeasurementLevel ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out MeasurementLevel result)
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
        out MeasurementLevel result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out MeasurementLevel result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<MeasurementLevel>

    public static MeasurementLevel Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<MeasurementLevel, AsciiString>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out MeasurementLevel result)
        => global::DSE.Open.Values.ValueParser.TryParse<MeasurementLevel, AsciiString>(utf8Source, provider, out result);

}

