// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using DSE.Open.Text.Json.Serialization;

namespace DSE.Open;

/// <summary>
/// An immutable sequence of ASCII bytes.
/// </summary>
/// <remarks>
/// Implements <see cref="ISpanFormattable"/> and <see cref="ISpanParsable{TSelf}"/> over
/// a <see cref="ReadOnlyMemory{T}"/> of <see cref="AsciiChar"/>.
/// </remarks>
[JsonConverter(typeof(JsonStringAsciiStringConverter))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct AsciiString
    : IEnumerable<AsciiChar>,
        IEquatable<AsciiString>,
        IEquatable<ReadOnlyMemory<AsciiChar>>,
        IComparable<AsciiString>,
        IEqualityOperators<AsciiString, AsciiString, bool>,
        ISpanFormattable,
        ISpanParsable<AsciiString>,
        IUtf8SpanFormattable,
        IUtf8SpanParsable<AsciiString>
{
    private readonly ReadOnlyMemory<AsciiChar> _value;

    public AsciiString(ReadOnlyMemory<AsciiChar> value)
    {
        _value = value;
    }

    public AsciiChar this[int i] => _value.Span[i];

    public AsciiString Slice(int start, int length) => new(_value.Slice(start, length));

    public bool IsEmpty => _value.IsEmpty;

    public int Length => _value.Length;

    /// <summary>
    /// Searches for the specified value and returns the index of its first occurrence. If not found,
    /// returns -1. Values are compared using IEquatable{T}.Equals(T).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public int IndexOf(AsciiChar value) => _value.Span.IndexOf(value);

    public ReadOnlyMemory<AsciiChar> AsMemory() => _value;

    public ReadOnlySpan<AsciiChar> Span => _value.Span;

    public AsciiChar[] ToArray() => _value.ToArray();

    public byte[] ToByteArray()
    {
        if (_value.IsEmpty)
        {
            return Array.Empty<byte>();
        }

        var result = new byte[_value.Length];

        ValuesMarshal.AsBytes(_value.Span).CopyTo(result);

        return result;
    }

    public char[] ToCharArray()
    {
        if (_value.IsEmpty)
        {
            return Array.Empty<char>();
        }

        var result = new char[_value.Length];

        var status = Ascii.ToUtf16(ValuesMarshal.AsBytes(_value.Span), result, out _);

        Debug.Assert(status == OperationStatus.Done);

        return result;
    }

    public static AsciiString Parse(ReadOnlySpan<char> s) => Parse(s, default);

    public static AsciiString Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"'{s}' is not a valid {nameof(AsciiString)} value.");
        return default; // unreachable
    }

    public static AsciiString Parse(string s) => Parse(s, default);

    public static AsciiString Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        out AsciiString result)
        => TryParse(s, default, out result);

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        out AsciiString result)
    {
        AsciiChar[]? rented = null;

        try
        {
            Span<AsciiChar> buffer = s.Length <= StackallocThresholds.MaxCharLength
                ? stackalloc AsciiChar[s.Length]
                : (rented = ArrayPool<AsciiChar>.Shared.Rent(s.Length));

            var status = Ascii.FromUtf16(s, ValuesMarshal.AsBytes(buffer), out var bytesWritten);

            if (status == OperationStatus.InvalidData)
            {
                result = default;
                return false;
            }

            Debug.Assert(status == OperationStatus.Done);

            result = new AsciiString(buffer[..bytesWritten].ToArray());
            return true;
        }
        finally
        {
            if (rented is not null)
            {
                ArrayPool<AsciiChar>.Shared.Return(rented);
            }
        }
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        out AsciiString result)
        => TryParse(s, default, out result);

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        out AsciiString result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }

    public static AsciiString Parse(ReadOnlySpan<byte> utf8Text) => Parse(utf8Text, default);

    public static AsciiString Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
    {
        if (TryParse(utf8Text, provider, out var result))
        {
            return result;
        }

        ThrowHelper.ThrowFormatException($"'{Encoding.UTF8.GetString(utf8Text)}' is not a valid {nameof(AsciiString)} value.");
        return default; // unreachable
    }

    public static bool TryParse(
        ReadOnlySpan<byte> utf8Text,
        IFormatProvider? provider,
        out AsciiString result)
    {
        if (!Ascii.IsValid(utf8Text))
        {
            result = default;
            return false;
        }

        var buffer = new AsciiChar[utf8Text.Length];

        utf8Text.CopyTo(ValuesMarshal.AsBytes(buffer));

        result = new AsciiString(buffer);
        return true;
    }

    public int CompareTo(AsciiString other) => _value.Span.SequenceCompareTo(other._value.Span);

    public int CompareToCaseInsensitive(AsciiString other)
        => CompareToCaseInsensitive(other._value.Span);

    public int CompareToCaseInsensitive(ReadOnlySpan<AsciiChar> asciiBytes)
    {
        var length = Math.Min(_value.Length, asciiBytes.Length);

        for (var i = 0; i < length; i++)
        {
            var c = AsciiChar.CompareToCaseInsensitive(_value.Span[i], asciiBytes[i]);

            if (c != 0)
            {
                return c;
            }
        }

        return _value.Length - asciiBytes.Length;
    }

    public bool Equals(ReadOnlySpan<AsciiChar> other) => _value.Span.SequenceEqual(other);

    public bool Equals(ReadOnlyMemory<AsciiChar> other) => Equals(other.Span);

    public bool Equals(AsciiString other) => Equals(other._value);

    public bool Equals(string other) => Equals(other.AsSpan());

    public bool Equals(ReadOnlySpan<char> other) => Ascii.Equals(ValuesMarshal.AsBytes(_value.Span), other);

    public bool EqualsCaseInsensitive(AsciiString other) => _value.Span.SequenceEqualsCaseInsensitive(other._value.Span);

    public bool EqualsCaseInsensitive(ReadOnlySpan<char> other) => Ascii.EqualsIgnoreCase(ValuesMarshal.AsBytes(_value.Span), other);

    public bool EqualsCaseInsensitive(string other) => EqualsCaseInsensitive(other.AsSpan());

    public override bool Equals(object? obj) => obj is AsciiString other && Equals(other);

    public override int GetHashCode()
    {
        var c = new HashCode();
        c.AddBytes(ValuesMarshal.AsBytes(_value.Span));
        return c.ToHashCode();
    }

    public AsciiString ToLower()
    {
        var result = new AsciiChar[_value.Length];
        TryFormat(ValuesMarshal.AsBytes(result), out _, "L", default);
        return new AsciiString(result);
    }

    public AsciiString ToUpper()
    {
        var result = new AsciiChar[_value.Length];
        TryFormat(ValuesMarshal.AsBytes(result), out _, "U", default);
        return new AsciiString(result);
    }

    public override string ToString() => ToString(null, null);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (_value.IsEmpty)
        {
            return string.Empty;
        }

        return string.Create(_value.Length, (Value: this, format, formatProvider),
            (buffer, state) => { state.Value.TryFormat(buffer, out _, state.format, state.formatProvider); });
    }

    public string ToStringLower() => ToString("L", null);

    public string ToStringUpper() => ToString("U", null);

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (_value.IsEmpty)
        {
            charsWritten = 0;
            return true;
        }

        if (_value.Length > destination.Length)
        {
            charsWritten = 0;
            return false;
        }

        return format switch
        {
            "L" => Ascii.ToLower(ValuesMarshal.AsBytes(_value.Span), destination, out charsWritten),
            "U" => Ascii.ToUpper(ValuesMarshal.AsBytes(_value.Span), destination, out charsWritten),
            _ => Ascii.ToUtf16(ValuesMarshal.AsBytes(_value.Span), destination, out charsWritten),
        } == OperationStatus.Done;
    }

    public bool TryFormat(
        Span<byte> utf8Destination,
        out int bytesWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        if (_value.IsEmpty)
        {
            bytesWritten = 0;
            return true;
        }

        if (_value.Length > utf8Destination.Length)
        {
            bytesWritten = 0;
            return false;
        }

        switch (format)
        {
            case "L":
                return Ascii.ToLower(ValuesMarshal.AsBytes(_value.Span), utf8Destination, out bytesWritten) == OperationStatus.Done;
            case "U":
                return Ascii.ToUpper(ValuesMarshal.AsBytes(_value.Span), utf8Destination, out bytesWritten) == OperationStatus.Done;
            default:
                bytesWritten = _value.Span.Length;
                return ValuesMarshal.AsBytes(_value.Span).TryCopyTo(utf8Destination);
        }
    }

    public IEnumerator<AsciiChar> GetEnumerator()
    {
        for (var i = 0; i < _value.Length; i++)
        {
            yield return _value.Span[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool EndsWith(AsciiString value) => EndsWith(value._value.Span);

    public bool EndsWith(ReadOnlySpan<byte> value) => EndsWith(ValuesMarshal.AsAsciiChars(value));

    public bool EndsWith(ReadOnlySpan<AsciiChar> value) => _value.Span.EndsWith(value);

    public bool EndsWith(string value) => EndsWith(value.AsSpan());

    public bool EndsWith(ReadOnlySpan<char> value)
    {
        Span<AsciiChar> buffer = value.Length <= StackallocThresholds.MaxByteLength
            ? stackalloc AsciiChar[value.Length]
            : new AsciiChar[value.Length];

        if (!TryToAsciiChars(value, buffer, out var bytesWritten))
        {
            return false;
        }

        return _value.Span.EndsWith(buffer[..bytesWritten]);
    }

    public bool StartsWith(AsciiString value) => StartsWith(value._value.Span);

    public bool StartsWith(ReadOnlySpan<byte> value) => StartsWith(ValuesMarshal.AsAsciiChars(value));

    public bool StartsWith(ReadOnlySpan<AsciiChar> value) => _value.Span.StartsWith(value);

    public bool StartsWith(string value) => StartsWith(value.AsSpan());

    public bool StartsWith(ReadOnlySpan<char> value)
    {
        Span<AsciiChar> buffer = value.Length <= StackallocThresholds.MaxByteLength
            ? stackalloc AsciiChar[value.Length]
            : new AsciiChar[value.Length];

        if (!TryToAsciiChars(value, buffer, out var bytesWritten))
        {
            return false;
        }

        return _value.Span.StartsWith(buffer[..bytesWritten]);
    }

    private static bool TryToAsciiChars(ReadOnlySpan<char> value, Span<AsciiChar> buffer, out int bytesWritten)
    {
        var status = Ascii.FromUtf16(value, ValuesMarshal.AsBytes(buffer), out bytesWritten);

        if (status != OperationStatus.Done)
        {
            Debug.Assert(status == OperationStatus.InvalidData);
            return false;
        }

        return true;
    }


    public int LastIndexOf(AsciiChar c) => Span.LastIndexOf(c);

    public static bool operator ==(AsciiString left, AsciiString right) => left.Equals(right);

    public static bool operator !=(AsciiString left, AsciiString right) => !(left == right);

    public static bool operator ==(AsciiString left, string right) => left.Equals(right);

    public static bool operator !=(AsciiString left, string right) => !(left == right);

#pragma warning disable CA2225 // Operator overloads have named alternates (Parse)

    public static explicit operator AsciiString(string value) => Parse(value);

#pragma warning restore CA2225 // Operator overloads have named alternates

    public static bool operator <(AsciiString left, AsciiString right) => left.CompareTo(right) < 0;

    public static bool operator <=(AsciiString left, AsciiString right) => left.CompareTo(right) <= 0;

    public static bool operator >(AsciiString left, AsciiString right) => left.CompareTo(right) > 0;

    public static bool operator >=(AsciiString left, AsciiString right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Converts the <see cref="AsciiString"/> to a <see cref="CharSequence"/>.
    /// </summary>
    public CharSequence ToCharSequence() => CharSequence.FromAsciiString(this);
}
