﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Values;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<Tag, AsciiString>))]
public readonly partial struct Tag
{

    private readonly AsciiString _value;
    private readonly bool _initialized;

    private Tag(AsciiString value, bool skipValidation = false)
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
                $"'{value}' is not a valid {nameof(Tag)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<Tag, AsciiString>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(AsciiString value, out Tag result)
    {
        if (IsValidValue(value))
        {
            result = new Tag(value);
            return true;
        }
    
        result = default;
        return false;
    }

    public static Tag FromValue(AsciiString value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator Tag(AsciiString value)
        => FromValue(value);

    static AsciiString global::DSE.Open.IConvertibleTo<Tag, AsciiString>.ConvertTo(Tag value)
        => (AsciiString)value;

    public static explicit operator AsciiString(Tag value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(Tag other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Tag other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(Tag left, Tag right) => left.Equals(right);
    
    public static bool operator !=(Tag left, Tag right) => !(left == right);

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
    /// Gets a representation of the <see cref="Tag"/> value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the <see cref="Tag"/> value.
    /// </returns>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        EnsureInitialized();
        char[]? rented = null;
    
        try
        {
            Span<char> buffer = MaxSerializedCharLength <= 128
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
        => ToString(format, System.Globalization.CultureInfo.InvariantCulture);

    public string ToStringInvariant()
        => ToStringInvariant(default);

    /// <summary>
    /// Gets a representation of the Tag value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Tag value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Tag result)
        => global::DSE.Open.Values.ValueParser.TryParse<Tag, AsciiString>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out Tag result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out Tag result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out Tag result)
        => global::DSE.Open.Values.ValueParser.TryParse<Tag, AsciiString>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out Tag result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out Tag result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static Tag Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Tag, AsciiString>(s, provider);

    public static Tag Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static Tag Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Tag, AsciiString>(s, provider);

    public static Tag Parse(string s)
        => Parse(s, default);

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => _value.TryFormat(utf8Destination, out bytesWritten, format, provider);

    public static Tag Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
    => global::DSE.Open.Values.ValueParser.Parse<Tag, AsciiString>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out Tag result)
        => global::DSE.Open.Values.ValueParser.TryParse<Tag, AsciiString>(utf8Source, provider, out result);

    public int CompareTo(Tag other)
    {
        EnsureInitialized();
        return _value.CompareTo(other._value);
    }

    public static bool operator <(Tag left, Tag right) => left.CompareTo(right) < 0;
    
    public static bool operator >(Tag left, Tag right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(Tag left, Tag right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(Tag left, Tag right) => left.CompareTo(right) >= 0;
}
