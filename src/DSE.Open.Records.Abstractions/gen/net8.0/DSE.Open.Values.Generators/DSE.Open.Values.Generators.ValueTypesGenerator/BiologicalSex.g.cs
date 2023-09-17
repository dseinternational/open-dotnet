﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225

using System;
using System.ComponentModel;
using DSE.Open.Values;

namespace DSE.Open.Records;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<BiologicalSex, AsciiString>))]
public readonly partial struct BiologicalSex
{

    private readonly AsciiString _value;
    private readonly bool _initialized;

    private BiologicalSex(AsciiString value, bool skipValidation = false)
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
                $"'{value}' is not a valid {nameof(BiologicalSex)} value");
        }
    }

    private void EnsureInitialized()
    {
        UninitializedValueException<BiologicalSex, AsciiString>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(AsciiString value, out BiologicalSex result)
    {
        if (IsValidValue(value))
        {
            result = new BiologicalSex(value);
            return true;
        }
    
        result = default;
        return false;
    }

    public static BiologicalSex FromValue(AsciiString value)
    {
        EnsureIsValidArgumentValue(value);
        return new(value, true);
    }

    public static explicit operator BiologicalSex(AsciiString value)
        => FromValue(value);

    static AsciiString global::DSE.Open.IConvertibleTo<BiologicalSex, AsciiString>.ConvertTo(BiologicalSex value)
        => (AsciiString)value;

    public static explicit operator AsciiString(BiologicalSex value)
    {
        value.EnsureInitialized();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(BiologicalSex other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is BiologicalSex other && Equals(other);

    public override int GetHashCode()
    {
        EnsureInitialized();
        return HashCode.Combine(_value);
    }

    public static bool operator ==(BiologicalSex left, BiologicalSex right) => left.Equals(right);
    
    public static bool operator !=(BiologicalSex left, BiologicalSex right) => !(left == right);

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
    /// Gets a representation of the BiologicalSex value as a string with formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the BiologicalSex value.
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
    /// Gets a representation of the BiologicalSex value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the BiologicalSex value.
    /// </returns>
    public override string ToString() => ToString(default, default);

    // ISpanParsable<T>

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out BiologicalSex result)
        => global::DSE.Open.Values.ValueParser.TryParse<BiologicalSex, AsciiString>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out BiologicalSex result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out BiologicalSex result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out BiologicalSex result)
        => global::DSE.Open.Values.ValueParser.TryParse<BiologicalSex, AsciiString>(s, provider, out result);

    public static bool TryParse(
        string? s,
        out BiologicalSex result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out BiologicalSex result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public static BiologicalSex Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<BiologicalSex, AsciiString>(s, provider);

    public static BiologicalSex Parse(ReadOnlySpan<char> s)
        => Parse(s, default);

    public static BiologicalSex Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<BiologicalSex, AsciiString>(s, provider);

    public static BiologicalSex Parse(string s)
        => Parse(s, default);

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
        => _value.TryFormat(utf8Destination, out bytesWritten, format, provider);

    public static BiologicalSex Parse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider)
    => global::DSE.Open.Values.ValueParser.Parse<BiologicalSex, AsciiString>(utf8Source, provider);

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Source,
        IFormatProvider? provider,
        out BiologicalSex result)
        => global::DSE.Open.Values.ValueParser.TryParse<BiologicalSex, AsciiString>(utf8Source, provider, out result);
}
