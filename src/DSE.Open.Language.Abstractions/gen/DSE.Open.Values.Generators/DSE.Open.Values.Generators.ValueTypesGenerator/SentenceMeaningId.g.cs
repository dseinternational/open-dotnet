﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


namespace DSE.Open.Language;

[global::System.ComponentModel.TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<SentenceMeaningId, ulong>))]
public readonly partial struct SentenceMeaningId
{
    private readonly ulong _value;

    private SentenceMeaningId(ulong value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(ulong value)
    {
        if (!IsValidValue(value))
        {
            throw new global::System.ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(SentenceMeaningId)} value");
        }
    }

    public static bool TryFromValue(ulong value, out SentenceMeaningId result)
    {
        if (IsValidValue(value))
        {
            result = new SentenceMeaningId(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static SentenceMeaningId FromValue(ulong value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator SentenceMeaningId(ulong value)
        => FromValue(value);

    static ulong global::DSE.Open.IConvertibleTo<SentenceMeaningId, ulong>.ConvertTo(SentenceMeaningId value)
        => (ulong)value;

    public static implicit operator ulong(SentenceMeaningId value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(SentenceMeaningId other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is SentenceMeaningId other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<SentenceMeaningId, SentenceMeaningId, bool>

    public static bool operator ==(SentenceMeaningId left, SentenceMeaningId right) => left.Equals(right);
    
    public static bool operator !=(SentenceMeaningId left, SentenceMeaningId right) => !(left == right);

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
    /// Gets a representation of the <see cref="SentenceMeaningId"/> value as a string with formatting options.
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

    public static SentenceMeaningId Parse(global::System.ReadOnlySpan<char> s, global::System.IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SentenceMeaningId, ulong>(s, provider);

    public static SentenceMeaningId ParseInvariant(global::System.ReadOnlySpan<char> s)
        => Parse(s, global::System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        global::System.ReadOnlySpan<char> s,
        global::System.IFormatProvider? provider,
        out SentenceMeaningId result)
        => global::DSE.Open.Values.ValueParser.TryParse<SentenceMeaningId, ulong>(s, provider, out result);

    public static bool TryParse(
        global::System.ReadOnlySpan<char> s,
        out SentenceMeaningId result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        global::System.ReadOnlySpan<char> s,
        out SentenceMeaningId result)
        => TryParse(s, global::System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<SentenceMeaningId>

    public static SentenceMeaningId Parse(string s, global::System.IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SentenceMeaningId, ulong>(s, provider);

    public static SentenceMeaningId Parse(string s)
        => Parse(s, default);

    public static SentenceMeaningId ParseInvariant(string s)
        => Parse(s, global::System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        global::System.IFormatProvider? provider,
        out SentenceMeaningId result)
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
        out SentenceMeaningId result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out SentenceMeaningId result)
        => TryParse(s, global::System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        global::System.Span<byte> utf8Destination,
        out int bytesWritten,
        global::System.ReadOnlySpan<char> format,
        global::System.IFormatProvider? provider)
        => ((global::System.IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<SentenceMeaningId>

    public static SentenceMeaningId Parse(
        global::System.ReadOnlySpan<byte> utf8Source,
        global::System.IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<SentenceMeaningId, ulong>(utf8Source, provider);

    public static bool TryParse(
        global::System.ReadOnlySpan<byte> utf8Source,
        global::System.IFormatProvider? provider,
        out SentenceMeaningId result)
        => global::DSE.Open.Values.ValueParser.TryParse<SentenceMeaningId, ulong>(utf8Source, provider, out result);

}

