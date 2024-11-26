﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DSE.Open.Observations;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<Count, UInt64>))]
public readonly partial struct Count
{
    private readonly UInt64 _value;

    private static void EnsureIsValidValue(UInt64 value)
    {
        if (!IsValidValue(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{value}' is not a valid {nameof(Count)} value");
        }
    }

    public static bool TryFromValue(UInt64 value, out Count result)
    {
        if (IsValidValue(value))
        {
            result = new Count(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static Count FromValue(UInt64 value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator Count(UInt64 value)
        => FromValue(value);

    static UInt64 global::DSE.Open.IConvertibleTo<Count, UInt64>.ConvertTo(Count value)
        => (UInt64)value;

    public static implicit operator UInt64(Count value)
    {
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(Count other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Count other && Equals(other);

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    // IEqualityOperators<Count, Count, bool>

    public static bool operator ==(Count left, Count right) => left.Equals(right);
    
    public static bool operator !=(Count left, Count right) => !(left == right);

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
    /// Gets a representation of the <see cref="Count"/> value as a string with formatting options.
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
    /// Gets a representation of the Count value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Count value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<Count>

    public static Count Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Count, UInt64>(s, provider);

    public static Count ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Count result)
        => global::DSE.Open.Values.ValueParser.TryParse<Count, UInt64>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out Count result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out Count result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<Count>

    public static Count Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Count, UInt64>(s, provider);

    public static Count Parse(string s)
        => Parse(s, default);

    public static Count ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out Count result)
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
        out Count result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out Count result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IUtf8SpanFormattable

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => ((IUtf8SpanFormattable)_value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<Count>

    public static Count Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Count, UInt64>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out Count result)
        => global::DSE.Open.Values.ValueParser.TryParse<Count, UInt64>(utf8Source, provider, out result);

    public int CompareTo(Count other)
    {
        return _value.CompareTo(other._value);
    }

    // IComparisonOperators<Count, Count, bool>

    public static bool operator <(Count left, Count right) => left.CompareTo(right) < 0;
    
    public static bool operator >(Count left, Count right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(Count left, Count right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(Count left, Count right) => left.CompareTo(right) >= 0;

    // IAdditionOperators<Count, Count, Count>

    public static Count operator +(Count left, Count right) => (Count)(left._value + right._value);

    public static Count operator --(Count value) => (Count)(value._value - 1);

    public static Count operator ++(Count value) => (Count)(value._value + 1);

    public static Count operator -(Count left, Count right) => (Count)(left._value - right._value);

    public static Count operator +(Count value) => (Count)(+value._value);

    public static Count operator -(Count value) => throw new NotImplementedException();

    public static Count operator *(Count left, Count right) => (Count)(left._value * right._value);

    public static Count operator /(Count left, Count right) => (Count)(left._value / right._value);

    public static Count operator %(Count left, Count right) => (Count)(left._value % right._value);

}
