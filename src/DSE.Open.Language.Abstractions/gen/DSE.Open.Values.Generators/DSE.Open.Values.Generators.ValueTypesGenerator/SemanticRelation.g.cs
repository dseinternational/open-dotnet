﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


namespace DSE.Open.Language;

[global::System.ComponentModel.TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<SemanticRelation, global::DSE.Open.AsciiString>))]
public readonly partial struct SemanticRelation
{
    private readonly global::DSE.Open.AsciiString _value;

    private SemanticRelation(global::DSE.Open.AsciiString value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(global::DSE.Open.AsciiString value)
    {
        if (!IsValidValue(value))
        {
            throw new global::System.ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(SemanticRelation)} value");
        }
    }

    public static bool TryFromValue(global::DSE.Open.AsciiString value, out SemanticRelation result)
    {
        if (IsValidValue(value))
        {
            result = new SemanticRelation(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static SemanticRelation FromValue(global::DSE.Open.AsciiString value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator SemanticRelation(global::DSE.Open.AsciiString value)
        => FromValue(value);

    static global::DSE.Open.AsciiString global::DSE.Open.IConvertibleTo<SemanticRelation, global::DSE.Open.AsciiString>.ConvertTo(SemanticRelation value)
        => (global::DSE.Open.AsciiString)value;

    public static implicit operator global::DSE.Open.AsciiString(SemanticRelation value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(SemanticRelation other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is SemanticRelation other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<SemanticRelation, SemanticRelation, bool>

    public static bool operator ==(SemanticRelation left, SemanticRelation right) => left.Equals(right);
    
    public static bool operator !=(SemanticRelation left, SemanticRelation right) => !(left == right);

    // ISpanFormattable

    public bool TryFormat(
        global::System.Span<char> destination,
        out int charsWritten,
        global::System.ReadOnlySpan<char> format,
        global::System.IFormatProvider? provider)
    {
        return ((global::System.ISpanFormattable)_value).TryFormat(destination, out charsWritten, format, provider);
    }

    public bool TryFormat(
        global::System.Span<char> destination,
        out int charsWritten)
        => TryFormat(destination, out charsWritten, default, default);

    public bool TryFormatInvariant(
        global::System.Span<char> destination,
        out int charsWritten,
        global::System.Span<char> format)
        => TryFormat(destination, out charsWritten, format, global::System.Globalization.CultureInfo.InvariantCulture);

    public bool TryFormatInvariant(
        global::System.Span<char> destination,
        out int charsWritten)
        => TryFormatInvariant(destination, out charsWritten, default);

    /// <summary>
    /// Gets a representation of the <see cref="SemanticRelation"/> value as a string with formatting options.
    /// </summary>
    [global::System.Runtime.CompilerServices.SkipLocalsInit]
    public string ToString(string? format, global::System.IFormatProvider? formatProvider)
    {
        return ((global::System.IFormattable)_value).ToString(format, formatProvider);
    }

    public string ToStringInvariant(string? format)
    {
        return ToString(format, global::System.Globalization.CultureInfo.InvariantCulture);
    }

    public string ToStringInvariant()
    {
        return ToStringInvariant(default);
    }

    /// <summary>
    /// Gets a representation of the SemanticRelation value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the SemanticRelation value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<SemanticRelation>

    public static SemanticRelation Parse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SemanticRelation, global::DSE.Open.AsciiString>(s, provider);

    public static SemanticRelation ParseInvariant(global::System.ReadOnlySpan<char> s)
        => Parse(s, global::System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        global::System.ReadOnlySpan<char> s,
        global::System.IFormatProvider? provider,
        out SemanticRelation result)
        => global::DSE.Open.Values.ValueParser.TryParse<SemanticRelation, global::DSE.Open.AsciiString>(s, provider, out result);

    public static bool TryParse(
        global::System.ReadOnlySpan<char> s,
        out SemanticRelation result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        global::System.ReadOnlySpan<char> s,
        out SemanticRelation result)
        => TryParse(s, global::System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<SemanticRelation>

    public static SemanticRelation Parse(string s, global::System.IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SemanticRelation, global::DSE.Open.AsciiString>(s, provider);

    public static SemanticRelation Parse(string s)
        => Parse(s, default);

    public static SemanticRelation ParseInvariant(string s)
        => Parse(s, global::System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        global::System.IFormatProvider? provider,
        out SemanticRelation result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }
    
        return TryParse(global::System.MemoryExtensions.AsSpan(s), provider, out result);
    }

    public static bool TryParse(
        string? s,
        out SemanticRelation result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out SemanticRelation result)
        => TryParse(s, global::System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        global::System.Span<byte> utf8Destination,
        out int bytesWritten,
        global::System.ReadOnlySpan<char> format,
        global::System.IFormatProvider? provider)
        => ((global::System.IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<SemanticRelation>

    public static SemanticRelation Parse(
        global::System.ReadOnlySpan<byte> utf8Source,
        global::System.IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SemanticRelation, global::DSE.Open.AsciiString>(utf8Source, provider);

    public static bool TryParse(
        global::System.ReadOnlySpan<byte> utf8Source,
        global::System.IFormatProvider? provider,
        out SemanticRelation result)
        => global::DSE.Open.Values.ValueParser.TryParse<SemanticRelation, global::DSE.Open.AsciiString>(utf8Source, provider, out result);

}

