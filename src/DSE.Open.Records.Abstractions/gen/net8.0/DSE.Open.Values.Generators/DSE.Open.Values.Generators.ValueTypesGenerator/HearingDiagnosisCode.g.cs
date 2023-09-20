﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Records;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<HearingDiagnosisCode, ClinicalConceptCode>))]
public readonly partial struct HearingDiagnosisCode
{

    private readonly ClinicalConceptCode _value;
    private readonly bool _initialized;

    private HearingDiagnosisCode(ClinicalConceptCode value, bool skipValidation = false)
    {

        if (!skipValidation)
        {
            EnsureIsValidArgumentValue(value);
        }

        _value = value;
        _initialized = true;
    }

    public bool IsInitialized => _initialized;

    private static void EnsureIsValidArgumentValue(ClinicalConceptCode value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(HearingDiagnosisCode)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<HearingDiagnosisCode, ClinicalConceptCode>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(ClinicalConceptCode value, out HearingDiagnosisCode result)
    {
        if (IsValidValue(value))
        {
            result = new HearingDiagnosisCode(value);
            return true;
        }
    
        result = default;
        return false;
    }

    public static HearingDiagnosisCode FromValue(ClinicalConceptCode value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator HearingDiagnosisCode(ClinicalConceptCode value)
        => FromValue(value);

    static ClinicalConceptCode global::DSE.Open.IConvertibleTo<HearingDiagnosisCode, ClinicalConceptCode>.ConvertTo(HearingDiagnosisCode value)
        => (ClinicalConceptCode)value;

    public static explicit operator ClinicalConceptCode(HearingDiagnosisCode value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(HearingDiagnosisCode other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is HearingDiagnosisCode other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(HearingDiagnosisCode left, HearingDiagnosisCode right) => left.Equals(right);
    
    public static bool operator !=(HearingDiagnosisCode left, HearingDiagnosisCode right) => !(left == right);

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
    /// Gets a representation of the <see cref="HearingDiagnosisCode"/> value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the <see cref="HearingDiagnosisCode"/> value.
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
    /// Gets a representation of the HearingDiagnosisCode value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the HearingDiagnosisCode value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out HearingDiagnosisCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<HearingDiagnosisCode, ClinicalConceptCode>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out HearingDiagnosisCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out HearingDiagnosisCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out HearingDiagnosisCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<HearingDiagnosisCode, ClinicalConceptCode>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out HearingDiagnosisCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out HearingDiagnosisCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static HearingDiagnosisCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<HearingDiagnosisCode, ClinicalConceptCode>(s, provider);

    public static HearingDiagnosisCode Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static HearingDiagnosisCode Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<HearingDiagnosisCode, ClinicalConceptCode>(s, provider);

    public static HearingDiagnosisCode Parse(string s)
        => Parse(s, default);

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => _value.TryFormat(utf8Destination, out bytesWritten, format, provider);

    public static HearingDiagnosisCode Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
    => global::DSE.Open.Values.ValueParser.Parse<HearingDiagnosisCode, ClinicalConceptCode>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out HearingDiagnosisCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<HearingDiagnosisCode, ClinicalConceptCode>(utf8Source, provider, out result);
}
