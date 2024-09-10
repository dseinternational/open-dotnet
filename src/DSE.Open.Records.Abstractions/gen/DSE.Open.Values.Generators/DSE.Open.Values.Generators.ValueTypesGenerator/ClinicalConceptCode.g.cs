﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DSE.Open.Records;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<ClinicalConceptCode, Int64>))]
public readonly partial struct ClinicalConceptCode
{
    private readonly Int64 _value;

    private ClinicalConceptCode(Int64 value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(Int64 value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(ClinicalConceptCode)} value");
        }
    }

    public static bool TryFromValue(Int64 value, out ClinicalConceptCode result)
    {
        if (IsValidValue(value))
        {
            result = new ClinicalConceptCode(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static ClinicalConceptCode FromValue(Int64 value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator ClinicalConceptCode(Int64 value)
        => FromValue(value);

    static Int64 global::DSE.Open.IConvertibleTo<ClinicalConceptCode, Int64>.ConvertTo(ClinicalConceptCode value)
        => (Int64)value;

    public static implicit operator Int64(ClinicalConceptCode value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(ClinicalConceptCode other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is ClinicalConceptCode other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<ClinicalConceptCode, ClinicalConceptCode, bool>

    public static bool operator ==(ClinicalConceptCode left, ClinicalConceptCode right) => left.Equals(right);
    
    public static bool operator !=(ClinicalConceptCode left, ClinicalConceptCode right) => !(left == right);

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
    /// Gets a representation of the <see cref="ClinicalConceptCode"/> value as a string with formatting options.
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
    /// Gets a representation of the ClinicalConceptCode value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the ClinicalConceptCode value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<ClinicalConceptCode>

    public static ClinicalConceptCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<ClinicalConceptCode, Int64>(s, provider);

    public static ClinicalConceptCode ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out ClinicalConceptCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<ClinicalConceptCode, Int64>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out ClinicalConceptCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out ClinicalConceptCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<ClinicalConceptCode>

    public static ClinicalConceptCode Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<ClinicalConceptCode, Int64>(s, provider);

    public static ClinicalConceptCode Parse(string s)
        => Parse(s, default);

    public static ClinicalConceptCode ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out ClinicalConceptCode result)
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
        out ClinicalConceptCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out ClinicalConceptCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<ClinicalConceptCode>

    public static ClinicalConceptCode Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<ClinicalConceptCode, Int64>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out ClinicalConceptCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<ClinicalConceptCode, Int64>(utf8Source, provider, out result);

}

