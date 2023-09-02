﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Records.Abstractions;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<ConditionDiagnosisCode, ClinicalConceptCode>))]
public readonly partial struct ConditionDiagnosisCode
    : global::DSE.Open.Values.IEquatableValue<ConditionDiagnosisCode, ClinicalConceptCode>
{

    private readonly ClinicalConceptCode _value;
    private readonly bool _initialized;

    private ConditionDiagnosisCode(ClinicalConceptCode value, bool skipValidation = false)
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
                $"'{value}' is not a valid {nameof(ConditionDiagnosisCode)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<ConditionDiagnosisCode, ClinicalConceptCode>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(ClinicalConceptCode value, out ConditionDiagnosisCode result)
    {
        if (IsValidValue(value))
        {
            result = new ConditionDiagnosisCode(value);
            return true;
        }
        
        result = default;
        return false;
    }

    public static ConditionDiagnosisCode FromValue(ClinicalConceptCode value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator ConditionDiagnosisCode(ClinicalConceptCode value)
        => FromValue(value);

    static ClinicalConceptCode global::DSE.Open.IConvertibleTo<ConditionDiagnosisCode, ClinicalConceptCode>.ConvertTo(ConditionDiagnosisCode value)
        => (ClinicalConceptCode)value;

    public static explicit operator ClinicalConceptCode(ConditionDiagnosisCode value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(ConditionDiagnosisCode other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is ConditionDiagnosisCode other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(ConditionDiagnosisCode left, ConditionDiagnosisCode right) => left.Equals(right);
    
    public static bool operator !=(ConditionDiagnosisCode left, ConditionDiagnosisCode right) => !(left == right);

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
    /// Gets a representation of the ConditionDiagnosisCode value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the ConditionDiagnosisCode value.
    /// </returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var maxCharLength = MaxSerializedCharLength;
    
        char[]? rented = null;
    
        try
        {
            Span<char> buffer = maxCharLength <= 128
                ? stackalloc char[maxCharLength]
                : (rented = System.Buffers.ArrayPool<char>.Shared.Rent(maxCharLength));
    
            _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
    
            ReadOnlySpan<char> returnValue = buffer[..charsWritten];
            return new string(returnValue);
        }
        finally
        {
            if (rented is not null)
            {
                System.Buffers.ArrayPool<char>.Shared.Return(rented);
            }
        }
    
    }

    public string ToStringInvariant(string? format)
        => ToString(format, System.Globalization.CultureInfo.InvariantCulture);

    public string ToStringInvariant()
        => ToStringInvariant(default);

    /// <summary>
    /// Gets a representation of the ConditionDiagnosisCode value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the ConditionDiagnosisCode value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out ConditionDiagnosisCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<ConditionDiagnosisCode, ClinicalConceptCode>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out ConditionDiagnosisCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out ConditionDiagnosisCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out ConditionDiagnosisCode result)
        => global::DSE.Open.Values.ValueParser.TryParse<ConditionDiagnosisCode, ClinicalConceptCode>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out ConditionDiagnosisCode result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out ConditionDiagnosisCode result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static ConditionDiagnosisCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<ConditionDiagnosisCode, ClinicalConceptCode>(s, provider);

    public static ConditionDiagnosisCode Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static ConditionDiagnosisCode Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<ConditionDiagnosisCode, ClinicalConceptCode>(s, provider);

    public static ConditionDiagnosisCode Parse(string s)
        => Parse(s, default);
}
