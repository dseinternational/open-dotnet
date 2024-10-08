﻿// <auto-generated/>

#nullable enable annotations
#nullable disable warnings

#pragma warning disable CA2225 // Operator overloads have named alternates


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DSE.Open.Runtime.Helpers;
using DSE.Open.Values;

namespace DSE.Open.Values;

[TypeConverter(typeof(global::DSE.Open.Values.ValueTypeConverter<Label, CharSequence>))]
public readonly partial struct Label
{
    private readonly CharSequence _value;

    private Label(CharSequence value, bool skipValidation = false)
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
                $"'{value}' is not a valid {nameof(Label)} value");
        }
    }

    private void EnsureIsNotDefault()
    {
        UninitializedValueException<Label, CharSequence>.ThrowIfUninitialized(this);
    }

    public static bool TryFromValue(CharSequence value, out Label result)
    {
        if (IsValidValue(value))
        {
            result = new Label(value, true);
            return true;
        }
    
        result = default;
        return false;
    }

    public static Label FromValue(CharSequence value)
    {
        EnsureIsValidValue(value);
        return new(value, true);
    }

    public static explicit operator Label(CharSequence value)
        => FromValue(value);

    static CharSequence global::DSE.Open.IConvertibleTo<Label, CharSequence>.ConvertTo(Label value)
        => (CharSequence)value;

    public static implicit operator CharSequence(Label value)
    {
        value.EnsureIsNotDefault();
        return value._value;
    }

    // IEquatable<T>

    public bool Equals(Label other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Label other && Equals(other);

    public override int GetHashCode()
    {
        EnsureIsNotDefault();
        return _value.GetHashCode();
    }

    // IEqualityOperators<Label, Label, bool>

    public static bool operator ==(Label left, Label right) => left.Equals(right);
    
    public static bool operator !=(Label left, Label right) => !(left == right);

    // ISpanFormattable

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        EnsureIsNotDefault();
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
    /// Gets a representation of the <see cref="Label"/> value as a string with formatting options.
    /// </summary>
    [SkipLocalsInit]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        EnsureIsNotDefault();

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
    /// Gets a representation of the Label value as a string with default formatting options.
    /// </summary>
    /// <returns>
    /// A representation of the Label value.
    /// </returns>
    public override string ToString()
    {
        return ToString(default, default);
    }

    // ISpanParsable<Label>

    public static Label Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Label, CharSequence>(s, provider);

    public static Label ParseInvariant(ReadOnlySpan<char> s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out Label result)
        => global::DSE.Open.Values.ValueParser.TryParse<Label, CharSequence>(s, provider, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out Label result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        ReadOnlySpan<char> s,
        out Label result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    // IParsable<Label>

    public static Label Parse(string s, IFormatProvider? provider)
        => global::DSE.Open.Values.ValueParser.Parse<Label, CharSequence>(s, provider);

    public static Label Parse(string s)
        => Parse(s, default);

    public static Label ParseInvariant(string s)
        => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    public static bool TryParse(
        string? s,
        IFormatProvider? provider,
        out Label result)
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
        out Label result)
        => TryParse(s, default, out result);

    public static bool TryParseInvariant(
        string? s,
        out Label result)
        => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    public int CompareTo(Label other)
    {
        EnsureIsNotDefault();

        return _value.CompareTo(other._value);
    }

    // IComparisonOperators<Label, Label, bool>

    public static bool operator <(Label left, Label right) => left.CompareTo(right) < 0;
    
    public static bool operator >(Label left, Label right) => left.CompareTo(right) > 0;
    
    public static bool operator <=(Label left, Label right) => left.CompareTo(right) <= 0;
    
    public static bool operator >=(Label left, Label right) => left.CompareTo(right) >= 0;

}

