﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DSE.Open.Runtime.Helpers;

namespace DSE.Open.Values;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<UriPath, CharSequence>))]
public readonly partial struct UriPath
{
    private readonly CharSequence _value;

    private UriPath(CharSequence value, bool skipValidation = false)
    {
        if (!skipValidation)
        {
            EnsureIsValidValue(value);
        }

        _value = value;
    }

    private static void EnsureIsValidValue(CharSequence value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(UriPath)} value");
        }
    }

    public static bool TryFromValue(CharSequence value, out UriPath result)
    {
        if (IsValidValue(value))
        {
            result = new UriPath(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static UriPath FromValue(CharSequence value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator UriPath(CharSequence value)
        => FromValue(value);

    static CharSequence global::DSE.Open.IConvertibleTo<UriPath, CharSequence>.ConvertTo(UriPath value)
        => (CharSequence)value;

    public static implicit operator CharSequence(UriPath value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(UriPath other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is UriPath other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<UriPath, UriPath, bool>

    public static bool operator ==(UriPath left, UriPath right) => left.Equals(right);
    
    public static bool operator !=(UriPath left, UriPath right) => !(left == right);

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
    /// Gets a representation of the <see cref="UriPath"/> value as a string with formatting options.
    /// </summary>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        char[]? rented = null;
    
        try
        {
            Span<char> buffer = MemoryThresholds.CanStackalloc<char>(MaxSerializedCharLength)
                ? stackalloc char[MaxSerializedCharLength]
                : (rented = System.Buffers.ArrayPool<char>.Shared.Rent(MaxSerializedCharLength));
    
            _ = TryFormat(buffer, out var charsWritten, format, formatProvider);
    
            return GetString(buffer[..charsWritten]);
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
    {
        return ToString(format, System.Globalization.CultureInfo.InvariantCulture);
    }

    public string ToStringInvariant()
    {
        return ToStringInvariant(default);
    }

    /// <summary>
    /// Gets a representation of the UriPath value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the UriPath value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<UriPath>

    public static UriPath Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<UriPath, CharSequence>(s, provider);

    public static UriPath ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out UriPath result)
        => global::DSE.Open.Values.ValueParser.TryParse<UriPath, CharSequence>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out UriPath result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out UriPath result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<UriPath>

    public static UriPath Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<UriPath, CharSequence>(s, provider);

    public static UriPath Parse(string s)
        => Parse(s, default);

    public static UriPath ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out UriPath result)
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
        out UriPath result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out UriPath result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public int CompareTo(UriPath other)
    {
        return _value.CompareTo(other._value);
    }

    // IComparisonOperators<UriPath, UriPath, bool>

    public static bool operator <(UriPath left, UriPath right) => left.CompareTo(right) < 0;
    
    public static bool operator >(UriPath left, UriPath right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(UriPath left, UriPath right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(UriPath left, UriPath right) => left.CompareTo(right) >= 0;

}

